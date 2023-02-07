using DataLibrary.Models;
using MMSAPI.Validations.Models;
using System;

namespace MMSAPI.Validations
{
    public interface IEntityUpdateHandler
    {
        EntityHandlerResult<TSuccess> Update<TSuccess>(IEntity entity) where TSuccess : class;
    }
}