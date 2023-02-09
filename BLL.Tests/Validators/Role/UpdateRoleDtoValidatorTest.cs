using BLL.DTO.Role;
using BLL.Infrastructure.Validators.Role;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace BLL.Tests.Validators.Role;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

public class UpdateRoleDtoValidatorTest
{
    private readonly IValidator<UpdateRoleDto> _updateRoleDtoValidator;

    public UpdateRoleDtoValidatorTest()
    {
        _updateRoleDtoValidator = new UpdateRoleDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_null_and_Id_equals_0()
    {
        //Arrange
        var faker = new Faker<UpdateRoleDto>()
            .RuleFor(x => x.Id, f => 0)
            .RuleFor(x => x.Name, f => null);

        var updateRole = faker.Generate();

        //Act
        var result = await _updateRoleDtoValidator.TestValidateAsync(updateRole);

        //Assert
        result.ShouldHaveValidationErrorFor(role => role.Id);
        result.ShouldHaveValidationErrorFor(role => role.Name);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<UpdateRoleDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(1, 150));

        var updateRole = faker.Generate();

        //Act
        var result = await _updateRoleDtoValidator.TestValidateAsync(updateRole);

        //Assert
        result.ShouldNotHaveValidationErrorFor(role => role.Id);
        result.ShouldNotHaveValidationErrorFor(role => role.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_longer_150()
    {
        //Arrange
        var faker = new Faker<UpdateRoleDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(151, 200));

        var updateRole = faker.Generate();

        //Act
        var result = await _updateRoleDtoValidator.TestValidateAsync(updateRole);

        //Assert
        result.ShouldNotHaveValidationErrorFor(role => role.Id);
        result.ShouldHaveValidationErrorFor(role => role.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<UpdateRoleDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => string.Empty);

        var updateRole = faker.Generate();

        //Act
        var result = await _updateRoleDtoValidator.TestValidateAsync(updateRole);

        //Assert
        result.ShouldNotHaveValidationErrorFor(role => role.Id);
        result.ShouldHaveValidationErrorFor(role => role.Name);
    }
}