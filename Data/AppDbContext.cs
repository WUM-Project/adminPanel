// using Applicant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin_Panel.Models;
// using Admin_Panel.Models.Attribute;

namespace Admin_Panel.Data
{
    public class AppDbContext : DbContext
    {
      
         public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
          
            
            Database.EnsureCreated();
        }
        public DbSet<Models.Attribute> Attributes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductToMark> ProductToMarks { get; set; }
        public DbSet<ProductToCategory> ProductToCategories { get; set; }
        public DbSet<ProductToAttribute> ProductToAttributes { get; set; }
        // public DbSet<User> Users { get; set; }
        // public DbSet<UserExams> UserExams { get; set; }
        // public DbSet<AccessCode> AccessCodes { get; set; }
        // public DbSet<RefreshToken> RefreshTokens { get; set; }
     

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // builder.Entity<User>().HasMany(x => x.RefreshTokens).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            // builder.Entity<UserExams>().HasKey(x => new { x.ExamId, x.UserId });
            builder.Entity<ProductToMark>().HasKey(sc => new { sc.ProductId, sc.MarkId });
            builder.Entity<ProductToCategory>().HasKey(sc => new { sc.ProductId, sc.CategoryId });
            builder.Entity<ProductToAttribute>().HasKey(sc => new { sc.ProductId, sc.AttributeId });
            base.OnModelCreating(builder);
        }
    }
}
