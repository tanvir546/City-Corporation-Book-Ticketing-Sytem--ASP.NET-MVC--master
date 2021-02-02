using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace City_Corporate_Book.Models
{
    public class CommentAgreeDisagreeStatus
    {
        [Key]
        public int No { get; set; }

        public int PostId { get; set; }

        public int Action { get; set; }

        public string UserEmail { get; set; }
    }
}