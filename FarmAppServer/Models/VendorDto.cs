using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmAppServer.Models
{
    public class VendorDto
    {
        [Required]
        public string VendorName { get; set; }
        [Required]
        public string ProducingCountry { get; set; }
        public bool IsDomestic { get; set; }
    }
}
