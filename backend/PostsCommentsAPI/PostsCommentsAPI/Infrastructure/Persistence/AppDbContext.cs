using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Domain.Entities;

namespace PostsCommentsAPI.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}