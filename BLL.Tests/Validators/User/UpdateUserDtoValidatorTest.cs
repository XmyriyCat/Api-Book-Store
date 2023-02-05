using BLL.DTO.User;
using BLL.Infrastructure.Validators.User;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

namespace BLL.Tests.Validators.User;

public class UpdateUserDtoValidatorTest
{
    private readonly IValidator<UpdateUserDto> _updateUserDtoValidator;

    public UpdateUserDtoValidatorTest()
    {
        _updateUserDtoValidator = new UpdateUserDtoValidator();
    }

    [Fact]
    public async Task Should_error_when_values_are_null_or_negative()
    {
        //Arrange
        var faker = new Faker<UpdateUserDto>()
            .RuleFor(x => x.Id, f => 0)
            .RuleFor(x => x.Username, f => null)
            .RuleFor(x => x.Login, f => null)
            .RuleFor(x => x.Email, f => null)
            .RuleFor(x => x.Country, f => null)
            .RuleFor(x => x.City, f => null)
            .RuleFor(x => x.Address, f => null)
            .RuleFor(x => x.RolesIds, f => new List<int>
            {
                f.Random.Int(-10, -1),
                f.Random.Int(-10, -1),
                f.Random.Int(-10, -1)
            });

        var updateUserDto = faker.Generate();

        //Act
        var result = await _updateUserDtoValidator.TestValidateAsync(updateUserDto);

        //Assert
        result.ShouldHaveValidationErrorFor(user => user.Id);
        result.ShouldHaveValidationErrorFor(user => user.Username);
        result.ShouldHaveValidationErrorFor(user => user.Login);
        result.ShouldNotHaveValidationErrorFor(user => user.Email);
        result.ShouldNotHaveValidationErrorFor(user => user.City);
        result.ShouldNotHaveValidationErrorFor(user => user.Country);
        result.ShouldNotHaveValidationErrorFor(user => user.Address);
        result.ShouldHaveValidationErrorFor(user => user.RolesIds);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<UpdateUserDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Username, f => f.Random.String2(1, 150))
            .RuleFor(x => x.Login, f => f.Random.String2(1, 150))
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.City, f => f.Random.String2(1, 150))
            .RuleFor(x => x.Country, f => f.Random.String2(1, 150))
            .RuleFor(x => x.Address, f => f.Random.String2(1, 150))
            .RuleFor(x => x.RolesIds, f => new List<int>
            {
                f.Random.Int(1),
                f.Random.Int(1),
                f.Random.Int(1)
            });

        var createUser = faker.Generate();

        //Act
        var result = await _updateUserDtoValidator.TestValidateAsync(createUser);

        //Assert
        result.ShouldNotHaveValidationErrorFor(user => user.Id);
        result.ShouldNotHaveValidationErrorFor(user => user.Username);
        result.ShouldNotHaveValidationErrorFor(user => user.Login);
        result.ShouldNotHaveValidationErrorFor(user => user.Email);
        result.ShouldNotHaveValidationErrorFor(user => user.City);
        result.ShouldNotHaveValidationErrorFor(user => user.Country);
        result.ShouldNotHaveValidationErrorFor(user => user.Address);
        result.ShouldNotHaveValidationErrorFor(user => user.RolesIds);
    }

    [Fact]
    public async Task Should_have_error_when_values_are_greater_150()
    {
        //Arrange
        var faker = new Faker<UpdateUserDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Username, f => f.Random.String2(151, 200))
            .RuleFor(x => x.Login, f => f.Random.String2(151, 200))
            .RuleFor(x => x.Email, f => string.Concat(f.Random.String2(151, 200), f.Internet.Email()))
            .RuleFor(x => x.Country, f => f.Random.String2(151, 200))
            .RuleFor(x => x.City, f => f.Random.String2(151, 200))
            .RuleFor(x => x.Address, f => f.Random.String2(151, 200))
            .RuleFor(x => x.RolesIds, f => new List<int>
            {
                f.Random.Int(1),
                f.Random.Int(1),
                f.Random.Int(1)
            });

        var updateUser = faker.Generate();

        //Act
        var result = await _updateUserDtoValidator.TestValidateAsync(updateUser);

        //Assert
        result.ShouldNotHaveValidationErrorFor(user => user.Id);
        result.ShouldHaveValidationErrorFor(user => user.Username);
        result.ShouldHaveValidationErrorFor(user => user.Login);
        result.ShouldHaveValidationErrorFor(user => user.Email);
        result.ShouldHaveValidationErrorFor(user => user.Country);
        result.ShouldHaveValidationErrorFor(user => user.City);
        result.ShouldHaveValidationErrorFor(user => user.Address);
        result.ShouldNotHaveValidationErrorFor(user => user.RolesIds);
    }

    [Fact]
    public async Task Should_error_when_values_are_empty_or_negative()
    {
        //Arrange
        var faker = new Faker<UpdateUserDto>()
            .RuleFor(x => x.Id, f => 0)
            .RuleFor(x => x.Username, f => string.Empty)
            .RuleFor(x => x.Login, f => string.Empty)
            .RuleFor(x => x.Email, f => string.Empty)
            .RuleFor(x => x.City, f => string.Empty)
            .RuleFor(x => x.Country, f => string.Empty)
            .RuleFor(x => x.Address, f => string.Empty)
            .RuleFor(x => x.RolesIds, f => new List<int>
            {
                f.Random.Int(-10, -1),
                f.Random.Int(-10, -1),
                f.Random.Int(-10, -1)
            });

        var updateUserDto = faker.Generate();

        //Act
        var result = await _updateUserDtoValidator.TestValidateAsync(updateUserDto);

        //Assert
        result.ShouldHaveValidationErrorFor(user => user.Id);
        result.ShouldNotHaveValidationErrorFor(user => user.Username);
        result.ShouldHaveValidationErrorFor(user => user.Login);
        result.ShouldHaveValidationErrorFor(user => user.Email);
        result.ShouldNotHaveValidationErrorFor(user => user.City);
        result.ShouldNotHaveValidationErrorFor(user => user.Country);
        result.ShouldNotHaveValidationErrorFor(user => user.Address);
        result.ShouldHaveValidationErrorFor(user => user.RolesIds);
    }
}