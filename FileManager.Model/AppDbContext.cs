using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FileManager.Model
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {

          public AppDbContext()
          {

          }

          public AppDbContext(DbContextOptions options) : base(options)
          {

          }

          public static AppDbContext Create()
          {
               return new AppDbContext();
          }
          public virtual DbSet<AppUser> AppUsers { get; set; }
          public virtual DbSet<Download> Downloads { get; set; }
          public virtual DbSet<File> Files { get; set; }
          public virtual DbSet<History> Histories { get; set; }
          public virtual DbSet<Notification> Notifications { get; set; }

          protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          {
               optionsBuilder.UseSqlServer("Server=.;Database=FileManager;Trusted_Connection=True;MultipleActiveResultSets=true");
          }

          protected override void OnModelCreating(ModelBuilder builder)
          {
               // Single Property Configuration
               builder.Entity<AppUser>().HasKey(e => e.Id);
               builder.Entity<IdentityUserLogin<string>>().HasKey(e => e.UserId);
               builder.Entity<IdentityUserRole<string>>().HasKey(e => new { e.RoleId, e.UserId });
               builder.Entity<IdentityRole<string>>().HasKey(e => e.Id);
               builder.Entity<IdentityRoleClaim<string>>().HasKey(e => e.Id);
               builder.Entity<IdentityUserClaim<string>>().HasKey(e => e.Id);
               builder.Entity<IdentityUserToken<string>>().HasKey(e => e.UserId);

               // ToTable Property Configuration
               builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogin");
               builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRole");
               builder.Entity<IdentityRole<string>>().ToTable("AspNetRole");
               builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaim");
               builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaim");
               builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserToken");

               // One to One Relationships Configurations
               
               // One to Many Relationships Configurations
               builder.Entity<AppUser>()
                   .HasMany<Download>(a => a.Downloads)
                   .WithOne(d => d.AppUser)
                   .HasForeignKey(d => d.AppUserId);

               builder.Entity<AppUser>()
                   .HasMany<File>(a => a.Files)
                   .WithOne(f => f.AppUser)
                   .HasForeignKey(f => f.AppUserId);

               builder.Entity<AppUser>()
                   .HasMany<History>(a => a.Histories)
                   .WithOne(h => h.AppUser)
                   .HasForeignKey(h => h.AppUserId);

               builder.Entity<AppUser>()
                   .HasMany<Notification>(a => a.Notifications)
                   .WithOne(n => n.AppUser)
                   .HasForeignKey(n => n.AppUserId);

               builder.Entity<File>()
                   .HasMany<Download>(f => f.Downloads)
                   .WithOne(d => d.File)
                   .HasForeignKey(d => d.FileId);

               builder.Entity<File>()
                   .HasMany<History>(f => f.Histories)
                   .WithOne(h => h.File)
                   .HasForeignKey(h => h.FileId);
          }
     }
}
