using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using chatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace chatApp.Data
{
    public class chatAppContext : IdentityDbContext<ApplicationUser>
    {

        //public chatAppContext()
        //{
        //}

        public chatAppContext(DbContextOptions<chatAppContext> options)
            : base(options)
        {
        }

        public chatAppContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Identity");
            builder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            // Add the shadow property to the model
            //builder.Entity<ApplicationUser>()
            //    .Property<int>("MessageForeignId");

            //builder.Entity<ApplicationUser>()
            // .HasOne(s=>s.Sender).WithMany(ad => ad.Senders)
            // .IsRequired();

            //builder.Entity<Messages>()
            // .HasMany(s => s.Senders).WithRequired(ad => ad.Messages)

            //.HasOne(m => m.quantity).IsRequired()

            builder.Entity<Messages>()
                .HasOne(p => p.MessageSender)
                .WithMany(p => p.SentMessages)
                   .HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Messages>()
                   .HasOne(m => m.MessageReciever)
                   .WithMany(t => t.RecievedMessages).IsRequired(false).OnDelete(DeleteBehavior.Restrict)
                   .HasForeignKey(m => m.RecieverId); 


         




            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-RFLSDPH\SQLEXPRESS;Database=chatApp_dev;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=true");
            
        }
        public DbSet<Messages> UserMessages { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
