using MyBlogApp.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogApp.DAL.Entity;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;

namespace MyBlogApp.DAL
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Company> Companys { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
           
            Database.EnsureCreated();
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompanyTag>()
                .HasKey(t => new { t.CompanyId, t.TagId });

            modelBuilder.Entity<CompanyTag>()
                .HasOne(sc => sc.Company)
                .WithMany(s => s.CompanyTags)
                .HasForeignKey(sc => sc.CompanyId);

            modelBuilder.Entity<CompanyTag>()
                .HasOne(sc => sc.Tag)
                .WithMany(c => c.TagCompanys)
                .HasForeignKey(sc => sc.TagId);

        }
    }
    
}
