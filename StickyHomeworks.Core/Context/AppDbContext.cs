using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StickyHomeworks.Core.Entities;

namespace StickyHomeworks.Core.Context;

public partial class DbContext : DbContext
{
    public DbContext()
    {
    }

    public DbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Emotion> Emotions { get; set; }

    public virtual DbSet<EmotionsGroup> EmotionsGroups { get; set; }

    public virtual DbSet<Homework> Homeworks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=db/app.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Emotion>(entity =>
        {
            entity.ToTable("emotions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.Path).HasColumnName("path");
            entity.Property(e => e.Size).HasColumnName("size");
        });

        modelBuilder.Entity<EmotionsGroup>(entity =>
        {
            entity.ToTable("emotions_groups");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Size)
                .HasColumnType("NUMBER")
                .HasColumnName("size");
        });

        modelBuilder.Entity<Homework>(entity =>
        {
            entity.ToTable("homework");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.EndTime)
                .HasColumnType("DATETIME")
                .HasColumnName("end_time");
            entity.Property(e => e.Subject).HasColumnName("subject");
            entity.Property(e => e.Tags).HasColumnName("tags");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
