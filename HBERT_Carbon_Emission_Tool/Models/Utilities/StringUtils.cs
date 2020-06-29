namespace CarbonEmissionTool.Models
{
    public class StringUtils
    {
        /// <summary>
        /// Concatenates the sector types into a comma delineated string for representation on a Revit sheet
        /// </summary>
        public static string GenerateSectorString(IProjectDetails projectDetails)
        {
            string commaDelineation = ", ";
            int commaLength = commaDelineation.Length;

            string sector = "";

            foreach (var sectorItem in projectDetails.Sectors)
            {
                if (sectorItem.IsSelected)
                {
                    sector += $"{sectorItem}{commaDelineation}";
                }
            }

            if (sector.EndsWith(commaDelineation))
                sector = sector.Remove(sector.Length - commaLength, commaLength);

            return sector;
        }
    }
}
