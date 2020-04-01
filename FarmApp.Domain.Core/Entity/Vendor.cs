﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FarmApp.Domain.Core.Entity
{
    public class Vendor
    {
        public Vendor()
        {
            Drugs = new HashSet<Drug>();
        }
        public int Id { get; set; }
        public string VendorName { get; set; }
        public bool? IsDomestic { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual ICollection<Drug> Drugs { get; set; }
    }
}
