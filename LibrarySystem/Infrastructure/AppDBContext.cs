using LibrarySystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure
{
    public class AppDBContext : DbContext
    {
        private readonly string _connectionString = @"Server=DESKTOP-DG1LLR4\SQLEXPRESS;Database=librarysystem;Trusted_Connection=True;TrustServerCertificate=True;";

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User 
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.UserName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.Password)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.Role)
                      .IsRequired();

                entity.HasMany(u => u.BorrowedBook)
                      .WithOne(bb => bb.User)
                      .HasForeignKey(bb => bb.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Reviews)
                      .WithOne(r => r.User)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<User>(entity =>
                {
                    entity.Property(u => u.PenaltyAmount)
                          .HasDefaultValue(0);
                });
            });

            //Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasMany(c => c.Books)
                      .WithOne(b => b.Category)
                      .HasForeignKey(b => b.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //Book
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Title)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(b => b.Author)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasMany(b => b.Reviews)
                      .WithOne(r => r.Book)
                      .HasForeignKey(r => r.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //BorrowedBook
            modelBuilder.Entity<BorrowedBook>(entity =>
            {
                entity.HasKey(bb => bb.Id);

                entity.Property(bb => bb.BorrowTime)
                      .IsRequired();

                entity.HasOne(bb => bb.User)
                      .WithMany(u => u.BorrowedBook)
                      .HasForeignKey(bb => bb.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bb => bb.Book)
                      .WithMany()
                      .HasForeignKey(bb => bb.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //Review
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Comment)
                      .HasMaxLength(100);

                entity.Property(r => r.Rating)
                      .IsRequired();

                entity.Property(r => r.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(r => r.IsApproved)
                      .HasDefaultValue(false);

                entity.HasOne(r => r.User)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Book)
                      .WithMany(b => b.Reviews)
                      .HasForeignKey(r => r.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Wishlist
            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasKey(w => w.Id);

                entity.HasOne(w => w.User)
                      .WithMany(u => u.Wishlists)
                      .HasForeignKey(w => w.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(w => w.Book)
                      .WithMany()
                      .HasForeignKey(w => w.BookId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(w => w.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
