using CommentApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommentApi.Data
{
    public class CommentDbContext(DbContextOptions<CommentDbContext> options) : DbContext(options)
    {
        public DbSet<Comment> Comments => Set<Comment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasIndex(c => c.PostId);
                entity.Property(c => c.AuthorName).HasMaxLength(100).IsRequired();
                entity.Property(c => c.Content).HasMaxLength(2000).IsRequired();
            });
        }
    }
}
