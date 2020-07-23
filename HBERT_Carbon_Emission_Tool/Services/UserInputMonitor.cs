using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Models;

namespace CarbonEmissionTool.Services
{
    /// <summary>
    /// Class which stores user input data and re-applies the data if they open the app multiple times in the
    /// same Revit session.
    /// </summary>
    static class UserInputMonitor
    {
        /// <summary>
        /// A list of all the <see cref="InputCapture"/> generated in the current Revit session.
        /// </summary>
        public static List<InputCapture> InputCaptures { get; }

        /// <summary>
        /// Constructs a new <see cref="UserInputMonitor"/>.
        /// </summary>
        static UserInputMonitor()
        {
            InputCaptures = new List<InputCapture>();
        }

        /// <summary>
        /// Adds a <see cref="InputCapture"/> to the monitor.
        /// </summary>
        public static void RegisterUserInputs(IProjectDetails projectDetails)
        {
            var document = ApplicationServices.Document;

            var inputCapture = new InputCapture(ApplicationServices.Document, projectDetails);

            var existingInputCapture = UserInputMonitor.GetCapture(document);

            // If the active document hash is found in the InputCaptures, remove it and replace it 
            // with the new one to ensure only the most recent data inputs are reapplied.
            if (existingInputCapture != null)
            {
                InputCaptures.Remove(existingInputCapture);
            }

            InputCaptures.Add(inputCapture);
        }

        /// <summary>
        /// Returns true if the provided document hash matches with an <see cref="InputCapture"/> stored in this
        /// monitor.
        /// </summary>
        public static bool HasInputCapture(Document document)
        {
            return InputCaptures.Any(capture => capture.DocumentHash == document.GetHashCode());
        }

        /// <summary>
        /// Returns an <see cref="InputCapture"/> which matches with the provided documents hashcode.
        /// </summary>
        public static InputCapture GetCapture(Document document)
        {
            var documentHashcode = document.GetHashCode();

            return InputCaptures.Find(capture => capture.DocumentHash == documentHashcode);
        }
    }
}
