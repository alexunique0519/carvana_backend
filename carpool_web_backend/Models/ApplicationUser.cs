using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace carpool_web_backend.Models
{

    public enum UserType
    {
        enuPassenger = 0,
        enuDriver
    }

    public class ApplicationUser : IdentityUser
    { 
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        //to differenciate passenger and driver, currently define
        [Required]
        public int UserType { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        //public virtual ICollection<CarModel> Cars { get; set; }
        //public virtual ICollection<RideShareModel> RideShares { get; set; }
        //public virtual ICollection<RideRequestModel> RideRequests { get; set; }
    }
}