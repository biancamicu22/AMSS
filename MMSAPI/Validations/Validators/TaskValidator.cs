using DataLibrary.Models;
using Microsoft.Extensions.DependencyInjection;
using MMSAPI.Repository;
using MMSAPI.Validations.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMSAPI.Validations.Validators
{
    public class TaskValidator : AbstractEntityHandler<AppTask>
    {
        public TaskValidator(AppTask newEntity, ITaskRepository taskRepository, IServiceProvider serviceProvider) : 
            base(newEntity, taskRepository)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        protected override void applyChanges()
        {
            var userRepo = (IUserRepository)ServiceProvider.GetService(typeof(IUserRepository));
            var sprintRepo = (ISprintRepository)ServiceProvider.GetService(typeof(ISprintRepository));

            dbEntity.Description = newEntity.Description;
            dbEntity.Name = newEntity.Name;
            //dbEntity.Status = newEntity.Status;
            dbEntity.User_Id = newEntity.User_Id;
            dbEntity.User = userRepo.GetById(newEntity.User_Id);

            dbEntity.Sprint = sprintRepo.GetById(newEntity.Sprint_Id);
        }

        protected override List<ValidationKeyValue> validate()
        {
            var validationResult = new List<ValidationKeyValue>();

            validationResult.ValidateString("description", dbEntity.Description, 10, 200);
            validationResult.ValidateString("name", dbEntity.Name, 10, 50);
            //validationResult.ValidateString("status", dbEntity.Status, 1, 25);

            validationResult.RequireObject("user", dbEntity.User);
            validationResult.RequireObject("sprint", dbEntity.Sprint);

            return validationResult;
        }
    }
}
