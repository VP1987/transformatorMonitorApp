using Microsoft.EntityFrameworkCore;
using TransformerMonitor.Domain.Entities;

namespace TransformerMonitor.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Transformer> Transformers => Set<Transformer>();
    public DbSet<VoltageReading> VoltageReadings => Set<VoltageReading>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Technician> Technicians => Set<Technician>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transformer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasMany(e => e.VoltageReadings).WithOne(e => e.Transformer).HasForeignKey(e => e.TransformerId);
            entity.HasMany(e => e.Tickets).WithOne(e => e.Transformer).HasForeignKey(e => e.TransformerId);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasMany(e => e.Technicians).WithOne(e => e.Team).HasForeignKey(e => e.TeamId);
            entity.HasMany(e => e.AssignedTickets).WithOne(e => e.AssignedTeam).HasForeignKey(e => e.AssignedTeamId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
