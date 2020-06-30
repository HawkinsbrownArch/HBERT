using Autodesk.Revit.UI;

namespace RevitAddIn.Interfaces
{
    /// <summary>
    /// Interface for adding a button to the Revit ribbon.
    /// </summary>
    interface IButtonData : IExternalCommand
    {
        /// <summary> The tooltip that appears when hovering the button in the ribbon menu.</summary>
        string ToolTip { get; }

        /// <summary> The button name the user sees in the ribbon.</summary>
        string VisibleButtonName { get; }

        /// <summary> The internal button name for the developer.</summary>
        string InternalButtonName { get; }

        /// <summary> The name of icon to display </summary>
        string IconName { get; }

    }
}
