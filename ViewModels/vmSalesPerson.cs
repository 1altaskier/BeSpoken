using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeSpoken.ViewModels
{
    public class vmSalesPerson
    {
        public int SalesPersonId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public int Zip { get; set; }

        [Required]
        public string Phone { get; set; }

        [Display(Name = "Alt Phone")]
        public string AltPhone { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Term Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> TermDate { get; set; }

        [Required]
        [Display(Name = "Manager")]
        public int ManagerId { get; set; }

        public string StateAbbrv { get; set; }
        public string StateName { get; set; }
    }
}