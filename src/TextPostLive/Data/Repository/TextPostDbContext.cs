using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TextPostLive.Data.Model;

namespace TextPostLive.Data.Repository;

public class TextPostDbContext : DbContext
{
    public TextPostDbContext(DbContextOptions<TextPostDbContext> options)
    : base(options)
    {
    }

    public DbSet<TextPost> TextPosts => Set<TextPost>();
}
