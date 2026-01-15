using Microsoft.EntityFrameworkCore;
using RealEstateBE.Domain.Entities;
using RealEstateBE.Domain.Entities.Base;
using RealEstateBE.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateBE.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Email).IsRequired().HasMaxLength(255);
                e.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                e.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                e.Property(u => u.PasswordHash).IsRequired();
                e.Property(u => u.PhoneNumber).HasMaxLength(20);
                e.Property(u => u.Role).HasDefaultValue(UserRole.User);
                e.HasQueryFilter(u => !u.IsDeleted);
            });

            modelBuilder.Entity<Property>(e =>
            {
                e.Property(p => p.Title).IsRequired().HasMaxLength(100);
                e.Property(p => p.Description).IsRequired().HasMaxLength(5000);
                e.Property(p => p.Price).HasPrecision(18, 2);
                e.Property(p => p.Area).HasPrecision(10, 2);
                e.Property(p => p.Latitude).HasPrecision(10, 7);
                e.Property(p => p.Longitute).HasPrecision(10, 7);
                e.Property(p => p.City).IsRequired().HasMaxLength(100);
                e.Property(p => p.District).IsRequired().HasMaxLength(100);
                e.Property(p => p.Address).IsRequired().HasMaxLength(500);

                e.HasOne(p => p.User)
                    .WithMany(u => u.Properties)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(p => p.Type);
                e.HasIndex(p => p.Status);
                e.HasIndex(p => p.City);
                e.HasIndex(p => p.Price);
                e.HasIndex(p => p.IsFeatured);
                e.HasIndex(p => p.IsPublished);

                e.HasQueryFilter(p => !p.IsDeleted);
            });

            modelBuilder.Entity<PropertyImage>(e =>
            {
                e.Property(pi => pi.ImageUrl).IsRequired().HasMaxLength(500);
                e.HasOne(pi => pi.Property)
                 .WithMany(p => p.Images)
                 .HasForeignKey(pi => pi.PropertyId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(pi => pi.DisplayOrder);
                e.HasQueryFilter(pi => !pi.IsDeleted);
            });

            modelBuilder.Entity<Favorite>(e =>
            {
                e.HasIndex(f => new { f.UserId, f.PropertyId }).IsUnique();

                e.HasOne(f => f.User)
                 .WithMany(u => u.Favorites)
                 .HasForeignKey(f => f.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(f => f.Property)
                 .WithMany(p => p.Favorites)
                 .HasForeignKey(f => f.PropertyId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasQueryFilter(f => !f.IsDeleted);
            });

            modelBuilder.Entity<Payment>(e =>
            {
                e.Property(p => p.Amount).HasPrecision(18, 2);
                e.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(50);
                e.Property(p => p.Currency).HasDefaultValue("TRY").HasMaxLength(3);

                e.HasOne(p => p.User)
                 .WithMany(u => u.Payments)
                 .HasForeignKey(p => p.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(p => p.Property)
                 .WithMany(pr => pr.Payments)
                 .HasForeignKey(p => p.PropertyId)
                 .OnDelete(DeleteBehavior.SetNull);

                e.HasIndex(p => p.Status);
                e.HasIndex(p => p.TransactionId);
                e.HasQueryFilter(p => !p.IsDeleted);
            });

            modelBuilder.Entity<ContactMessage>(e =>
            {
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);
                e.Property(c => c.Email).IsRequired().HasMaxLength(255);
                e.Property(c => c.Subject).IsRequired().HasMaxLength(200);
                e.Property(c => c.Message).IsRequired().HasMaxLength(2000);

                // ✅ UserId için SetNull - Kullanıcı silinse de mesaj kalır
                e.HasOne(c => c.User)
                 .WithMany(u => u.ContactMessages)
                 .HasForeignKey(c => c.UserId)
                 .OnDelete(DeleteBehavior.SetNull);

                // ✅ PropertyId için SetNull - İlan silinse de mesaj kalır
                e.HasOne(c => c.Property)
                 .WithMany(p => p.ContactMessages)
                 .HasForeignKey(c => c.PropertyId)
                 .OnDelete(DeleteBehavior.SetNull);

                e.HasIndex(c => c.IsRead);
                e.HasIndex(c => c.IsReplied);
                e.HasQueryFilter(c => !c.IsDeleted);
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}