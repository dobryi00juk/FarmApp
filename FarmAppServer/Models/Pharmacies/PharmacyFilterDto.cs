using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmAppServer.Models.Pharmacy
{
    public class PharmacyFilterDto
    {
        [Required] public int Id { get; set; }

        [Required] public string PharmacyName { get; set; }

        [Required] public int? PharmacyId { get; set; }
        
        [Required] public string ParentPharmacyName { get; set; }

        [Required] public int RegionId { get; set; }

        [Required] public string RegionName { get; set; }

        [Required] public bool IsMode { get; set; }

        [Required] public bool IsType { get; set; }

        [Required] public bool IsNetwork { get; set; }
        
        [Required] public bool IsDeleted { get; set; }
    }
}
