using System.Windows.Media;

namespace CarbonEmissionTool.Models
{
    public class ChartColor
    {
        public string Name { get; }

        public Color Color { get; }

        /// <summary>
        /// Constructs a new <see cref="ChartColor"/>.
        /// </summary>
        public ChartColor(string name, Color color)
        {
            this.Name = name;

            this.Color = color;
        }
    }
}
