using DataLibrary;
using DataLibrary.Models;
namespace MMSAPI.Repository
{
    public class SprintRepository : BaseRepository<Sprint>, ISprintRepository
    {
        public SprintRepository(MMSContext context) : base(context)
        {
        }
    }
}
