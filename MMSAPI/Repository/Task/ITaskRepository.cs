using DataLibrary.DTO;
using DataLibrary.Models;
namespace MMSAPI.Repository
{
    public interface ITaskRepository : IBaseRepository<AppTask>
    {
        void AssignTaskEmail(AppTask task);
        void AssignTask(string taskID);
        void CompleteTask(string taskID);
        void StatTask(string taskID);
    }
}
