using Book.Data.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.Context
{
    public class MainDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public MainDBContext(DbContextOptions<MainDBContext> options) : base(options)
        {
            
        }

        public DbSet<Author> Author { get; set; }
        public DbSet<Books> books { get; set; }
        public DbSet<ApplicationRole> Role { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

            builder.Entity<Books>()
                .HasOne(a => a.Author)
                .WithMany(d => d.books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser() { Id = "1", Name = "AdminFirstName",   Role = "Admin", ConcurrencyStamp = "1", NormalizedUserName = "Admin", UserName = "admin@example.com", Email = "admin@example.com" },
                new ApplicationUser() { Id = "2", Name = "AuthorFirstName",  Role = "Author", ConcurrencyStamp = "2", NormalizedUserName = "Author", UserName = "author@example.com", Email = "author@example.com" }
            );
        }
       

    }
}
