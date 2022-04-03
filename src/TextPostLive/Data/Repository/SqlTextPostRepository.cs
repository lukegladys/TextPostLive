using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using TextPostLive.Data.Model;

namespace TextPostLive.Data.Repository;

public class SqlTextPostRepository
{
    private readonly TextPostDbContext _context;

    public SqlTextPostRepository(TextPostDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TextPost>?> GetTextPostsAsync()
    {
        var textPosts = await _context.TextPosts.AsNoTracking()
                                                .ToListAsync();

        return textPosts;
    }

    public async Task<TextPost> SaveTextPostAsync(TextPost newTextPost)
    {
        await _context.TextPosts.AddAsync(newTextPost);
        await _context.SaveChangesAsync();

        return newTextPost;
    }
}
