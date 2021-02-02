using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace City_Corporate_Book.Models.ViewModel
{
    public class RegistrationVModel
    {
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
        public string LoginIdentity { get; set; }

        [Required]
        public string Password { get; set; }
    }
}