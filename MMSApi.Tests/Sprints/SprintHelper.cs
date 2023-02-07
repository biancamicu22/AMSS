using DataLibrary.Models;
using System;
using System.Collections.Generic;

namespace MMSApi.Tests.Sprints
{
    public static class SprintHelper
    {
        public static Sprint GetTestSprint(bool emptyId = false)
        {
            var randDays = (new Random()).Next(14, 14 * 12);
            return new Sprint
            {
                Start_Date = DateTime.Now.AddDays(randDays),
                End_Date = DateTime.Now.AddDays(randDays - 14),
                Goal = $"Sprint de acum {randDays} Goal",
                Id = !emptyId ? Guid.NewGuid().ToString() : null,
                Name = $"Sprint de acum {randDays}",
                Tasks = new List<AppTask>()
            };
        }

        public static List<Sprint> GetTestData(int count)
        {
            var returnData = new List<Sprint>();
            for(int i = 0; i < count; i++)
            {
                returnData.Add(new Sprint
                {
                    Start_Date = DateTime.Now.AddDays(14 * (count - i)),
                    End_Date = DateTime.Now.AddDays(14 * (count - i) - 14),
                    Goal = $"Sprint {i} Goal",
                    Id = i.ToString(),
                    Name = $"Sprint {i}",
                    Tasks = new List<AppTask>()
                });
            }
            return returnData;
        }
    }
}
