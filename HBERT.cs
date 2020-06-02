using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using RevitFilledRegionType = Autodesk.Revit.DB.FilledRegionType;
using HBERT.Infrastructure;

using Autodesk.Revit.UI;
using HBERT_UI;
using HBERT_UI.Infrastructure;

namespace HBERT
{
    internal class HBERT : IExternalCommand
    {
        internal HBERT() { }

        /// <summary>
        /// Computes the embodied carbon of the scheme using the Embodied Carbon Shedule
        /// </summary>
        internal void ComputeEmbodiedCarbon(Document doc)
        {
            HbertMainForm hbertMainForm = new HbertMainForm(doc);
            System.Windows.Forms.Application.Run(hbertMainForm);

            bool executeStepOne = ExceptionUtils.UserClosedForm<HbertMainForm>(hbertMainForm);

            if (executeStepOne)
            {
                string sheetNumber = "HBERT";
                string sheetName = "EC Evaluation";

                ProjectDetails projectDetails = hbertMainForm.ActiveProjectDetails;

                SheetInputsForm sheetInputsForm;
                //<<<<----START THE TRANSACTION---->>>>
                using (Transaction transaction = new Transaction(doc, "HBERT"))
                {
                    transaction.Start();

                    sheetInputsForm = new SheetInputsForm(projectDetails, sheetNumber, sheetName);
                    System.Windows.Forms.Application.Run(sheetInputsForm);

                    transaction.Commit();
                }

                bool executeStepTwo = ExceptionUtils.UserClosedForm<SheetInputsForm>(sheetInputsForm);

                if (executeStepTwo)
                {
                    FamilySymbol titleBlock = projectDetails.TitleBlock;
                    ViewSchedule embodiedCarbonSchedule = projectDetails.CarbonSchedule;
                    View3D axoView = projectDetails.AxoView;
                    string projectName = projectDetails.ProjectName, location = projectDetails.ProjectAddress, floorArea = projectDetails.FloorArea.ToString() + " m²", ribaWorkstage = projectDetails.RibaWorkstage, projectVersion = projectDetails.ProjectVersion, sector = projectDetails.Sector;
                    bool newBuild = projectDetails.NewBuild;

                    DateTime now = DateTime.Now;

                    bool isValidSchedule = true;
                    ExceptionUtils.ValidateCarbonSchedule(embodiedCarbonSchedule, ref isValidSchedule);

                    double convertToFt = 304.8;
                    double convertPointToMm = 4.347826087; //Converts the fontSizes from point to mm
                    XYZ zVec = Transform.Identity.BasisZ;

                    string projectType = newBuild ? "New Build" : "Refurbishment";

                    //The dimensions of the tree chart
                    double width = 164.0 / convertToFt;
                    double height = 112.0 / convertToFt;

                    string floorAreaText = floorArea + " m²";
                    string date = now.Day.ToString() + "." + now.Month.ToString() + "." + now.Year.ToString();
                    int redColour = Utilities.ConvertColourToInt(232, 70, 16);

                    //<<<<----START THE TRANSACTION---->>>>
                    using (Transaction transaction = new Transaction(doc, "HBERT"))
                    {
                        transaction.Start();

                        ViewSheet oldECSheet = Utilities.GetOldECSheet(doc, sheetName, sheetNumber);
                        bool sheetIsActive = Utilities.DeleteOldECSheet(doc, oldECSheet); //If HBERT has been run once, then delete the old sheet 

                        if (sheetIsActive)
                        {
                            ExceptionUtils.SheetIsActive();
                            return;
                        }

                        //Create the annotation style dictionary
                        Annotation.GenerateStyleDictionary(doc, convertToFt, convertPointToMm, redColour);

                        //Generate the list of filled region types from the document. They are used for creating the charts using HB colour codes
                        FilledRegionHelper.CreateAll(doc);
                        Dictionary<string, RevitFilledRegionType> filledRegionTypeDictionary = FilledRegionHelper.CreateTypeDictionary(doc);

                        //Get the Embodied Carbon data from the schedule
                        List<KeyValuePair<string, double>> eCData = Utilities.GetScheduleData(doc, embodiedCarbonSchedule, 0.025);

                        //Check if there is any data in the eCData and throw an exception is there is nothing found in the schedule
                        //If the ebodied carbon sheet is active it cant be deleted which prevents the process from re-running. Inform the user so they can activate another view to circumvent the problem
                        if (eCData == null)
                        {
                            ExceptionUtils.ScheduleContainsNoData();
                            return;
                        }

                        // Prepare the data for the charting. Values must be sorted descending (and positive, obviously)
                        eCData.Sort((x, y) => y.Value.CompareTo(x.Value));
                        List<KeyValuePair<string, double>> eCvaluesNormalized = TreeChart.NormalizeSizes(eCData, width, height);

                        //Create the new sheet to present the data 
                        ViewSheet newSheet = ViewSheet.Create(doc, titleBlock.Id);

                        newSheet.Name = sheetName;
                        newSheet.SheetNumber = sheetNumber;

                        ViewDrafting newTreeViewDrawing = ViewDrafting.Create(doc, Utilities.GetDraftingViewTypeId(doc));
                        ViewDrafting newBarChartDrawing = ViewDrafting.Create(doc, Utilities.GetDraftingViewTypeId(doc));
                        newTreeViewDrawing.Scale = 1;
                        newBarChartDrawing.Scale = 1;

                        //The x and y values define the coordinate system for the returned rectangles. The values will range from x to x + width and y to y + height
                        List<Dictionary<string, object>> paddedRects = TreeChart.PaddedSquarify(eCvaluesNormalized, 246.0 / convertToFt, 66.0 / convertToFt, width, height);

                        //Create the tree graph
                        Annotation annotateTreeGraph = new Annotation();
                        List<FilledRegion> treeGraph = new TreeChart().GenerateTreeGraph(doc, paddedRects, filledRegionTypeDictionary, newTreeViewDrawing.Id, annotateTreeGraph, convertToFt);

                        //Create the bar graph
                        Annotation annotateBarGraph = new Annotation();
                        List<FilledRegion> barGraph = new StackedBarChart().GenerateStackedBars(doc, paddedRects, filledRegionTypeDictionary, newBarChartDrawing.Id, annotateBarGraph, 10.0 / convertToFt, 130.0 / convertToFt, 0.0, 0.0, 0.5 / convertToFt);

                        doc.Regenerate();

                        Viewport viewportTreeGraph = Viewport.Create(doc, newSheet.Id, newTreeViewDrawing.Id, new XYZ(328.0 / convertToFt, 96.0 / convertToFt, 0.0));
                        Viewport viewportBarGraph = Viewport.Create(doc, newSheet.Id, newBarChartDrawing.Id, new XYZ(340.0 / convertToFt, 214.5 / convertToFt, 0.0));
                        Viewport viewport3D = Viewport.Create(doc, newSheet.Id, axoView.Id, new XYZ(126.0 / convertToFt, 150.0 / convertToFt, 0.0));

                        Utilities.SetViewportType(doc, viewportTreeGraph);
                        Utilities.SetViewportType(doc, viewportBarGraph);
                        Utilities.SetViewportType(doc, viewport3D);
                        //Generate all sheet annotation

                        //Project headings
                        double maxTextNoteWidth = 160.0 / convertToFt;

                        var projectNameHeading = new Annotation().CreateTextNote(doc, newSheet.Id, new XYZ(10.0 / convertToFt, 267.0 / convertToFt, 0.0), Annotation.FontSize.Thirty, redColour, maxTextNoteWidth, projectName, true, false);
                        var projectVersionHeading = new Annotation().CreateTextNote(doc, newSheet.Id, new XYZ(10.0 / convertToFt, 255.0 / convertToFt, 0.0), Annotation.FontSize.Thirty, redColour, maxTextNoteWidth, projectVersion, false, false);
                        var projectInfoAnnotation = new Annotation().ProjectInfo(doc, newSheet.Id, Annotation.FontSize.Ten, 10.0, 240.0, -6.32, redColour, convertToFt, new string[6] { date, ribaWorkstage, location, floorArea, projectType, sector });

                        //TreeGraph Annotations
                        var treeGraphHeading = new Annotation().CreateTextNote(doc, newSheet.Id, new XYZ(246.0 / convertToFt, 158.0 / convertToFt, 0.0), Annotation.FontSize.Sixteen, redColour, maxTextNoteWidth, "Embodied Carbon per Material", true, false);
                        var treeGraphAnnotationElements = new Annotation().AnnotateGraph(doc, newTreeViewDrawing.Id, annotateTreeGraph, 35.0, convertToFt, Utilities.ConvertColourToInt(254, 254, 254), false);

                        //Bar Chart annotations
                        var barGraphHeading = new Annotation().CreateTextNote(doc, newSheet.Id, new XYZ(246.0 / convertToFt, 263.0 / convertToFt, 0.0), Annotation.FontSize.Sixteen, redColour, maxTextNoteWidth, "Total Embodied Carbon", true, false);
                        //The sub-title to the left of the bar chart
                        var barChartSubHeading = new Annotation().CreateTextNote(doc, newBarChartDrawing.Id, new XYZ(-27.0 / convertToFt, 10.0 / convertToFt, 0.0), Annotation.FontSize.Eleven, redColour, 25.0 / convertToFt, projectName, true, false);
                        var barChartAnnotationElements = new Annotation().AnnotateGraph(doc, newBarChartDrawing.Id, annotateBarGraph, 35.0, convertToFt, redColour, true);

                        //Calculation of TotalEmbodiesCarbon as headline figure (on bar chart view)
                        var barChartTotalEmbodiedCarbon = new Annotation().CreateTextNote(doc, newBarChartDrawing.Id, new XYZ(-27.0 / convertToFt, -12.0 / convertToFt, 0.0), Annotation.FontSize.Eleven, redColour, 25.0 / convertToFt, "Total Embodied Carbon", false, false);
                        var barChartAveragePerSqM = new Annotation().CreateTextNote(doc, newBarChartDrawing.Id, new XYZ(-27.0 / convertToFt, -27.0 / convertToFt, 0.0), Annotation.FontSize.Eleven, redColour, 25.0 / convertToFt, "Average per m² of Floor Area", false, false);

                        double total = eCData.Sum(v => v.Value);
                        double average = total / projectDetails.FloorArea;
                        string totalText = Math.Round(total, 0).ToString();

                        int decimalPlaces = average < 1 ? 2 : 0;
                        string averageText = Math.Round(average * 1000.0, decimalPlaces).ToString(); // Needs to be converted to kg from tons, so multiply by 1000.

                        var totalEmbodiedCarbon = new Annotation().CreateTextNote(doc, newBarChartDrawing.Id, new XYZ(7.0 / convertToFt, -8.5 / convertToFt, 0.0), Annotation.FontSize.Thirty, Utilities.ConvertColourToInt(0,0,0), 120.0 / convertToFt, totalText + " ton CO₂e", false, false);
                        var averageEmbodiedCarbon = new Annotation().CreateTextNote(doc, newBarChartDrawing.Id, new XYZ(7.0 / convertToFt, -24.5 / convertToFt, 0.0), Annotation.FontSize.Thirty, Utilities.ConvertColourToInt(0, 0, 0), 120.0 / convertToFt, averageText + "kg CO₂e/m²", false, false);

                        //Set the number part of the totalEmbodiedCarbon and averageEmbodiedCarbon titles to bold
                        Annotation.SetBoldCharacters(totalEmbodiedCarbon, 0, totalText.Length);
                        Annotation.SetBoldCharacters(averageEmbodiedCarbon, 0, averageText.Length);

                        //<<<<----END THE TRANSACTION---->>>>
                        transaction.Commit();

                        GoogleCloud.UploadToCloud(projectDetails, eCData);
                    };
                }
            }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            throw new NotImplementedException();
        }
    }
}
