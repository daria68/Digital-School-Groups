using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect.Models
{
    public class Activity
    {

        [Key]
        public int ActivityId { get; set; }
        [Required]
        public string ActivityName { get; set; }
        [Required]
        public string ActivityDescription { get; set; }
        [Required]
        public string ActivityDay{ get; set; }
        [Required]
        public string ActivityInterval { get; set; }
        [Required]
        public string ActivityLocation { get; set; }


        //cheie externa
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }


      
    }
}