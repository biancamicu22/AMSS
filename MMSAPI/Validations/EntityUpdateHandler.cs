using DataLibrary.Models;
using MMSAPI.Repository;
using MMSAPI.Validations.Models;
using MMSAPI.Validations.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMSAPI.Validations
{
    public class EntityUpdateHandler : IEntityUpdateHandler
    {
        public IServiceProvider ServiceProvider { get; }
        public ITaskRepository TaskRepository { get; }
        public ISprintRepository SprintRepository { get; }
        public ICommentRepository CommentRepository { get; }

        public EntityUpdateHandler(IServiceProvider serviceProvider, ITaskRepository taskRepository, ISprintRepository sprintRepository,
            ICommentRepository commentRepository)
        {
            ServiceProvider = serviceProvider;
            TaskRepository = taskRepository;
            SprintRepository = sprintRepository;
            CommentRepository = commentRepository;
        }

        public EntityHandlerResult<TSuccess> Update<TSuccess>(IEntity entity) where TSuccess: class
        {
            var @switch = new Dictionary<Type, Func<EntityHandlerResult<TSuccess>>> {
                { typeof(AppTask), () => new TaskValidator(entity as AppTask, TaskRepository, ServiceProvider).update<TSuccess>()},
                { typeof(Sprint), () => new SprintValidator(entity as Sprint, SprintRepository).update<TSuccess>()},
                { typeof(Comment), () => new CommentValidator(entity as Comment, CommentRepository, ServiceProvider).update<TSuccess>()},
            };

            return @switch[entity.GetType()]();
        }
    }
}
