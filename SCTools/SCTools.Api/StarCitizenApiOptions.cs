using System.ComponentModel.DataAnnotations;

namespace NSW.StarCitizen.Tools.API
{
    public class StarCitizenApiOptions
    {
        /// <summary>
        /// Star Citizen root game folder
        /// </summary>
        [Required]
        public string RootFolder { get; set; } = string.Empty;
    }
}
