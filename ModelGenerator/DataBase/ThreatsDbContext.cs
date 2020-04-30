
using Microsoft.EntityFrameworkCore;
using ModelGenerator.DataBase.Models;

namespace ModelGenerator.DataBase
{
    public class ThreatsDbContext : DbContext
    {
        public ThreatsDbContext(DbContextOptions options) : base(options)
        {

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