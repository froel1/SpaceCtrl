using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<Object> Object { get; set; }
        public virtual DbSet<ObjectToClient> ObjectToClient { get; set; }
        public virtual DbSet<TargetGroup> TargetGroup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
                entity.HasKey(e => new { e.Id, e.Key });

                entity.ToTable("Device", "device");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Key)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.Device)
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Device_TargetGroup");
            });

            modelBuilder.Entity<Object>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ObjectToClient>(entity =>
            {
                entity.HasKey(e => new { e.ObjectId, e.ClientId });

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
