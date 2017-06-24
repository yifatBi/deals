using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DealCrash.Models
{
    public class Comment
    {
        public int CommentID { get; set; }

        [Required]
        [ForeignKey("user")]
        [DisplayName("User ID")]
        public int UserID { get; set; }

        [Required]
        [ForeignKey("deal")]
        [DisplayName("Deal ID")]
        public int DealID { get; set; }

        [Required]
        [DisplayName("Comment Text")]
        public string text { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Created at")]
        public DateTime createdDate { get; set; }

        public virtual Deal deal { get; set; }

        public virtual User user { get; set; }
    }
}