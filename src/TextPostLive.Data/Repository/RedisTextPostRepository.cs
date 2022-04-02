using StackExchange.Redis;
using System.Text.Json;
using TextPostLive.Data.Model;

namespace TextPostLive.Data.Repository;

public class RedisTextPostRepository : ITextPostRepository
{
    private readonly IConnectionMultiplexer _redis;

    public RedisTextPostRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<IEnumerable<TextPost?>> GetTextPostsAsync()
    {
        var db = _redis.GetDatabase();

        var postList = await db.HashGetAllAsync("textposts");

        if (postList.Length > 0)
        {
            var obj = Array.ConvertAll(postList, val => JsonSerializer.Deserialize<TextPost>(val.Value))
                           .ToList();

            obj.Sort((x, y) => DateTime.Compare(y.DatePosted, x.DatePosted));
            return obj;
        }

        return new List<TextPost>();
    }

    public async Task<TextPost> SaveTextPostAsync(TextPost post)
    {
        var db = _redis.GetDatabase();
        var serialTextPost = JsonSerializer.Serialize(post);

        await db.HashSetAsync("textposts", new HashEntry[] { new HashEntry(post.Id, serialTextPost) });

        return post;
    }
}
