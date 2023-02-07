using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MMSAPI.Repository
{
    public interface IBaseRepository<T>: IRepository where T : class
    {
        T GetById(string id);
        ICollection<T> GetAll();
        T Add(T entity);
        T Edit(T entity);
        bool Delete(string id);
    }
}
