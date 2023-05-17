using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Xml;
using ru.novolabs.ExchangeDTOs;
using ru.novolabs.Migrations;
using ru.novolabs.SuperCore;

namespace ru.novolabs.Common
{
    [Obsolete("Please, use CacheContextRemastered for new Helpers. This CacheContext is only for backward compability")]
    public class CacheContext : DbContext
    {
        public CacheContext()
            : base(ConnectionStringHelper.GetConnStr())
        {
            Initialization();
        }
        public CacheContext(string connStr)
            : base(connStr)
        {
            Initialization();
        }
        private void Initialization()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CacheContext, Configuration>(true));
        }
        private void NullInitialization()
        {
            Database.SetInitializer<CacheContext>(null);     
        }
        public CacheContext(bool isHierarchy)
            : base(ConnectionStringHelper.GetConnStr())
        {
            if (isHierarchy)
                NullInitialization();
            else
                Initialization();         
        }
        public CacheContext(string connStr, bool isHierarchy)
            : base(connStr)
        {
            if (isHierarchy)
                NullInitialization();
            else
                Initialization();
        }

        public DbSet<RequestObjectStatus> RequestObjectStatusSet { get; set; }
        public DbSet<ResultObjectStatus> ResultObjectStatusSet { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Result> Results { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<UserField> UserFields { get; set; }

        public DbSet<PatientCard> PatientCards { get; set; }
        public DbSet<Target> Targets { get; set; }

        public DbSet<SampleResult> SampleResults { get; set; }
        public DbSet<TargetResult> TargetResults { get; set; }
        public DbSet<MicroResult> MicroResults { get; set; }

        public DbSet<Work> Works { get; set; }

        public DbSet<Norm> Norms { get; set; }

        public DbSet<Defect> Defects { get; set; }
    }
}
