namespace CarbonEmissionTool.Model.Utilities
{
    class StringUtils
    {
        /// <summary>
        /// Concatenates the sector types into a comma delineated string for representation on a Revit sheet
        /// </summary>
        internal static string GenerateSectorString(string[] sectorList)
        {
            string sector = "";
            for (int i = 0; i < sectorList.Length; i++)
            {
                string currentSector = sectorList[i];

                if (currentSector != "")
                    sector += currentSector + ", ";
            }

            if (sector.EndsWith(", "))
                sector = sector.Remove(sector.Length - 2, 2);

            return sector;
        }


    }
}
