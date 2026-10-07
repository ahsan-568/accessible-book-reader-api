using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace chatApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        //[Key]
        //public int Id { get; set; }
        public string FirstName { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Messages> SentMessages { get; set; } // passing a refernce through where we can access forein key from it
        public ICollection<Messages> RecievedMessages { get; set; }
    }
}
