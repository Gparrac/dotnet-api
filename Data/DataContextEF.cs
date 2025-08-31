using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data;


public class DataContextEF(IConfiguration config) : DbContext
{
    private readonly IConfiguration _config = config;

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserSalary> UserSalary { get; set; }
    public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        /* Define the connection when it is not configured ℹ️
           - We need to add dependency, following our kind of db <EntityFrameworkCore.SqlServer
        */
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(_config.GetConnectionString("connection"),
                   optionsBuilder => optionsBuilder.EnableRetryOnFailure());
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("TutorialAppSchema");
        modelBuilder.Entity<User>()
            .ToTable("Users", "TutorialAppSchema")
            .HasKey(u => u.UserId);
        modelBuilder.Entity<UserSalary>()
            .ToTable("UserSalary", "TutorialAppSchema")
            .HasKey(u => u.UserId);
        modelBuilder.Entity<UserJobInfo>()
            .ToTable("UserJobInfo", "TutorialAppSchema")
            .HasKey(u => u.UserId);
    }
} 