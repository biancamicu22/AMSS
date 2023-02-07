using DataLibrary.Models;
using MMSAPI.Repository;
using MMSAPI.Validations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MMSAPI.Validations
{
    public abstract class AbstractEntityHandler<T> where T: IEntity
    {
        private object repository { get; set; }
        protected T dbEntity { get; set; }
        protected T newEntity { get; set; }
        public AbstractEntityHandler(T newEntity, IRepository repository)
        {
            this.repository = repository;

            this.newEntity = newEntity;
            this.dbEntity = get();
        }



        protected X GetRepository<X>()
        {
            return (X)repository;
        }


        private Type GetRepoType()
        {
            var @switch = new Dictionary<Type, Type> {
                { typeof(AppTask), typeof(TaskRepository)},
                { typeof(Sprint), typeof(SprintRepository)},
                { typeof(Comment), typeof(CommentRepository)},
            };

            return @switch[typeof(T)];
        }

        protected virtual T get()
        {
            MethodInfo methodDefinition = GetRepoType().GetMethod("GetById");
            var result = methodDefinition.Invoke(repository, new object[] { (newEntity.GetType().GetProperty("Id").GetValue(newEntity)) });

            return (T)result;
        }

        protected abstract void applyChanges();

        protected abstract List<ValidationKeyValue> validate();

        public virtual EntityHandlerResult<TSuccess> update<TSuccess>() where TSuccess: class
        {
            if (dbEntity == null)
            {
                return new EntityHandlerResult<TSuccess>(new List<ValidationKeyValue> { new ValidationKeyValue("Object", "Obiectul nu exista!") }, null);
            }

            applyChanges();

            var validationResult = validate();

            var entityHandlerResult = new EntityHandlerResult<TSuccess>(validationResult, null);

            if (entityHandlerResult.ValidationResults.Count > 0) return entityHandlerResult;

            MethodInfo methodDefinition = GetRepoType().GetMethod("Edit");

            entityHandlerResult.SuccessResult = (TSuccess)methodDefinition.Invoke(repository, new object[] { dbEntity });

            return entityHandlerResult;
        }
    }
}
