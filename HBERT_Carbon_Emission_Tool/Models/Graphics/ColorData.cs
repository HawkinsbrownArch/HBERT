using System.Windows.Media;

namespace CarbonEmissionTool.Models
{
    public class ColorData
    {
        public string Name { get; set; }

        public Color Color { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ColorData() { }

        /// <summary>
        /// Constructs a new <see cref="ColorData"/>.
        /// </summary>
        public ColorData(string name, Color color)
        {
            this.Name = name;

            this.Color = color;
        }
    }
}
