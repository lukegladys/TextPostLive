using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextPostLive.Data.Model;

namespace TextPostLive.Data.Repository
{
    public interface ITextPostRepository
    {
        Task<IEnumerable<TextPost?>> GetTextPostsAsync();
        Task<TextPost> SaveTextPostAsync(TextPost post);
    }
}
