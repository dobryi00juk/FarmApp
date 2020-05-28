using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmAppServer.Models.Sales
{
    public class UpdateSaleDto
    {
        public DateTime SaleDate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsDiscount { get; set; }
    }
}
