using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;

namespace CarbonEmissionTool.Models
{
    /// <summary>
    /// Represents a heading on a sheet.
    /// </summary>
    public interface IHeading
    {
        Autodesk.Revit.DB.View PlacementView { get; }

        XYZ Origin { get; }

        FontSize FontSize { get; }

        BoldFormatter BoldFormatter { get; }

        System.Windows.Media.Color Color { get; }

        double TextNoteWidth { get; }

        string Title { get; }

        bool Vertical { get; }
    }
}
