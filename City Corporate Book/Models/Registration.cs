using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace City_Corporate_Book.Models
{
    public class Registration
    {
        [Key]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [Required]
        [StringLength(23)]
        public string VoterId { get; set; }

        [Required]
        public byte LoginIdentity { get; set; }

    }
}