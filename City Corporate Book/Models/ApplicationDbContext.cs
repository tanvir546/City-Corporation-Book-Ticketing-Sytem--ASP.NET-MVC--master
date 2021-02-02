using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace City_Corporate_Book.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base ("ApplicationDbContext")
        { }

        public DbSet<Login> Logins { get; set; }

        public DbSet<Registration> Registrations { get; set; }

        public DbSet<MemberUserLocationWiseComplain> MemUserLocationComplains { get; set; }

        public DbSet<MemberUserLocationWiseComplainCommants> MemUserCompCommants{ get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<CommentAgreeDisagreeStatus> CommentAgreeDisagreeStatuses { get; set; }
    }
}