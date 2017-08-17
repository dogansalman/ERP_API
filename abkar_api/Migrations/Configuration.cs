namespace abkar_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using abkar_api.Models;
    using System.Data.SqlTypes;

    internal sealed class Configuration : DbMigrationsConfiguration<abkar_api.Models.MigrationContexts>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(abkar_api.Models.MigrationContexts context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Deparments.AddOrUpdate(d => d.name,
                new Departments { name = "Planlama", created_date = DateTime.Now, role = "planning" },
                new Departments { name = "Operasyon", created_date = DateTime.Now, role = "operation" },
                new Departments { name = "Kalite Kontrol", created_date = DateTime.Now, role = "quality" },
                new Departments { name = "Yönetim", created_date = DateTime.Now, role = "admin" }
            );
        }
    }
}
