using BLL.DTO.WarehouseBook;
using BLL.Infrastructure.Validators.WarehouseBook;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local

namespace BLL.Tests.Validators.WarehouseBook;

public class UpdateWarehouseBookDtoValidatorTest
{
    private readonly IValidator<UpdateWarehouseBookDto> _updateWarehouseBookDtoValidator;

    public UpdateWarehouseBookDtoValidatorTest()
    {
        _updateWarehouseBookDtoValidator = new UpdateWarehouseBookDtoValidator();
    }

    [Fact]
    public async Task Should_error_when_values_are_negative()
    {
        //Arrange
        var faker = new Faker<UpdateWarehouseBookDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(-10, -1))
            .RuleFor(x => x.Quantity, f => f.Random.Int(-10, -1))
            .RuleFor(x => x.WarehouseId, f => f.Random.Int(-10, -1))
            .RuleFor(x => x.BookId, f => f.Random.Int(-10, -1));

        var updateWarehouseBook = faker.Generate();

        //Act
        var result = await _updateWarehouseBookDtoValidator.TestValidateAsync(updateWarehouseBook);

        //Assert
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.Id);
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.Quantity);
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.WarehouseId);
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.BookId);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<UpdateWarehouseBookDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Quantity, f => f.Random.Int(1))
            .RuleFor(x => x.WarehouseId, f => f.Random.Int(1))
            .RuleFor(x => x.BookId, f => f.Random.Int(1));

        var updateWarehouseBook = faker.Generate();

        //Act
        var result = await _updateWarehouseBookDtoValidator.TestValidateAsync(updateWarehouseBook);

        //Assert
        result.ShouldNotHaveValidationErrorFor(warehouseBook => warehouseBook.Id);
        result.ShouldNotHaveValidationErrorFor(warehouseBook => warehouseBook.Quantity);
        result.ShouldNotHaveValidationErrorFor(warehouseBook => warehouseBook.WarehouseId);
        result.ShouldNotHaveValidationErrorFor(warehouseBook => warehouseBook.BookId);
    }

    [Fact]
    public async Task Should_have_error_when_values_are_equals_0()
    {
        //Arrange
        var faker = new Faker<UpdateWarehouseBookDto>()
            .RuleFor(x => x.Id, f => 0)
            .RuleFor(x => x.Quantity, f => 0)
            .RuleFor(x => x.WarehouseId, f => 0)
            .RuleFor(x => x.BookId, f => 0);

        var updateWarehouseBook = faker.Generate();

        //Act
        var result = await _updateWarehouseBookDtoValidator.TestValidateAsync(updateWarehouseBook);

        //Assert
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.Id);
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.Quantity);
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.WarehouseId);
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.BookId);
    }
}