using Microsoft.EntityFrameworkCore;
using RealEstateBE.Domain.Entities;
using RealEstateBE.Domain.Entities.Base;
using RealEstateBE.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public DbSet<PropertyImage> PropertiesImage { get; set; }  
        public DbSet<Favorite> Favorites { get; set; } 
        public DbSet<Payment> Payments { get; set; }    

        public DbSet<ContactMessage> ContactMessages { get; set;  }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>(e =>
            {

                e.HasIndex(e => e.Email).IsUnique();
                e.Property(e => e.Email).IsRequired().HasMaxLength(255);
                e.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                e.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                e.Property(e => e.PasswordHash).IsRequired();
                e.Property(e => e.PhoneNumber).HasMaxLength(20);
                e.Property(e => e.Role).HasDefaultValue(Domain.Enums.UserRole.User);
                e.HasQueryFilter(e => !e.IsDeleted);






            }






            );

            modelBuilder.Entity<Property>(e =>
            {
                e.Property(e => e.Tile).IsRequired().HasMaxLength(100);
                e.Property(e => e.Description).IsRequired().HasMaxLength(5000);
                e.Property(e => e.Price).HasPrecision(18, 2);
                e.Property(e => e.Area).HasPrecision(10, 2);
                e.Property(e => e.Latitude).HasPrecision(10, 7);
                e.Property(e => e.Longitute).HasPrecision(10, 7);
                e.Property(e => e.City).IsRequired().HasMaxLength(100);
                e.Property(e => e.District).IsRequired().HasMaxLength(100);
                e.Property(e => e.Address).IsRequired().HasMaxLength(500);
                e.HasOne(e => e.User).WithMany(u => u.Properties).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(e => e.Type);
                e.HasIndex(e => e.Status);
                e.HasIndex(e => e.City);
                e.HasIndex(e => e.Price);
                e.HasIndex(e => e.IsFeatured);
                e.HasIndex(e => e.IsPublished);

                e.HasQueryFilter(e => !e.IsDeleted);



            });

            modelBuilder.Entity<PropertyImage>(e =>
            {
                e.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
                e.HasOne(e => e.Property).WithMany(p => p.Images).HasForeignKey(a => a.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(e => e.DisplayOrder);
                e.HasQueryFilter(e => !e.IsDeleted);


            });

            modelBuilder.Entity<Favorite>(e =>
            {
                e.HasIndex(e => new
                {
                    e.UserId,
                    e.PropertyId
                }).IsUnique();

                e.HasOne(e => e.User)
                .WithMany(e => e.Favorites)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);


                e.HasOne(e => e.Property)
                .WithMany(p => p.Favorites)
                .HasForeignKey(a => a.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

                e.HasQueryFilter(e => !e.IsDeleted);

            });


            modelBuilder.Entity<Payment>(e =>
            {
                e.Property(e => e.Amount).HasPrecision(18, 2);
                e.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
                e.Property(e => e.Currency).HasDefaultValue("TRY").HasMaxLength(3);
                e.HasOne(e => e.User).WithMany(u => u.Payments).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(e => e.Property).WithMany(p => p.Payments).HasForeignKey(a => a.PropertyId).OnDelete(DeleteBehavior.SetNull);

                e.HasIndex(e => e.Status);
                e.HasIndex(e => e.TransactionId);
                e.HasQueryFilter(e => !e.IsDeleted);

            });

            modelBuilder.Entity<ContactMessage>(entity =>
            {
                // Name, Email, Subject, Message zorunlu
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(2000);

                // User ile ilişki (opsiyonel)
                entity.HasOne(e => e.User)
                    .WithMany(u => u.ContactMessages)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Property ile ilişki (opsiyonel)
                entity.HasOne(e => e.Property)
                    .WithMany(p => p.ContactMessages)
                    .HasForeignKey(e => e.PropertyId)
                    .OnDelete(DeleteBehavior.SetNull);

                // İndeksler
                entity.HasIndex(e => e.IsRead);
                entity.HasIndex(e => e.IsReplied);

                // Soft delete filter
                entity.HasQueryFilter(e => !e.IsDeleted);
            });













        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Değişen entity'leri bul
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                // Yeni eklenen kayıtlar için
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                // Güncellenen kayıtlar için
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }

                // Silinen kayıtlar için (Soft delete)
                if (entry.State == EntityState.Deleted)
                {
                    // Fiziksel olarak silme, sadece flag'i değiştir
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    



}
}
