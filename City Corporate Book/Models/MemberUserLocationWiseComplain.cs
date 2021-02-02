using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace City_Corporate_Book.Models
{
    public class MemberUserLocationWiseComplain
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public byte Status { get; set; }
        
        [Required]
        [Key]
        public int StatusNo { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Post { get; set; }
        
        public int Role { get; set; }

        public int Agree { get; set; }

        public int Disagree { get; set; }

    }
}