using BLL.Services.Contract;
using Google.Apis.Auth;
using Moq;

namespace BLL.Tests.Infrastructure.Mock
{
    public class GoogleTokenServiceMock
    {
        public static Mock<IGoogleTokenService> MockGoogleTokenService(GoogleJsonWebSignature.Payload result)
        {
            var googleTokenServiceMock = new Mock<IGoogleTokenService>();
            
            googleTokenServiceMock.Setup(x => x.ValidateGoogleTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(result);
            
            return googleTokenServiceMock;
        }
    }
}
