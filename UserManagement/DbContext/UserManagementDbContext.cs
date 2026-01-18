using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UserManagement.DbContext.Models;

namespace UserManagement.DbContext;

public partial class UserManagementDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public UserManagementDbContext()
    {
    }

    public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag");

            entity.HasIndex(e => e.TagName, "UQ_Tag_TagName").IsUnique();

            entity.Property(e => e.CreatedTimeStamp).HasColumnType("datetime");
            entity.Property(e => e.TagDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TagName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedTimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ_User_Username").IsUnique();

            entity.Property(e => e.CreatedTimeStamp).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedTimeStamp).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("UserProfile");

            entity.Property(e => e.ContactNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedTimeStamp).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedTimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserProfile_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
