using System.ComponentModel.DataAnnotations;
using TextPostLive.Data.Models;

namespace TextPostLive.Data.Model;

public class TextPost : IEntity
{
    [Required]
    public string Id { get; set; } = $"textpost:{Guid.NewGuid()}";

    [Required]
    public string Post { get; set; }

    public DateTime DatePosted { get; set; }


    public TextPost(string post)
    {
        DatePosted = DateTime.UtcNow;
        Post = post;
    }
}
