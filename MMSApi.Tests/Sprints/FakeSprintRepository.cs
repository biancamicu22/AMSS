using DataLibrary;
using DataLibrary.Models;
using MMSAPI.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSApi.Tests.Sprints
{
    public class FakeSprintRepository
    {
        public static List<Sprint> Sprints;

        public static ISprintRepository GetFakeSprintRepository(int initialSprints)
        {
            var repo = new Mock<ISprintRepository>();

            Sprints = SprintHelper.GetTestData(initialSprints);

            repo.Setup(x => x.Add(It.IsAny<Sprint>())).Returns((Sprint sprint) =>
            {
                Sprints.Add(sprint);
                return sprint;
            }).Verifiable();


            repo.Setup(x => x.Delete(It.IsAny<string>())).Returns((string sprintId) =>
            {
                var sprint = Sprints.Find(s => s.Id == sprintId);
                if (sprint == null) return false;
                Sprints.Remove(sprint);
                return true;
            }).Verifiable();

            repo.Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((string sprintId) => Sprints.Find(s => s.Id == sprintId)).Verifiable();


            repo.Setup(x => x.GetAll())
                .Returns(Sprints).Verifiable();

            return repo.Object;
        }
    }
}
