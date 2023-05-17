using ru.novolabs.ExchangeDTOs;
using ru.novolabs.Migrations;
using ru.novolabs.SuperCore;
using System.Data.Entity;

namespace ru.novolabs.Common.CacheRemastered
{
    public class CacheContextRemastered : DbContext
    {
        public CacheContextRemastered()
            : base(ConnectionStringHelper.GetConnStr())
        {
            Initialization();
        }
        public CacheContextRemastered(string connStr)
            : base(connStr)
        {
            Initialization();
        }
        private void Initialization()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CacheContextRemastered, ConfigurationRemastered>(true));
        }

        public DbSet<RequestObjectStatus> RequestObjectStatusSet { get; set; }
        public DbSet<ResultObjectStatus> ResultObjectStatusSet { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Result> Results { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Sample> Samples { get; set; }

        public DbSet<Target> Targets { get; set; }

        public DbSet<SampleResult> SampleResults { get; set; }
        public DbSet<TargetResult> TargetResults { get; set; }
        public DbSet<MicroResult> MicroResults { get; set; }

        public DbSet<Work> Works { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestObjectStatus>().HasRequired(r => r.Request).WithRequiredDependent();
            modelBuilder.Entity<Request>().HasRequired(r => r.Patient).WithRequiredDependent().WillCascadeOnDelete(false);//.WithRequiredPrincipal();
            modelBuilder.Entity<Request>().HasMany(r => r.Samples).WithRequired().WillCascadeOnDelete(true);
            modelBuilder.Entity<Sample>().HasMany(s => s.Targets).WithRequired().WillCascadeOnDelete(true);
            modelBuilder.Entity<Target>().HasMany(t => t.Tests).WithRequired().WillCascadeOnDelete(true);
            modelBuilder.Entity<Request>().HasMany(r => r.UserFields).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Patient>().HasRequired(p => p.PatientCard).WithRequiredPrincipal();
            modelBuilder.Entity<Patient>().HasMany(p => p.UserFields).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<PatientCard>().HasMany(p => p.UserFields).WithOptional().WillCascadeOnDelete(true);

            modelBuilder.Entity<ResultObjectStatus>().HasRequired(r => r.Result).WithRequiredDependent().WillCascadeOnDelete();
            modelBuilder.Entity<Result>().HasRequired(r => r.Patient).WithRequiredDependent().WillCascadeOnDelete(false);//.WithRequiredPrincipal();
            modelBuilder.Entity<Result>().HasMany(r => r.UserFields).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Result>().HasMany(r => r.SampleResults).WithRequired().WillCascadeOnDelete();
            modelBuilder.Entity<SampleResult>().HasMany(s => s.TargetResults).WithRequired().WillCascadeOnDelete();
            modelBuilder.Entity<SampleResult>().HasMany(s => s.MicroResults).WithRequired().WillCascadeOnDelete();
            modelBuilder.Entity<SampleResult>().HasMany(s => s.Defects).WithOptional().WillCascadeOnDelete(false);
            modelBuilder.Entity<TargetResult>().HasMany(t => t.Works).WithOptional();//.WillCascadeOnDelete(true);
            modelBuilder.Entity<MicroResult>().HasMany(m => m.Antibiotics).WithOptional();//.WillCascadeOnDelete(true);
            modelBuilder.Entity<Work>().HasRequired(w => w.Norm).WithRequiredPrincipal();
            modelBuilder.Entity<Work>().HasMany(w => w.Defects).WithOptional().WillCascadeOnDelete(false);
            //  modelBuilder.Entity<Work>().HasMany(w => w.Images).WithRequired().WillCascadeOnDelete();
            modelBuilder.Entity<MicroResult>().HasOptional(m => m.ParentWork).WithOptionalPrincipal();//.WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
    }
}
