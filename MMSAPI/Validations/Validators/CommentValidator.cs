using DataLibrary.Models;
using MMSAPI.Repository;
using MMSAPI.Validations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMSAPI.Validations.Validators
{
    public class CommentValidator : AbstractEntityHandler<Comment>
    {
        public IServiceProvider ServiceProvider { get; }
        public CommentValidator(Comment newEntity, ICommentRepository taskRepository, IServiceProvider serviceProvider) :
            base(newEntity, taskRepository)
        {
            ServiceProvider = serviceProvider;
        }

        protected override void applyChanges()
        {
            dbEntity.Description = newEntity.Description;
        }

        protected override List<ValidationKeyValue> validate()
        {
            var validationResult = new List<ValidationKeyValue>();

            validationResult.ValidateString("comment", dbEntity.Description, 10, 200);

            return validationResult;
        }
    }
}
