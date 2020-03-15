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

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Object> Object { get; set; }
        public virtual DbSet<ObjectToClient> ObjectToClient { get; set; }
        public virtual DbSet<TargetGroup> TargetGroup { get; set; }

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
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client", "client");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Client_TargetGroup");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("Device", "device");

                entity.HasIndex(e => e.Key)
                    .HasName("IX_Device")
                    .IsUnique();

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.Device)
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Device_TargetGroup");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Object>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.DeviceKeyNavigation)
                    .WithMany(p => p.Object)
                    .HasPrincipalKey(p => p.Key)
                    .HasForeignKey(d => d.DeviceKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Object_Device");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Object)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Object_Image");
            });

            modelBuilder.Entity<ObjectToClient>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.ObjectGuid });

                entity.ToTable("ObjectToClient", "client");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.ObjectToClient)
                    .HasForeignKey(d => d.ObjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ObjectToClient_Object");
            });

            modelBuilder.Entity<TargetGroup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EncodingsPath).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
