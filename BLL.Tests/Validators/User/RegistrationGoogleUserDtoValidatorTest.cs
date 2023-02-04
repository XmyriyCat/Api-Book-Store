using BLL.DTO.User;
using BLL.Infrastructure.Validators.User;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

#pragma warning disable CS8603

namespace BLL.Tests.Validators.User;

public class RegistrationGoogleUserDtoValidatorTest
{
    private readonly IValidator<RegistrationGoogleUserDto> _registrationUserDtoValidator;

    public RegistrationGoogleUserDtoValidatorTest()
    {
        _registrationUserDtoValidator = new RegistrationGoogleUserDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_null()
    {
        //Arrange
        var faker = new Faker<RegistrationGoogleUserDto>()
            .RuleFor(x => x.GoogleToken, f => null)
            .RuleFor(x => x.Password, f => null);

        var registrationGoogleUser = faker.Generate();
        
        //Act
        var result = await _registrationUserDtoValidator.TestValidateAsync(registrationGoogleUser);
        
        //Assert
        result.ShouldHaveValidationErrorFor(google => google.GoogleToken);
        result.ShouldHaveValidationErrorFor(google => google.Password);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<RegistrationGoogleUserDto>()
            .RuleFor(x => x.GoogleToken, f => f.Random.String(15))
            .RuleFor(x => x.Password, f => f.Random.String(15));

        var registrationGoogleUser = faker.Generate();
        
        //Act
        var result = await _registrationUserDtoValidator.TestValidateAsync(registrationGoogleUser);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(google => google.GoogleToken);
        result.ShouldNotHaveValidationErrorFor(google => google.Password);
    }

    [Fact]
    public async Task Should_have_error_when_values_are_empty()
    {
        //Arrange
        var faker = new Faker<RegistrationGoogleUserDto>()
            .RuleFor(x => x.GoogleToken, f => f.Random.String(0))
            .RuleFor(x => x.Password, f => f.Random.String(0));

        var registrationGoogleUser = faker.Generate();
        
        //Act
        var result = await _registrationUserDtoValidator.TestValidateAsync(registrationGoogleUser);
        
        //Assert
        result.ShouldHaveValidationErrorFor(google => google.GoogleToken);
        result.ShouldHaveValidationErrorFor(google => google.Password);
    }
}