using BLL.DTO.Warehouse;
using BLL.Infrastructure.Validators.Warehouse;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

namespace BLL.Tests.Validators.Warehouse;

public class CreateWarehouseDtoValidatorTest
{
    private readonly IValidator<CreateWarehouseDto> _createWarehouseDtoValidator;

    public CreateWarehouseDtoValidatorTest()
    {
        _createWarehouseDtoValidator = new CreateWarehouseDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_null()
    {
        //Arrange
        var faker = new Faker<CreateWarehouseDto>()
            .RuleFor(x => x.Name, f => null)
            .RuleFor(x => x.Country, f => null)
            .RuleFor(x => x.City, f => null)
            .RuleFor(x => x.Address, f => null)
            .RuleFor(x => x.PhoneNumber, f => null);

        var createWarehouse = faker.Generate();

        //Act
        var result = await _createWarehouseDtoValidator.TestValidateAsync(createWarehouse);

        //Assert
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.Name);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.Country);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.City);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.Address);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.PhoneNumber);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<CreateWarehouseDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(1, 150))
            .RuleFor(x => x.Country, f => f.Random.String2(1, 150))
            .RuleFor(x => x.City, f => f.Random.String2(1, 150))
            .RuleFor(x => x.Address, f => f.Random.String2(1, 150))
            .RuleFor(x => x.PhoneNumber, f => f.Random.String2(1, 150));

        var createWarehouse = faker.Generate();

        //Act
        var result = await _createWarehouseDtoValidator.TestValidateAsync(createWarehouse);

        //Assert
        result.ShouldNotHaveValidationErrorFor(warehouse => warehouse.Name);
        result.ShouldNotHaveValidationErrorFor(warehouse => warehouse.Country);
        result.ShouldNotHaveValidationErrorFor(warehouse => warehouse.City);
        result.ShouldNotHaveValidationErrorFor(warehouse => warehouse.Address);
        result.ShouldNotHaveValidationErrorFor(warehouse => warehouse.PhoneNumber);
    }

    [Fact]
    public async Task Should_have_error_when_values_are_greater_150()
    {
        //Arrange
        var faker = new Faker<CreateWarehouseDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(151, 200))
            .RuleFor(x => x.Country, f => f.Random.String2(151, 200))
            .RuleFor(x => x.City, f => f.Random.String2(151, 200))
            .RuleFor(x => x.Address, f => f.Random.String2(151, 200))
            .RuleFor(x => x.PhoneNumber, f => f.Random.String2(151, 200));

        var createWarehouse = faker.Generate();

        //Act
        var result = await _createWarehouseDtoValidator.TestValidateAsync(createWarehouse);

        //Assert
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.Name);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.Country);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.City);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.Address);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.PhoneNumber);
    }

    [Fact]
    public async Task Should_have_error_when_values_are_empty()
    {
        //Arrange
        var faker = new Faker<CreateWarehouseDto>()
            .RuleFor(x => x.Name, f => string.Empty)
            .RuleFor(x => x.Country, f => string.Empty)
            .RuleFor(x => x.City, f => string.Empty)
            .RuleFor(x => x.Address, f => string.Empty)
            .RuleFor(x => x.PhoneNumber, f => string.Empty);

        var createWarehouse = faker.Generate();

        //Act
        var result = await _createWarehouseDtoValidator.TestValidateAsync(createWarehouse);

        //Assert
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.Name);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.Country);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.City);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.Address);
        result.ShouldHaveValidationErrorFor(warehouse => warehouse.PhoneNumber);
    }
}