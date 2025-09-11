using BAMK.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BAMK.Infrastructure.Data
{
    public class BAMKDbContext : DbContext
    {
        public BAMKDbContext(DbContextOptions<BAMKDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TShirt> TShirts { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // TShirt configuration
            modelBuilder.Entity<TShirt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
            });

            // ProductDetail configuration
            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Material).HasMaxLength(100);
                entity.Property(e => e.CareInstructions).HasMaxLength(500);
                entity.Property(e => e.Brand).HasMaxLength(100);
                entity.Property(e => e.Origin).HasMaxLength(100);
                entity.Property(e => e.Weight).HasMaxLength(50);
                entity.Property(e => e.Dimensions).HasMaxLength(100);
                entity.Property(e => e.Features).HasMaxLength(1000);
                entity.Property(e => e.AdditionalInfo).HasMaxLength(2000);
                
                entity.HasOne(e => e.TShirt)
                    .WithOne(t => t.ProductDetail)
                    .HasForeignKey<ProductDetail>(e => e.TShirtId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.OrderStatus).IsRequired().HasMaxLength(50);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
                
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.TShirt)
                    .WithMany(t => t.OrderItems)
                    .HasForeignKey(e => e.TShirtId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Question configuration
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.QuestionTitle).IsRequired().HasMaxLength(200);
                entity.Property(e => e.QuestionContent).IsRequired().HasMaxLength(2000);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Questions)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Answer configuration
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AnswerContent).IsRequired().HasMaxLength(2000);
                
                entity.HasOne(e => e.Question)
                    .WithMany(q => q.Answers)
                    .HasForeignKey(e => e.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Answers)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
