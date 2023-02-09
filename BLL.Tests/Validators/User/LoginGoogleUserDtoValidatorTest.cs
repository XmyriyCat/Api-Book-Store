using BLL.DTO.User;
using BLL.Infrastructure.Validators.User;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

namespace BLL.Tests.Validators.User;

public class LoginGoogleUserDtoValidatorTest
{
    private readonly IValidator<LoginGoogleUserDto> _loginGoogleUserDtoValidator;

    public LoginGoogleUserDtoValidatorTest()
    {
        _loginGoogleUserDtoValidator = new LoginGoogleUserDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_GoogleToken_is_null()
    {
        //Arrange
        var faker = new Faker<LoginGoogleUserDto>()
           .RuleFor(x => x.GoogleToken, f => null);

        var loginGoogleUserToken = faker.Generate();

        //Act
        var result = await _loginGoogleUserDtoValidator.TestValidateAsync(loginGoogleUserToken);

        //Assert
        result.ShouldHaveValidationErrorFor(loginGoogle => loginGoogle.GoogleToken);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<LoginGoogleUserDto>()
           .RuleFor(x => x.GoogleToken, f => f.Random.String2(1, 150));

        var loginGoogleUserToken = faker.Generate();

        //Act
        var result = await _loginGoogleUserDtoValidator.TestValidateAsync(loginGoogleUserToken);

        //Assert
        result.ShouldNotHaveValidationErrorFor(loginGoogle => loginGoogle.GoogleToken);
    }

    [Fact]
    public async Task Should_have_error_when_GoogleToken_is_empty()
    {
        //Arrange
        var faker = new Faker<LoginGoogleUserDto>()
           .RuleFor(x => x.GoogleToken, f => string.Empty);

        var loginGoogleUserToken = faker.Generate();

        //Act
        var result = await _loginGoogleUserDtoValidator.TestValidateAsync(loginGoogleUserToken);

        //Assert
        result.ShouldHaveValidationErrorFor(loginGoogle => loginGoogle.GoogleToken);
    }
}