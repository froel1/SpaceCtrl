using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SpaceCtrl.Data.Models.Database
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

        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Object> Object { get; set; }
        public virtual DbSet<Person> Person { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=167.172.108.255;Database=SpaceCtrl;User Id=sa;Password=kEEp4izontal;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Object>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ImageDate).HasColumnType("datetime");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Object)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Object_Device");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Object)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Object_Image");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SyncDetails).IsRequired();

                entity.Property(e => e.SyncRequestedAt).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
