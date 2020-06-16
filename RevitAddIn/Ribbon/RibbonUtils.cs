using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using RevitAddIn.Interfaces;

////////////////////------------------BIMORPH DIGITAL ENGINEERING CONSULTANCY------------------\\\\\\\\\\\\\\\\\\\\\\\
//                                                                                                                  \\
// Copyright 2019. All rights reserved. Bimorph Consultancy LTD, 5 St Johns Lane, London EC1M 4BH                   \\
// Developed and written by Thomas Mahon @Thomas__Mahon info@bimorph.com                                            \\
// Website: https://bimorph.com                                                                                     \\
// Follow: facebook.com/bimorphBIM | linkedin.com/company/bimorph-bim | @bimorphBIM                                 \\
///////////////////////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

namespace RevitAddIn.Ribbon
{
    /// <summary>
    /// Class used to instantiate a new button in the Revit ribbon.
    /// </summary>
    class RibbonUtils
    {
        /// <summary> The address of the executing assembly.</summary>
        static string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

        /// <summary>
        /// Constructs a new button on the given revit ribbon and provides the data to construct the <see cref="PushButtonData"/>
        /// and <see cref="PushButton"/>.
        /// </summary>
        /// <param name="ribbonPanel"> The ribbon panel where this button will appear.</param>
        /// <param name="buttonData"> The button data which this button triggers.</param>
        public static void AddButton(RibbonPanel ribbonPanel, IButtonData buttonData)
        {
            PushButtonData pushButtonData = new PushButtonData(buttonData.InternalButtonName, buttonData.VisibleButtonName, thisAssemblyPath, buttonData.GetType().FullName);
            PushButton pushButton = (PushButton)ribbonPanel.AddItem(pushButtonData);
            pushButton.ToolTip = buttonData.ToolTip;

            Uri uriImage = new Uri($"{Path.GetDirectoryName(thisAssemblyPath)}\\Resources\\{buttonData.IconName}");

            BitmapImage largeImage = new BitmapImage(uriImage);

            pushButton.LargeImage = largeImage;

        }

        /// <summary>
        /// Adds a new ribbon tab to Revits UI with the given tab name.
        /// </summary>
        public static void AddHbRibbonTab(UIControlledApplication application, string tabName)
        {
            // Create a custom ribbon tab
            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch
            {
                // Ribbon bar should already exist.
            }
        }
    }
}
