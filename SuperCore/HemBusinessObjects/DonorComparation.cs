using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class DonorComparation
    {
        public DonorComparation()
        {
            Donor = new Donor();
            Product = new Product();
        }
        [CSN("Donor")]
        public Donor Donor { get; set; }
        [CSN("Product")]
        public Product Product { get; set; }
    }
}
