using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SpaceCtrl.Data.Database.DbObjects
{
    public partial class SpaceCtrlContext : DbContext
    {
        public SpaceCtrlContext()
        {
        }

        public SpaceCtrlContext(DbContextOptions<SpaceCtrlContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Channel> Channel { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<Frame> Frame { get; set; }
        public virtual DbSet<GroupEntry> GroupEntry { get; set; }
        public virtual DbSet<GroupShift> GroupShift { get; set; }
        public virtual DbSet<Object> Object { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonGroup> PersonGroup { get; set; }
        public virtual DbSet<PersonImages> PersonImages { get; set; }
        public virtual DbSet<ShiftType> ShiftType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Channel)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Channel_Device");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Frame>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GroupEntry>(entity =>
            {
                entity.ToTable("GroupEntry", "grp");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Frame)
                    .WithMany(p => p.GroupEntry)
                    .HasForeignKey(d => d.FrameId)
                    .HasConstraintName("FK_GroupEntry_Frame");

                entity.HasOne(d => d.GroupShift)
                    .WithMany(p => p.GroupEntry)
                    .HasForeignKey(d => d.GroupShiftId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupEntry_GroupShift");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.GroupEntry)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_GroupEntry_Person1");
            });

            modelBuilder.Entity<GroupShift>(entity =>
            {
                entity.ToTable("GroupShift", "grp");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupShift)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupShift_PersonGroup");

                entity.HasOne(d => d.ShiftTypeNavigation)
                    .WithMany(p => p.GroupShift)
                    .HasForeignKey(d => d.ShiftType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupShift_ShiftType");
            });

            modelBuilder.Entity<Object>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FrameDate).HasColumnType("datetime");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Object)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Object_Device");

                entity.HasOne(d => d.Frame)
                    .WithMany(p => p.Object)
                    .HasForeignKey(d => d.FrameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Object_Frame");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SyncDetails).IsRequired();

                entity.Property(e => e.SyncRequestedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_PersonGroup");
            });

            modelBuilder.Entity<PersonGroup>(entity =>
            {
                entity.ToTable("PersonGroup", "grp");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<PersonImages>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.FrameId });

                entity.HasOne(d => d.Frame)
                    .WithMany(p => p.PersonImages)
                    .HasForeignKey(d => d.FrameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PersonImages_Frame");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PersonImages)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PersonImages_Person");
            });

            modelBuilder.Entity<ShiftType>(entity =>
            {
                entity.ToTable("ShiftType", "grp");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}