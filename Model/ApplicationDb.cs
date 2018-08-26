using Microsoft.EntityFrameworkCore;
using System;

namespace WebApplication3.Framework.Models
{
    public class ApplicationDb:DbContext
    {
        public ApplicationDb(DbContextOptions<ApplicationDb> options) : base(options)
        { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer("Server=192.168.2.252;Database=test2;User ID =sa;Password=yinbenshu-1", b => b.UseRowNumberForPaging());
        //}
       
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
       
            modelBuilder.Entity<Role>().HasKey(r => r.RoleId);
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>().HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>().HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);

        }



    }
}
