using System;
using System.Data.Entity.Migrations;
using abkar_api.Models;

namespace abkar_api.Migrations
{
  internal sealed class TestConfiguration : DbMigrationsConfiguration<abkar_api.Models.MigrationContexts>
    {
        public TestConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(abkar_api.Models.MigrationContexts context)
        {
            context.StockTypes.AddOrUpdate(st => st.name,
                new StockTypes {
                    name = "Demir"
                }
                );
            context.SaveChanges();
        }
    }
}