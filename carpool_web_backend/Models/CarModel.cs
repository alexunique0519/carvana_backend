using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace carpool_web_backend.Models
{
    public class CarModel
    {
        public int Id { get; set; }

        [Required]
        public int CarType { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public int Seats { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}