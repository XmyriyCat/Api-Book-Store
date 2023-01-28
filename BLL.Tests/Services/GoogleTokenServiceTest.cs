using BLL.Services.Implementation;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace BLL.Tests.Services
{
    public class GoogleTokenServiceTest
    {
        private readonly IConfiguration _config;
        private const string GoogleClientId = "263574892200-knp9l4edttla8kh1u2ukjma2qed2dq4s.apps.googleusercontent.com"; // For validation google token

        public GoogleTokenServiceTest()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"Authentication:Google:ClientId", GoogleClientId},
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Theory]
        [InlineData("1gh2vhb42h4b2mn4bm2nb4mn2bmn14")]
        [InlineData("eyJhbGciOiJSUzI1NiIsImtpZCI6ImQzN2FhNTA0MzgxMjkzN2ZlNDM5NjBjYTNjZjBlMjI4NGI2Z" +
                    "mMzNGQiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJh" +
                    "enAiOiIyNjM1NzQ4OTIyMDAta25wOWw0ZWR0dGxhOGtoMXUydWtqbWEycWVkMmRxNHMuYXBwcy5nb29" +
                    "nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiIyNjM1NzQ4OTIyMDAta25wOWw0ZWR0dGxhOGtoMXUydW" +
                    "tqbWEycWVkMmRxNHMuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMTA2MTc1MzQyNT" +
                    "A5NDg2MjkxNDIiLCJlbWFpbCI6ImFsZXNrZXZpYzc5QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp" +
                    "0cnVlLCJhdF9oYXNoIjoiSUlzbmQtMjRSSnp1Q09DSEFwWGJ3ZyIsIm5hbWUiOiJQYXZlbCBBbGVzaGtld" +
                    "mljaCIsInBpY3R1cmUiOiJodHRwczovL2xoMy5nb29nbGV1c2VyY29udGVudC5jb20vYS9BRWRGVHA1aVJ" +
                    "EMFlkOS1YU3FLSzZEVERJTU1xaWR0YVpIdG5faEx1OHF0Y1l3PXM5Ni1jIiwiZ2l2ZW5fbmFtZSI6IlBhdm" +
                    "VsIiwiZmFtaWx5X25hbWUiOiJBbGVzaGtldmljaCIsImxvY2FsZSI6InJ1IiwiaWF0IjoxNjc0MDQ0OTgzLC" +
                    "JleHAiOjE2NzQwNDg1ODN9.gCKPP9DSJrt6myuyaWqcyeQtpaOXdhOC58AYID897jYxBgnzrw64uOWm8yePO1" +
                    "QqSds8pvSXqG3NmSpB3E5i0QL3zvUzoShORj0PJFHnMcN6VUjhYczWh5XHzdE3BsFVkSb8Q0y1t1RTO-HSd_0O" +
                    "x8_D0FTMS6GBkvrGa--6vcUgglP7E7PPqVtBZBTMymyJqRsImwzfRN_ETivScUlcCdUB6by_7IH__qvXkrwWRM" +
                    "b87RO-Jk-JyeBjhW2CgbNkr_xV9YvCdTSFLr-bSoN4ON5ktaCKhuP-LcyA5lnqP1u7Q4stJqr6mwC3QMVUZLP" +
                    "ji-1hHXRbapZ1P2P21j3i-w")] // Expired token
        public async Task ValidateGoogleTokenAsync_Return_InvalidJwtException(string googleIdToken)
        {
            // Arrange
            var googleTokenService = new GoogleTokenService(_config);

            // Act && Assert
            await Assert.ThrowsAsync<InvalidJwtException>(() => googleTokenService.ValidateGoogleTokenAsync(googleIdToken));
        }

        [Theory]
        [InlineData("")]
        public async Task ValidateGoogleTokenAsync_Return_ArgumentException(string googleIdToken)
        {
            // Arrange
            var googleTokenService = new GoogleTokenService(_config);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => googleTokenService.ValidateGoogleTokenAsync(googleIdToken));
        }
    }
}
