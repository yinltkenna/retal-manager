using Microsoft.EntityFrameworkCore;
using TenancyService.src.Domain.Entities;

namespace TenancyService.src.Infrastructure.Data
{
    public class TenancyDbContext : DbContext
    {
        public TenancyDbContext(DbContextOptions<TenancyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; } = null!;
        public DbSet<TenantIdentityDocument> TenantIdentityDocuments { get; set; } = null!;
        public DbSet<Contract> Contracts { get; set; } = null!;
        public DbSet<ContractMember> ContractMembers { get; set; } = null!;
        public DbSet<ContractFile> ContractFiles { get; set; } = null!;
        public DbSet<ContractExtension> ContractExtensions { get; set; } = null!;
        public DbSet<ContractTermination> ContractTerminations { get; set; } = null!;
        public DbSet<ContractDepositTransaction> ContractDepositTransactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>().HasKey(t => t.Id);
            modelBuilder.Entity<TenantIdentityDocument>().HasKey(d => d.Id);
            modelBuilder.Entity<Contract>().HasKey(c => c.Id);
            modelBuilder.Entity<ContractMember>().HasKey(cm => cm.Id);
            modelBuilder.Entity<ContractFile>().HasKey(cf => cf.Id);
            modelBuilder.Entity<ContractExtension>().HasKey(ce => ce.Id);
            modelBuilder.Entity<ContractTermination>().HasKey(ct => ct.Id);
            modelBuilder.Entity<ContractDepositTransaction>().HasKey(dt => dt.Id);

            // Soft delete filters
            modelBuilder.Entity<Tenant>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<TenantIdentityDocument>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<Contract>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<ContractMember>().HasQueryFilter(cm => !cm.IsDeleted);
            modelBuilder.Entity<ContractFile>().HasQueryFilter(cf => !cf.IsDeleted);
            modelBuilder.Entity<ContractExtension>().HasQueryFilter(ce => !ce.IsDeleted);
            modelBuilder.Entity<ContractTermination>().HasQueryFilter(ct => !ct.IsDeleted);
            modelBuilder.Entity<ContractDepositTransaction>().HasQueryFilter(dt => !dt.IsDeleted);

            // Relationships
            modelBuilder.Entity<TenantIdentityDocument>()
                .HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContractMember>()
                .HasOne<Contract>()
                .WithMany()
                .HasForeignKey(cm => cm.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContractMember>()
                .HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(cm => cm.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContractFile>()
                .HasOne<Contract>()
                .WithMany()
                .HasForeignKey(cf => cf.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContractExtension>()
                .HasOne<Contract>()
                .WithMany()
                .HasForeignKey(ce => ce.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContractTermination>()
                .HasOne<Contract>()
                .WithMany()
                .HasForeignKey(ct => ct.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContractDepositTransaction>()
                .HasOne<Contract>()
                .WithMany()
                .HasForeignKey(dt => dt.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            // Constraints
            modelBuilder.Entity<Tenant>().Property(t => t.FullName).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Tenant>().Property(t => t.PhoneNumber).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Tenant>().Property(t => t.IdentityCard).HasMaxLength(50);

            modelBuilder.Entity<Contract>().Property(c => c.ContractCode).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Contract>().Property(c => c.SignedRoomNumber).HasMaxLength(100);
            modelBuilder.Entity<Contract>().Property(c => c.BranchAddress).HasMaxLength(400);
            modelBuilder.Entity<Contract>().Property(c => c.SignedRoomType).HasMaxLength(200);

            // Indexes
            modelBuilder.Entity<Tenant>().HasIndex(t => t.PhoneNumber).IsUnique(false);
            modelBuilder.Entity<Contract>().HasIndex(c => c.ContractCode).IsUnique();
        }
    }
}
