namespace BeSpoken.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Sale
    {
        public int SalesId { get; set; }
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Salesperson")]
        public int SalesPersonId { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Sales Date")]
        public System.DateTime SalesDate { get; set; }

        [Required]
        public Nullable<int> Qty { get; set; }

        public Nullable<decimal> Amount { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
        public virtual SalesPerson SalesPerson { get; set; }
    }
}
