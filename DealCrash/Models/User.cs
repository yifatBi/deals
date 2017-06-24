using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DealCrash.Enums;

namespace DealCrash.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("E-mail")]
        public string email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("password")]
        public string confirmPassword { get; set; }

        [DisplayName("Author name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string name { get; set; }

        [DisplayName("Link")]
        public string link { get; set; }

        [DisplayName("Gender")]
        public Gender gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Birth Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime birthDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("Created at")]
        public DateTime createdDate { get; set; }

        [DisplayName("Is Admin?")]
        public Boolean admin { get; set; }

        [DisplayName("Active?")]
        public Boolean active { get; set; }

        [DisplayName("Comments")]
        public virtual List<Comment> comments { get; set; }

        [DisplayName("Deals")]
        public virtual List<Deal> deals { get; set; }
    }
}