namespace BeSpoken.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Sales = new HashSet<Sale>();
        }
    
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        [Required]
        public string Style { get; set; }

        [Required]
        [Display(Name = "Purchase Price")]
        public decimal PurchasePrice { get; set; }

        [Required]
        [Display(Name = "Sale Price")]
        public Nullable<decimal> SalePrice { get; set; }

        [Required]
        [Display(Name = "Qty On Hand")]
        public int QtyOnHand { get; set; }

        [Required]
        [Display(Name = "Commission Percentage")]
        public decimal CommissionPercentage { get; set; }


        public int DiscountId { get; set; }
    
        public virtual Discount Discount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
