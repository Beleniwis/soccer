using Microsoft.EntityFrameworkCore;
using Soccer.Models;

namespace Soccer.Data;

public class SoccerDbContext : DbContext
{
    public SoccerDbContext(DbContextOptions<SoccerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }

    public DbSet<Team> Teams { get; set; }

    public DbSet<Player> Players { get; set; }

    public DbSet<League> Leagues { get; set; }

    public DbSet<TeamLeague> TeamLeagues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TeamLeague>()
            .HasKey(x => new { x.TeamId, x.LeagueId });

        modelBuilder.Entity<Team>()
            .HasOne(x => x.Country)
            .WithMany(x => x.Teams)
            .HasForeignKey(x => x.CountryId);

        modelBuilder.Entity<League>()
            .HasOne(x => x.Country)
            .WithMany(x => x.Leagues)
            .HasForeignKey(x => x.CountryId);

        modelBuilder.Entity<Player>()
            .HasOne(x => x.Team)
            .WithMany(x => x.Players)
            .HasForeignKey(x => x.TeamId);

        modelBuilder.Entity<TeamLeague>()
            .HasOne(x => x.Team)
            .WithMany(x => x.TeamLeagues)
            .HasForeignKey(x => x.TeamId);

        modelBuilder.Entity<TeamLeague>()
            .HasOne(x => x.League)
            .WithMany(x => x.TeamLeagues)
            .HasForeignKey(x => x.LeagueId);
    }
}