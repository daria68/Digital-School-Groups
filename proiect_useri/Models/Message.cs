using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using proiect_useri.Models;

namespace proiect.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        [Required(ErrorMessage = "Continutul este obligatoriu")]
        public string MessageContent { get; set; }
        public string User_Id { get; set; }
        //cheie externa
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
    
}