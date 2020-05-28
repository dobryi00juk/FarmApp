using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmAppServer.Models.Drug
{
    public class DrugDto
    {
        //[Required] public int Id { get; set; }
        [Required] public string DrugName { get; set; }
        [Required] public int CodeAthTypeId { get; set; }
        [Required] public int VendorId { get; set; }
        [Required] public bool IsGeneric { get; set; }
        //[Required] public bool IsDeleted { get; set; }
    }
}
