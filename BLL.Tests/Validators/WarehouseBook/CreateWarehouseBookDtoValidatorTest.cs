using BLL.DTO.WarehouseBook;
using BLL.Infrastructure.Validators.WarehouseBook;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

#pragma warning disable CS8603

namespace BLL.Tests.Validators.WarehouseBook;

public class CreateWarehouseBookDtoValidatorTest
{
    private readonly IValidator<CreateWarehouseBookDto> _createWarehouseBookDtoValidator;

    public CreateWarehouseBookDtoValidatorTest()
    {
        _createWarehouseBookDtoValidator = new CreateWarehouseBookDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_less_0()
    {
        //Arrange
        var faker = new Faker<CreateWarehouseBookDto>()
            .RuleFor(x => x.Quantity, f => f.Random.Int(-10, -1))
            .RuleFor(x => x.WarehouseId, f => f.Random.Int(-10, -1))
            .RuleFor(x => x.BookId, f => f.Random.Int(-10, -1));

        var createWareouseBook = faker.Generate();
        
        //Act
        var result = await _createWarehouseBookDtoValidator.TestValidateAsync(createWareouseBook);
        
        //Assert
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.Quantity);
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.WarehouseId);
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.BookId);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<CreateWarehouseBookDto>()
            .RuleFor(x => x.Quantity, f => f.Random.Int(0, 150))
            .RuleFor(x => x.WarehouseId, f => f.Random.Int(1, 150))
            .RuleFor(x => x.BookId, f => f.Random.Int(1, 150));

        var createWareouseBook = faker.Generate();
        
        //Act
        var result = await _createWarehouseBookDtoValidator.TestValidateAsync(createWareouseBook);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(warehouseBook => warehouseBook.Quantity);
        result.ShouldNotHaveValidationErrorFor(warehouseBook => warehouseBook.WarehouseId);
        result.ShouldNotHaveValidationErrorFor(warehouseBook => warehouseBook.BookId);
    }

    [Fact]
    public async Task Should_have_error_when_WarehouseId_and_BookId_are_equals_0()
    {
        //Arrange
        var faker = new Faker<CreateWarehouseBookDto>()
            .RuleFor(x => x.Quantity, f => 0)
            .RuleFor(x => x.WarehouseId, f => 0)
            .RuleFor(x => x.BookId, f => 0);

        var createWareouseBook = faker.Generate();
        
        //Act
        var result = await _createWarehouseBookDtoValidator.TestValidateAsync(createWareouseBook);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(warehouseBook => warehouseBook.Quantity);
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.WarehouseId);
        result.ShouldHaveValidationErrorFor(warehouseBook => warehouseBook.BookId);
    }
}