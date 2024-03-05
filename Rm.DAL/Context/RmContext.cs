
using Microsoft.EntityFrameworkCore;
using Rm.Model.Models;
using Rm.Models;

namespace Rm.DAL.Context;

public partial class RmContext : DbContext
{
    public RmContext()
    {
    }

    public RmContext(DbContextOptions<RmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Number)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Role1)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.Property(e => e.Token1).HasColumnName("Token");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreationDate)
                .HasPrecision(0)
                .HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.ImagePath)
               .HasMaxLength(200)
               .IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleId");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Position)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}