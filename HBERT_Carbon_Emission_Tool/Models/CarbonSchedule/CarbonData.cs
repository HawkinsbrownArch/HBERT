namespace CarbonEmissionTool.Models
{
    /// <summary>
    /// Represents a material and embodied carbon value extracted from the EC schedule in Revit.
    /// </summary>
    public class CarbonData
    {
        /// <summary>
        /// The material name.
        /// </summary>
        public string MaterialName { get; }

        /// <summary>
        /// The embodied carbon value.
        /// </summary>
        public double EmbodiedCarbon { get; }

        /// <summary>
        /// Constructs a new <see cref="CarbonData"/> object.
        /// </summary>
        public CarbonData(string materialName, double embodiedCarbon)
        {
            this.MaterialName = materialName;
            this.EmbodiedCarbon = embodiedCarbon;
        }
    }
}
