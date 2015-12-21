using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace carpool_web_backend.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("carpool_db", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<RideRequestModel> RideRequests { get; set; }
        public DbSet<RideShareModel> RideShares { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<RideShareModel>()
                .HasRequired<ApplicationUser>(u => u.Driver)
                .WithMany(u => u.RideShares)
                .HasForeignKey(u => u.UserId);
           


            modelBuilder.Entity<ApplicationUser>()
                .HasMany<RideShareModel>(r => r.RideShares)
                .WithRequired(r => r.User)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<ApplicationUser>()
              .HasMany<RideRequestModel>(r => r.RideRequests)
              .WithRequired(r => r.User)
              .HasForeignKey(r => r.UserId);

     */
            base.OnModelCreating(modelBuilder);
        }    

    }
}