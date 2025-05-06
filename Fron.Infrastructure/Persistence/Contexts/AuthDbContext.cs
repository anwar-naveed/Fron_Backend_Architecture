using System;
using System.Collections.Generic;
using Fron.Domain.AuthEntities;
using Microsoft.EntityFrameworkCore;

namespace Fron.Infrastructure.Persistence.Contexts;
public partial class AuthDbContext : DbContext
{
    public AuthDbContext()
    {
    }

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<Role> Role { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<LogEntryErrors> LogEntryErrors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-HIAGS2V;Initial Catalog=AdventureWorksAuth;User ID=sa;Password=123;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
        });

        modelBuilder.Entity<UserRole>()
            .HasOne(e => e.User)
            .WithMany(e => e.UserRoles)
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(navigationExpression: e => e.Role)
            .WithMany(e => e.UserRoles)
            .HasForeignKey(e => e.RoleId);

        modelBuilder.Entity<UserRole>().ToTable("UserRoles");

        modelBuilder.Entity<LogEntryErrors>(entity =>
        {
            entity.ToTable("LogEntryErrors");

            entity.Property(e => e.Exception).IsUnicode(false);
            entity.Property(e => e.Message)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RequestHeaders).IsUnicode(false);
            entity.Property(e => e.RequestMethod)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RequestPath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Source)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StackTrace).IsUnicode(false);
            entity.Property(e => e.UserDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
        });
    }
}
