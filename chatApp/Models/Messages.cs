using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;

namespace chatApp.Models
{
    [Table("UserMessages")]
    public class Messages
    {
        public Messages()
        {
           // Senders = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        
        public string Message { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }

        public ApplicationUser MessageSender { get; set; }
        public ApplicationUser MessageReciever { get; set; }

        public string FileLink { get; set; }


        //public string SenderId { get; set; }

        //[ForeignKey("Reciever"), Column(Order = 0)]
        //public int RecieverId { get; set; } 
    }
}
