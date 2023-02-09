using Microsoft.AspNetCore.Identity;
using Moq;
// ReSharper disable UnusedParameter.Local

namespace BLL.Tests.Infrastructure.Mock
{
    public static class UserManagerMock
    {
        // Mock UserManager<TUser> for testing 
        public static Mock<UserManager<TUser>> MockUserManagerSuccess<TUser>(List<TUser> users) where TUser : class
        {
            var storeManager = new Mock<IUserStore<TUser>>();
            var userManagerMock = new Mock<UserManager<TUser>>(storeManager.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Object.UserValidators.Add(new UserValidator<TUser>());
            userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<TUser, string>((x, y) => users.Add(x));

            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<TUser>()))
                .ReturnsAsync(IdentityResult.Success);

            return userManagerMock;
        }

        // Mock UserManager<TUser> for testing 
        public static Mock<UserManager<TUser>> MockUserManagerFailure<TUser>(List<TUser> users) where TUser : class
        {
            var storeManager = new Mock<IUserStore<TUser>>();
            var userManagerMock = new Mock<UserManager<TUser>>(storeManager.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Object.UserValidators.Add(new UserValidator<TUser>());
            userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<TUser>()))
                .ReturnsAsync(IdentityResult.Failed());

            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<TUser>()))
                .ReturnsAsync(IdentityResult.Failed());

            return userManagerMock;
        }
    }
}
