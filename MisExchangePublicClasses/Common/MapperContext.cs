using ru.novolabs.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ru.novolabs.Common
{
    public class MapperContext : DbContext
    {
        public MapperContext(string connectionStr)
            : base(connectionStr)
        {
            
        }

        public DbSet<TestMapperItem> TestMapping { get; set; }
        public DbSet<TargetMapperItem> TargetMapping { get; set; }
        public DbSet<BiomaterialMapperItem> BioMapping { get; set; }       
    }
}
