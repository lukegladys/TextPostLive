namespace TextPostLive.Data
{
    public class TextPost
    {
        public DateTime DatePosted { get; set; }
        public string? Post { get; set; }

        public TextPost(string post)
        {
            DatePosted = DateTime.UtcNow;
            Post = post;
        }
    }
}