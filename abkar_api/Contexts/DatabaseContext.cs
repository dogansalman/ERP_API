using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using abkar_api.Models;

namespace abkar_api.Contexts
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext() : base("Name=ConnectionStr")
        {
            Database.SetInitializer<DatabaseContext>(null);

        }
        public DbSet<Customers> customers { get; set; }
        public DbSet<Personnel> personnels { get; set; }
        public DbSet<Departments> departments { get; set; }
        public DbSet<Suppliers> suppliers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customers>().ToTable("customers");
            modelBuilder.Entity<Personnel>().ToTable("personnel");
            modelBuilder.Entity<Departments>().ToTable("departments");
            modelBuilder.Entity<Suppliers>().ToTable("suppliers");
        }
    }
}