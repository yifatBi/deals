using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DealCrash.Models
{
    public class Deal
    {
        public int DealID { get; set; }

        [Required]
        [ForeignKey("user")]
        [DisplayName("User ID")]
        public int UserID { get; set; }

        [Required]
        [MaxLength(150)]
        [DisplayName("Deal Title")]
        public string title { get; set; }

        [Required]
        [DisplayName("Text")]
        public string text { get; set; }

        [Required]
        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Image Link")]
        public string image { get; set; }

        [DisplayName("Address of the store")]
        public string address { get; set; }

        [DisplayName("The Facebook of the store")]
        public string facebook { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("Created at")]
        public DateTime createdDate { get; set; }

        [DisplayName("Comments")]
        public virtual List<Comment> comments { get; set; }

        public virtual User user { get; set; }
    }
}