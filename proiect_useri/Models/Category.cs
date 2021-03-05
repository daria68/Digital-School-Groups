using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Numele este obligatoriu")]
        [StringLength(50, ErrorMessage = "Numele nu poate avea mai mult de 50 caractere")]
        public string CategoryName { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}