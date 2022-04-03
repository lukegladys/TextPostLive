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

public class TextPostServiceClient
{
    private readonly HttpClient _httpClient;

    public TextPostServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<TextPost>?> GetTextPosts()
    {
        var textPosts = await _httpClient.GetFromJsonAsync<IEnumerable<TextPost>>("api/posts/");

        return textPosts;
    }

    public async Task<TextPost> SaveTextPost(string post)
    {
        var newTextPost = new TextPost(post);
        await _httpClient.PostAsJsonAsync("api/newpost", newTextPost).ConfigureAwait(false);

        return newTextPost;
    }
}
