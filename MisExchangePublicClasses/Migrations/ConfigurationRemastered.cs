using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace ru.novolabs.Migrations
{
    internal sealed class ConfigurationRemastered : DbMigrationsConfiguration<ru.novolabs.Common.CacheRemastered.CacheContextRemastered>
    {
        public ConfigurationRemastered()
        {
            AutomaticMigrationsEnabled = ConfigurationManager.AppSettings["enableMigrations"] != null ? ConfigurationManager.AppSettings["enableMigrations"] == "true" : true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ru.novolabs.Common.CacheRemastered.CacheContextRemastered context)
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
        }
    }
}
