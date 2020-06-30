using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;

namespace CarbonEmissionTool.Models
{
    /// <summary>
    /// Represents a heading on a sheet.
    /// </summary>
    public interface IHeading
    {
        View PlacementView { get; }

        XYZ Origin { get; }

        FontSize FontSize { get; }

        BoldFormatter BoldFormatter { get; }

        HorizontalTextAlignment HorizontalAlignment { get; }

        ColorData Color { get; }

        double TextNoteWidth { get; }

        string Title { get; }

        bool Vertical { get; }
    }
}
