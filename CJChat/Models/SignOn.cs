using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CJChat.Models
{
    public class SignOn
    {
        [Required]
        [StringLength(50)]
        public String UserName { get; set; }
        [Required]
        public String Location { get; set; }
        [Required]
        public Boolean Remember { get; set; }
    }
}