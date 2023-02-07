using DataLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSApi.Tests.Users
{
    public class FakeSignInManager : SignInManager<User>
    {
        public FakeSignInManager()
            : base(new Mock<FakeUserManager>().Object,
                  new HttpContextAccessor(),
                  new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<User>>>().Object,
                  null, null)
        { }

        public static FakeSignInManager GetFakeSignInManager()
        {
            var signInManager = new Mock<FakeSignInManager>();

            signInManager.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync((string username, string password, bool isPersistent, bool lockoutOnFailuer) =>
                {
                    var user = FakeUserRepository.UserList.Find(u => u.UserName.ToLower().Equals(username.ToLower()));
                    if (user == null) return SignInResult.Failed;
                    if (user.PasswordHash.Equals(password))
                    {
                        return SignInResult.Success;
                    }
                    return SignInResult.Failed;
                });

            return signInManager.Object;
        }
    }
}
