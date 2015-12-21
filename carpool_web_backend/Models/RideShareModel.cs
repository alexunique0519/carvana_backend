using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace carpool_web_backend.Models
{
    
    public partial class RideShareModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        [StringLength(50)]
        public string FromProvince { get; set; }

        [Required]
        [StringLength(50)]
        public string FromCity { get; set; }

        [Required]
        [StringLength(255)]
        public string FromLocation { get; set; }

        [Required]
        [StringLength(50)]
        public string ToProvince { get; set; }

        [Required]
        [StringLength(50)]
        public string ToCity { get; set; }

        [Required]
        [StringLength(255)]
        public string ToLocation { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        public int SeatsAvailable { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}