using BLL.DTO.User;
using BLL.Infrastructure.Validators.User;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

namespace BLL.Tests.Validators.User;

public class LoginUserDtoValidatorTest
{
    private readonly IValidator<LoginUserDto> _loginUserDtoValidator;

    public LoginUserDtoValidatorTest()
    {
        _loginUserDtoValidator = new LoginUserDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_null()
    {
        //Arrange
        var faker = new Faker<LoginUserDto>()
            .RuleFor(x => x.Login, f => null)
            .RuleFor(x => x.Password, f => null);

        var loginUserDto = faker.Generate();

        //Act
        var result = await _loginUserDtoValidator.TestValidateAsync(loginUserDto);

        //Assert
        result.ShouldHaveValidationErrorFor(login => login.Login);
        result.ShouldHaveValidationErrorFor(login => login.Password);
    }


    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<LoginUserDto>()
            .RuleFor(x => x.Login, f => f.Random.String2(1, 150))
            .RuleFor(x => x.Password, f => f.Random.String2(1, 150));

        var loginUserDto = faker.Generate();

        //Act
        var result = await _loginUserDtoValidator.TestValidateAsync(loginUserDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(login => login.Login);
        result.ShouldNotHaveValidationErrorFor(login => login.Password);
    }

    [Fact]
    public async Task Should_have_error_when_values_greater_150()
    {
        //Arrange
        var faker = new Faker<LoginUserDto>()
            .RuleFor(x => x.Login, f => f.Random.String2(151, 200))
            .RuleFor(x => x.Password, f => f.Random.String2(151, 200));

        var loginUserDto = faker.Generate();

        //Act
        var result = await _loginUserDtoValidator.TestValidateAsync(loginUserDto);

        //Assert
        result.ShouldHaveValidationErrorFor(login => login.Login);
        result.ShouldHaveValidationErrorFor(login => login.Password);
    }

    [Fact]
    public async Task Should_have_error_when_values_are_empty()
    {
        //Arrange
        var faker = new Faker<LoginUserDto>()
            .RuleFor(x => x.Login, f => string.Empty)
            .RuleFor(x => x.Password, f => string.Empty);

        var loginUserDto = faker.Generate();

        //Act
        var result = await _loginUserDtoValidator.TestValidateAsync(loginUserDto);

        //Assert
        result.ShouldHaveValidationErrorFor(login => login.Login);
        result.ShouldHaveValidationErrorFor(login => login.Password);
    }
}