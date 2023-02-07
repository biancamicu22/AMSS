using AutoFixture;
using AutoFixture.Kernel;
using DataLibrary.Models;
using DataLibrary.StatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMSApi.Tests.Comments
{
    public static class CommentsHelper
    {
        public static List<Comment> GenerateComments(int count)
        {
            var comments = new List<Comment>();
            var fixture = GetFixtureForComments();
            for (int i = 0; i < count; i++)
            {
                comments.Add(fixture.Create<Comment>());
            }
            return comments;
        }

        private static Fixture GetFixtureForComments()
        {
            var fixture = new Fixture();
            fixture
                .Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));

            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(DataLibrary.StatePattern.State),
                    typeof(AssignedState))
            );

            return fixture;
        }
    }
}
