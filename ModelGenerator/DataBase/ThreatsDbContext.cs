
using Microsoft.EntityFrameworkCore;
using ModelGenerator.DataBase.Models;

namespace ModelGenerator.DataBase
{
    public class ThreatsDbContext : DbContext
    {
        public ThreatsDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = "Server=.\\SQLEXPRESS;Database=ThreatDb;Trusted_Connection=True;";
            optionsBuilder.UseSqlServer(conn);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ThreatSource>()
                .HasKey(t => new { t.ThreatId, t.SourceId });

            modelBuilder.Entity<ThreatTarget>()
                .HasKey(t => new { t.ThreatId, t.TargetId });

            modelBuilder.Entity<ThreatSource>()
                .HasOne(sc => sc.Threat)
                .WithMany(s => s.ThreatSource)
                .HasForeignKey(sc => sc.SourceId);

            modelBuilder.Entity<ThreatSource>()
                .HasOne(sc => sc.Source)
                .WithMany(c => c.ThreatSource)
                .HasForeignKey(sc => sc.ThreatId);

            modelBuilder.Entity<ThreatTarget>()
                .HasOne(sc => sc.Threat)
                .WithMany(s => s.ThreatTarget)
                .HasForeignKey(sc => sc.TargetId);

            modelBuilder.Entity<ThreatTarget>()
                .HasOne(sc => sc.Target)
                .WithMany(c => c.ThreatTarget)
                .HasForeignKey(sc => sc.ThreatId);



            modelBuilder.Entity<ModelPreferencesSource>()
                .HasKey(t => new { t.SourceId, t.ModelPreferencesId });

            modelBuilder.Entity<ModelPreferencesTarget>()
                .HasKey(t => new { t.TargetId, t.ModelPreferencesId });

            modelBuilder.Entity<ModelPreferencesSource>()
                .HasOne(sc => sc.ModelPreferences)
                .WithMany(s => s.ModelPreferencesSource)
                .HasForeignKey(sc => sc.SourceId);

            modelBuilder.Entity<ModelPreferencesSource>()
                .HasOne(sc => sc.Source)
                .WithMany(c => c.ModelPreferencesSource)
                .HasForeignKey(sc => sc.ModelPreferencesId);

            modelBuilder.Entity<ModelPreferencesTarget>()
                .HasOne(sc => sc.ModelPreferences)
                .WithMany(s => s.ModelPreferencesTarget)
                .HasForeignKey(sc => sc.TargetId);

            modelBuilder.Entity<ModelPreferencesTarget>()
                .HasOne(sc => sc.Target)
                .WithMany(c => c.ModelPreferencesTarget)
                .HasForeignKey(sc => sc.ModelPreferencesId);
        }

        public DbSet<Model> Model { get; set; }

        public DbSet<ModelLine> ModelLine { get; set; }

        public DbSet<ModelPreferences> ModelPreferences { get; set; }

        public DbSet<Source> Source { get; set; }

        public DbSet<Target> Target { get; set; }

        public DbSet<Threat> Threat { get; set; }

        public DbSet<ThreatDanger> ThreatDanger { get; set; }

        public DbSet<ThreatPossibility> ThreatPossibility { get; set; }

        public DbSet<User> User { get; set; }

    }
}