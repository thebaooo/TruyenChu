using Microsoft.EntityFrameworkCore;

namespace TruyenChu.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryCategory> StoryCategories { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình mối quan hệ giữa Story và Author
            modelBuilder.Entity<Story>()
                .HasOne(s => s.Author)
                .WithMany(a => a.Stories)
                .HasForeignKey(s => s.AuthorId);

            // Cấu hình mối quan hệ nhiều-nhiều giữa Story và Category qua StoryCategory
            modelBuilder.Entity<StoryCategory>()
                .HasKey(sc => new { sc.StoryId, sc.CategoryId }); // Đặt khóa chính cho StoryCategory

            modelBuilder.Entity<StoryCategory>()
                .HasOne(sc => sc.Story)
                .WithMany(s => s.StoryCategories)
                .HasForeignKey(sc => sc.StoryId);

            modelBuilder.Entity<StoryCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.StoryCategories)
                .HasForeignKey(sc => sc.CategoryId);
        }
    }
}
