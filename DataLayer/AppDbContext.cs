using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext()
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderModel>()
        .HasKey(c => new { c.Oid, c.Username });
            base.OnModelCreating(builder);
        }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Login> LoginModel { get; set; }
        public DbSet<Register> RegisterModel { get; set; }
        public DbSet<MedicineCategory> MedicineCategorie { get; set; }
        public DbSet<MedicineModel> MedicineModel { get; set; }
        public DbSet<OrderModel> OrderModel { get; set; }

    }
}
