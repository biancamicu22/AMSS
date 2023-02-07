using DataLibrary;
using DataLibrary.Models;
namespace MMSAPI.Repository
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(MMSContext context) : base(context)
        {
        }
    }
}
