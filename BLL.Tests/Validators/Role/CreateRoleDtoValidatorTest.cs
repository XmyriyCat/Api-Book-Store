using BLL.DTO.Role;
using BLL.Infrastructure.Validators.Role;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

#pragma warning disable CS8603

namespace BLL.Tests.Validators.Role;

public class CreateRoleDtoValidatorTest
{
    private readonly IValidator<CreateRoleDto> _createRoleValidatorDto;

    public CreateRoleDtoValidatorTest()
    {
        _createRoleValidatorDto = new CreateRoleDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_null()
    {
        //Arrange
        var faker = new Faker<CreateRoleDto>()
            .RuleFor(x => x.Name, f => null);

        var createRole = faker.Generate();
        
        //Act
        var result = await _createRoleValidatorDto.TestValidateAsync(createRole);
        
        //Assert
        result.ShouldHaveValidationErrorFor(role => role.Name);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<CreateRoleDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(1,150));

        var createRole = faker.Generate();
        
        //Act
        var result = await _createRoleValidatorDto.TestValidateAsync(createRole);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(role => role.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_longer_150()
    {
        //Arrange
        var faker = new Faker<CreateRoleDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(0));

        var createRole = faker.Generate();
        
        //Act
        var result = await _createRoleValidatorDto.TestValidateAsync(createRole);
        
        //Assert
        result.ShouldHaveValidationErrorFor(role => role.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<CreateRoleDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(0));

        var createRole = faker.Generate();
        
        //Act
        var result = await _createRoleValidatorDto.TestValidateAsync(createRole);
        
        //Assert
        result.ShouldHaveValidationErrorFor(role => role.Name);
    }
}