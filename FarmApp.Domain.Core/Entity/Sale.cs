using System;
using System.Collections.Generic;
using System.Text;

namespace FarmApp.Domain.Core.Entity
{
    public class Sale
    {
        public long Id { get; set; }
        public int DrugId { get; set; }
        public int PharmacyId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool? IsDiscount { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual Drug Drug { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
    }
}
