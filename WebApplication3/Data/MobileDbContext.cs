using AutoMapper.Features;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model;

namespace WebApplication3.Data
{
    public class MobileDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public DbSet<MobileModel> Mobiles { get; set; }

        public DbSet<FeaturesModel> Features { get; set; }

        public DbSet<BrandModel> Brands { get; set; }

        public DbSet<CartModel> Cart { get; set; }

        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItemModel> OrderItems { get; set; }

        public MobileDbContext(DbContextOptions<MobileDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MobileModel>().HasQueryFilter(m => !m.IsDeleted);

            // Many-to-One: Mobile -> Brand
            modelBuilder.Entity<MobileModel>()
                .HasOne(m => m.Brand)
                .WithMany(b => b.Mobiles)
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-One: Mobile -> Feature
            modelBuilder.Entity<FeaturesModel>()
                .HasOne(f => f.Mobile)
                .WithOne(m => m.Features)
                .HasForeignKey<FeaturesModel>(f => f.MobileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Order entity
            modelBuilder.Entity<OrderModel>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<OrderModel>()
                .Property(o => o.RowVersion)
                .IsRowVersion();

            // Configure Order-User relationship
            modelBuilder.Entity<OrderModel>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure OrderItem relationships
            modelBuilder.Entity<OrderItemModel>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItemModel>()
                .HasOne(oi => oi.Mobile)
                .WithMany()
                .HasForeignKey(oi => oi.MobileId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure decimal precision for monetary values
            modelBuilder.Entity<OrderModel>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItemModel>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItemModel>()
                .Property(oi => oi.Subtotal)
                .HasPrecision(18, 2);

            // For RowVersion concurrency
            modelBuilder.Entity<OrderModel>()
        .Property(p => p.RowVersion)
        .IsRowVersion()
        .HasDefaultValueSql("CURRENT_TIMESTAMP");


            // Data Seeding in BrandModel
            modelBuilder.Entity<BrandModel>()
            .HasData(
                new BrandModel { Id = 1, Name = "Apple", Country = "USA", FoundedYear = 1976, Website = "www.apple.com", Description = "Apple Inc. is an American multinational technology company." },
                new BrandModel { Id = 2, Name = "Samsung", Country = "South Korea", FoundedYear = 1938, Website = "www.samsung.com", Description = "Samsung is a South Korean multinational conglomerate." },
                new BrandModel { Id = 3, Name = "Reliance Jio", Country = "India", FoundedYear = 2007, Website = "www.jio.com", Description = "Jio offers affordable 4G feature phones and smartphones in India." },
                new BrandModel { Id = 4, Name = "Vivo", Country = "China", FoundedYear = 2009, Website = "www.vivo.com", Description = "Vivo is a Chinese technology company specializing in smartphones." },
                new BrandModel { Id = 5, Name = "Oppo", Country = "China", FoundedYear = 2004, Website = "www.oppo.com", Description = "Oppo is a Chinese consumer electronics and mobile communications company." },
                new BrandModel { Id = 6, Name = "Xiaomi", Country = "China", FoundedYear = 2010, Website = "www.mi.com", Description = "Xiaomi is a Chinese electronics company that makes smartphones and smart home devices." },
                new BrandModel { Id = 7, Name = "OnePlus", Country = "China", FoundedYear = 2013, Website = "www.oneplus.com", Description = "OnePlus is a Chinese electronics manufacturer known for premium smartphones." },
                new BrandModel { Id = 8, Name = "Huawei", Country = "China", FoundedYear = 1987, Website = "www.huawei.com", Description = "Huawei is a Chinese multinational technology corporation." },
                new BrandModel { Id = 9, Name = "Google", Country = "USA", FoundedYear = 2016, Website = "store.google.com", Description = "Google develops the Pixel series of smartphones." },
                new BrandModel { Id = 10, Name = "Sony", Country = "Japan", FoundedYear = 1946, Website = "www.sony.com", Description = "Sony is a Japanese multinational corporation known for Xperia smartphones." },
                new BrandModel { Id = 11, Name = "Nokia", Country = "Finland", FoundedYear = 1865, Website = "www.nokia.com", Description = "Nokia is a Finnish telecommunications and consumer electronics company." },
                new BrandModel { Id = 12, Name = "Realme", Country = "China", FoundedYear = 2018, Website = "www.realme.com", Description = "Realme is a Chinese smartphone brand offering high-performance devices." },
                new BrandModel { Id = 13, Name = "Motorola", Country = "USA", FoundedYear = 1928, Website = "www.motorola.com", Description = "Motorola is an American telecommunications company known for pioneering mobile phones." },
                new BrandModel { Id = 14, Name = "Asus", Country = "Taiwan", FoundedYear = 1989, Website = "www.asus.com", Description = "Asus is a Taiwanese multinational company known for gaming smartphones." },
                new BrandModel { Id = 15, Name = "Lenovo", Country = "China", FoundedYear = 1984, Website = "www.lenovo.com", Description = "Lenovo is a Chinese technology company known for its laptops and Moto smartphones." }
            );

        }
    }
}
