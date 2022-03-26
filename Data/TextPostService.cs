using StackExchange.Redis;

namespace TextPostLive.Data
{
    public class TextPostService
    {
        private readonly IConnectionMultiplexer _redis;

        public TextPostService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<List<string>> GetTextPostsAsync()
        {
            var db = _redis.GetDatabase();
            var listExists = await db.KeyExistsAsync(CurrentDateListKey());
            if (!listExists)
            {
                return new List<string>();
            }

            var todaysPostsList = await db.ListRangeAsync(CurrentDateListKey());

            var todaysPosts = new List<string>();
            foreach(var post in todaysPostsList)
            {
                todaysPosts.Add(post.ToString());
            }
            return todaysPosts;
        }

        public async Task<TextPost> SaveTextPostAsync(string post)
        {
            var db = _redis.GetDatabase();
            await db.ListLeftPushAsync(CurrentDateListKey(), post);

            var textPost = new TextPost(post);
            return textPost;
        }

        private static string CurrentDateListKey() => DateTime.UtcNow.ToString("yyyyMMdd");
    }
}