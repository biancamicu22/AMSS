using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLibrary.StatePattern
{
    public interface State
    {
         void Change(AppTask task);
    }
}
