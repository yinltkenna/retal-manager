using Microsoft.EntityFrameworkCore;
using PropertyService.src.Domain.Entities;
using PropertyService.src.Domain.Enums;

namespace PropertyService.src.Infrastructure.Data
{
    public class PropertyDbContext : DbContext
    {
        public PropertyDbContext(DbContextOptions<PropertyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Branch> Branches { get; set; } = null!;
        public DbSet<RoomType> RoomTypes { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<RoomPrice> RoomPrices { get; set; } = null!;
        public DbSet<RoomImage> RoomImages { get; set; } = null!;
        public DbSet<Amenity> Amenities { get; set; } = null!;
        public DbSet<RoomTypeAmenity> RoomTypeAmenities { get; set; } = null!;
        public DbSet<RoomFacility> RoomFacilities { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<RoomHistory> RoomHistories { get; set; } = null!;
        public DbSet<RoomMaintenance> RoomMaintenances { get; set; } = null!;
        public DbSet<RoomStatusLog> RoomStatusLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Primary keys
            modelBuilder.Entity<Branch>().HasKey(b => b.Id);
            modelBuilder.Entity<RoomType>().HasKey(rt => rt.Id);
            modelBuilder.Entity<Room>().HasKey(r => r.Id);
            modelBuilder.Entity<RoomPrice>().HasKey(rp => rp.Id);
            modelBuilder.Entity<RoomImage>().HasKey(ri => ri.Id);
            modelBuilder.Entity<Amenity>().HasKey(a => a.Id);
            modelBuilder.Entity<RoomTypeAmenity>().HasKey(rta => rta.Id);
            modelBuilder.Entity<RoomFacility>().HasKey(rf => rf.Id);
            modelBuilder.Entity<Reservation>().HasKey(r => r.Id);
            modelBuilder.Entity<RoomHistory>().HasKey(rh => rh.Id);
            modelBuilder.Entity<RoomMaintenance>().HasKey(rm => rm.Id);
            modelBuilder.Entity<RoomStatusLog>().HasKey(rsl => rsl.Id);

            // Soft delete filters
            modelBuilder.Entity<Branch>().HasQueryFilter(b => !b.IsDeleted);
            modelBuilder.Entity<RoomType>().HasQueryFilter(rt => !rt.IsDeleted);
            modelBuilder.Entity<Room>().HasQueryFilter(r => !r.IsDeleted);
            modelBuilder.Entity<RoomPrice>().HasQueryFilter(rp => !rp.IsDeleted);
            modelBuilder.Entity<RoomImage>().HasQueryFilter(ri => !ri.IsDeleted);
            modelBuilder.Entity<Amenity>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<RoomTypeAmenity>().HasQueryFilter(rta => !rta.IsDeleted);
            modelBuilder.Entity<RoomFacility>().HasQueryFilter(rf => !rf.IsDeleted);
            modelBuilder.Entity<Reservation>().HasQueryFilter(r => !r.IsDeleted);
            modelBuilder.Entity<RoomHistory>().HasQueryFilter(rh => !rh.IsDeleted);
            modelBuilder.Entity<RoomMaintenance>().HasQueryFilter(rm => !rm.IsDeleted);
            modelBuilder.Entity<RoomStatusLog>().HasQueryFilter(rsl => !rsl.IsDeleted);

            // Indexes
            modelBuilder.Entity<Branch>().HasIndex(b => b.Name).IsUnique();
            modelBuilder.Entity<RoomType>().HasIndex(rt => new { rt.BranchId, rt.TypeName }).IsUnique();
            modelBuilder.Entity<Room>().HasIndex(r => new { r.BranchId, r.RoomNumber }).IsUnique();
            modelBuilder.Entity<Amenity>().HasIndex(a => a.Name).IsUnique();
            modelBuilder.Entity<RoomTypeAmenity>().HasIndex(rta => new { rta.RoomTypeId, rta.AmenityId }).IsUnique();
            modelBuilder.Entity<RoomFacility>().HasIndex(rf => new { rf.RoomId, rf.Name }).IsUnique();

            // Relationships
            modelBuilder.Entity<RoomType>()
                .HasOne<Branch>()
                .WithMany()
                .HasForeignKey(rt => rt.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>()
                .HasOne<RoomType>()
                .WithMany()
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                .HasOne<Branch>()
                .WithMany()
                .HasForeignKey(r => r.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomPrice>()
                .HasOne<RoomType>()
                .WithMany()
                .HasForeignKey(rp => rp.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomImage>()
                .HasOne<Room>()
                .WithMany()
                .HasForeignKey(ri => ri.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomTypeAmenity>()
                .HasOne<RoomType>()
                .WithMany()
                .HasForeignKey(rta => rta.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomTypeAmenity>()
                .HasOne<Amenity>()
                .WithMany()
                .HasForeignKey(rta => rta.AmenityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomFacility>()
                .HasOne<Room>()
                .WithMany()
                .HasForeignKey(rf => rf.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne<Room>()
                .WithMany()
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomHistory>()
                .HasOne<Room>()
                .WithMany()
                .HasForeignKey(rh => rh.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomMaintenance>()
                .HasOne<Room>()
                .WithMany()
                .HasForeignKey(rm => rm.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomStatusLog>()
                .HasOne<Room>()
                .WithMany()
                .HasForeignKey(rsl => rsl.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Property constraints
            modelBuilder.Entity<Branch>().Property(b => b.Name).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Branch>().Property(b => b.Address).IsRequired().HasMaxLength(400);
            modelBuilder.Entity<Branch>().Property(b => b.Hotline).HasMaxLength(50);

            modelBuilder.Entity<RoomPrice>().Property(rp => rp.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.Property(rt => rt.TypeName).IsRequired().HasMaxLength(200);
                entity.Property(rt => rt.BasePrice).HasColumnType("decimal(18,2)");
            });
            modelBuilder.Entity<Room>().Property(r => r.RoomNumber).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Amenity>().Property(a => a.Name).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<RoomFacility>().Property(rf => rf.Name).IsRequired().HasMaxLength(200);

            // Default values
            modelBuilder.Entity<Branch>().Property(b => b.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<Reservation>().Property(r => r.Status).HasDefaultValue(ReservationStatus.Pending);
            modelBuilder.Entity<RoomMaintenance>().Property(rm => rm.Status).HasDefaultValue(MaintenanceStatus.Pending);

            // Secondary indices
            modelBuilder.Entity<Reservation>().HasIndex(r => new { r.RoomId, r.StartDate, r.EndDate });
            modelBuilder.Entity<RoomHistory>().HasIndex(rh => new { rh.RoomId, rh.CheckInAt });
            modelBuilder.Entity<RoomMaintenance>().HasIndex(rm => new { rm.RoomId, rm.Status });
            modelBuilder.Entity<RoomStatusLog>().HasIndex(rsl => new { rsl.RoomId, rsl.ChangedAt });
        }
    }
}
