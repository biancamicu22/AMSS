using DataLibrary.Models;
using MMSAPI.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSApi.Tests.Users
{
    public class FakeUserRepository
    {
        public static List<User> UserList;

        public static IUserRepository GetFakeUserRepository(int initialUsers)
        {
            UserList = UserHelpers.GenerateUsersManually(initialUsers);

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAll())
                .Returns(UserList).Verifiable();

            userRepository.Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((string id) => UserList.Find(u => u.Id == id)).Verifiable();

            userRepository.Setup(x => x.Edit(It.IsAny<User>()))
                .Returns((User user) => {
                    var usrId = UserList.FindIndex(u => u.Id == u.Id);
                    if (usrId == -1) return null;
                    UserList[usrId] = user;
                    return user;
                }
            );

            return userRepository.Object;
        }
    }
}
