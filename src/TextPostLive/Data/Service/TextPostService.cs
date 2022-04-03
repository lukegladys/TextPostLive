using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TextPostLive.Data.Model;
using TextPostLive.Data.Repository;

namespace TextPostLive.Data.Service;

public class TextPostService
{
    private readonly RedisTextPostRepository _redisTextPostRepository;
    private readonly SqlTextPostRepository _sqlTextPostRepository;

    public TextPostService(RedisTextPostRepository redisTextPostRepository,
                           SqlTextPostRepository sqlTextPostRepository)
    {
        _redisTextPostRepository = redisTextPostRepository;
        _sqlTextPostRepository = sqlTextPostRepository;
    }

    public async Task<IEnumerable<string>?> GetTextPostsAsync()
    {
        var textPosts = await _redisTextPostRepository.GetTextPostsAsync();

        return textPosts;
    }

    public async Task<TextPost> CacheTextPostAsync(string post)
    {
        await _redisTextPostRepository.SaveTextPostAsync(post);

        var newTextPost = new TextPost(post);
        return newTextPost;
    }

    public async Task<TextPost> SaveTextPostAsync(TextPost post)
    {
        await _sqlTextPostRepository.SaveTextPostAsync(post);
        return post;
    }
}
