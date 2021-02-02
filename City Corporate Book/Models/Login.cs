using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace City_Corporate_Book.Models
{
    public class Login
    {
        [Key]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Password { get; set; }
        
    }
}