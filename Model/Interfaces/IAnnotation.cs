using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations.Formatting;
using CarbonEmissionTool.Model.Enums;

namespace CarbonEmissionTool.Model.Interfaces
{
    public interface IAnnotation
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
