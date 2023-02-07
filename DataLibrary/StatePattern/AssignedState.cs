using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLibrary.StatePattern
{
    public class AssignedState : State
    {
        public  void Change(AppTask task)
        {
            task.Status = "ToDo";
        }
    }
}
