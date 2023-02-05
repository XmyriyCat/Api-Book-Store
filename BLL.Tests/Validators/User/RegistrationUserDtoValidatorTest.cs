using BLL.DTO.User;
using BLL.Infrastructure.Validators.User;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

namespace BLL.Tests.Validators.User;

public class RegistrationUserDtoValidatorTest
{
    private readonly IValidator<RegistrationUserDto> _registrationUserDtoValidator;

    public RegistrationUserDtoValidatorTest()
    {
        _registrationUserDtoValidator = new RegistrationUserDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_null()
    {
        //Arrange
        var faker = new Faker<RegistrationUserDto>()
            .RuleFor(x => x.Login, f => null)
            .RuleFor(x => x.Username, f => null)
            .RuleFor(x => x.Password, f => null);

        var registrationUserDto = faker.Generate();

        //Act
        var result = await _registrationUserDtoValidator.TestValidateAsync(registrationUserDto);

        //Assert
        result.ShouldHaveValidationErrorFor(userDto => userDto.Login);
        result.ShouldHaveValidationErrorFor(userDto => userDto.Username);
        result.ShouldHaveValidationErrorFor(userDto => userDto.Password);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<RegistrationUserDto>()
            .RuleFor(x => x.Username, f => f.Random.String2(0, 150))
            .RuleFor(x => x.Login, f => f.Random.String2(1, 150))
            .RuleFor(x => x.Password, f => f.Random.String2(1, 150));

        var registrationUserDto = faker.Generate();

        //Act
        var result = await _registrationUserDtoValidator.TestValidateAsync(registrationUserDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(userDto => userDto.Login);
        result.ShouldNotHaveValidationErrorFor(userDto => userDto.Username);
        result.ShouldNotHaveValidationErrorFor(userDto => userDto.Password);
    }

    [Fact]
    public async Task Should_have_error_when_values_are_greater_150()
    {
        //Arrange
        var faker = new Faker<RegistrationUserDto>()
            .RuleFor(x => x.Login, f => f.Random.String2(151, 200))
            .RuleFor(x => x.Username, f => f.Random.String2(151, 200))
            .RuleFor(x => x.Password, f => f.Random.String2(151, 200));

        var registrationUserDto = faker.Generate();

        //Act
        var result = await _registrationUserDtoValidator.TestValidateAsync(registrationUserDto);

        //Assert
        result.ShouldHaveValidationErrorFor(userDto => userDto.Login);
        result.ShouldHaveValidationErrorFor(userDto => userDto.Username);
        result.ShouldHaveValidationErrorFor(userDto => userDto.Password);
    }

    [Fact]
    public async Task Should_have_error_when_Login_and_Password_are_empty()
    {
        //Arrange
        var faker = new Faker<RegistrationUserDto>()
            .RuleFor(x => x.Login, f => string.Empty)
            .RuleFor(x => x.Username, f => string.Empty)
            .RuleFor(x => x.Password, f => string.Empty);

        var registrationUserDto = faker.Generate();

        //Act
        var result = await _registrationUserDtoValidator.TestValidateAsync(registrationUserDto);

        //Assert
        result.ShouldHaveValidationErrorFor(userDto => userDto.Login);
        result.ShouldNotHaveValidationErrorFor(userDto => userDto.Username);
        result.ShouldHaveValidationErrorFor(userDto => userDto.Password);
    }
}