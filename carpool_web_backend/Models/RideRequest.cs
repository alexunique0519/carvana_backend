namespace carpool_web_backend.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

   
    public partial class RideRequestModel
    {
       
        public int Id { get; set; }

        public DateTime DepartureStartTime { get; set; }

        public DateTime DepartureEndTime { get; set; }

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

        
        public int NumOfPersons { get; set; }

     
        [StringLength(128)]
        public string UserId { get; set; }


        public virtual ApplicationUser User { get; set; }

    }
}
