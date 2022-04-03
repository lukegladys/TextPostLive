using StackExchange.Redis;
using System.Text.Json;
using TextPostLive.Data.Model;

namespace TextPostLive.Data.Repository;

public class RedisTextPostRepository
{
    private readonly IConnectionMultiplexer _redis;

    public RedisTextPostRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<IEnumerable<string>?> GetTextPostsAsync()
    {
        var db = _redis.GetDatabase();
        var listExists = await db.KeyExistsAsync("textposts");
        if (!listExists)
        {
            return new List<string>();
        }

        var todaysPostsList = await db.ListRangeAsync("textposts");

        var todaysPosts = new List<string>();
        foreach (var post in todaysPostsList)
        {
            todaysPosts.Add(post.ToString());
        }
        return todaysPosts;
    }

    public async Task<TextPost> SaveTextPostAsync(string post)
    {
        var db = _redis.GetDatabase();
        await db.ListLeftPushAsync("textposts", post);

        var textPost = new TextPost(post);
        return textPost;
    }
}
