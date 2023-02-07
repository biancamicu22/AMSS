using DataLibrary.Models;
using Microsoft.Extensions.DependencyInjection;
using MMSAPI.Repository;
using MMSAPI.Validations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMSAPI.Validations.Validators
{
    public class SprintValidator : AbstractEntityHandler<Sprint>
    {
        public SprintValidator(Sprint newEntity, ISprintRepository sprintRepository) :
            base(newEntity, sprintRepository)
        { }

        protected override void applyChanges()
        {
            dbEntity.End_Date = newEntity.End_Date;
            dbEntity.Start_Date = newEntity.Start_Date;
            dbEntity.Name = newEntity.Name;
            dbEntity.Goal = newEntity.Goal;
        }

        protected override List<ValidationKeyValue> validate()
        {
            var validationResult = new List<ValidationKeyValue>();

            validationResult.ValidateDate("end_date", dbEntity.End_Date, DateTime.Now.AddDays(1), DateTime.Now.AddDays(365));
            validationResult.ValidateDate("start_date", dbEntity.Start_Date, DateTime.Now.AddDays(-365), dbEntity.End_Date);
            validationResult.ValidateString("goal", dbEntity.Goal, 5, 30);
            validationResult.ValidateString("name", dbEntity.Name, 5, 40);

            return validationResult;
        }
    }
}
