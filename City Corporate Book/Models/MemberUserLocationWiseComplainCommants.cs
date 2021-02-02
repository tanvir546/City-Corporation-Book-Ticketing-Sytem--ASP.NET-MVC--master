using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace City_Corporate_Book.Models
{
    public class MemberUserLocationWiseComplainCommants
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int Status { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        [EmailAddress]
        [StringLength(220)]
        public string Email { get; set; }
        
    }
}