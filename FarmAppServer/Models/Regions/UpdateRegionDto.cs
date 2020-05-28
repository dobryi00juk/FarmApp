using System.ComponentModel.DataAnnotations;

namespace FarmAppServer.Models.Regions
{
    public class UpdateRegionDto
    {
        [Required] public int RegionId { get; set; }
        [Required] public int RegionTypeId { get; set; }
        [Required] public string RegionName { get; set; }
        [Required] public int Population { get; set; }
    }
}
