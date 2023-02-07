using DataLibrary.Models;
using System.Linq;

namespace DataLibrary.StatePattern
{
    public class CreateadState : State
    {
        public  void Change(AppTask task)
        {
           task.Status = "Created";
        }
    }
}
