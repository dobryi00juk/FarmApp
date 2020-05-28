using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmAppServer.Models.Regions
{
    public class PostRegionDto
    {
        [Required] public int RegionId { get; set; }
        [Required] public int RegionTypeId { get; set; }
        [Required] public string RegionName { get; set; }
        [Required] public int Population { get; set; }
    }
}
