using BLL.DTO.OrderLine;
using BLL.Infrastructure.Validators.OrderLine;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

#pragma warning disable CS8603

namespace BLL.Tests.Validators.OrderLine;

public class UpdateOrderLineDtoValidatorTest
{
    private readonly IValidator<UpdateOrderLineDto> _updateOrderLineDtoValidator;

    public UpdateOrderLineDtoValidatorTest()
    {
        _updateOrderLineDtoValidator = new UpdateOrderLineDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_negative()
    {
        //Arrange
        var faker = new Faker<UpdateOrderLineDto>()
            .RuleFor(x => x.Id, f => 0)
            .RuleFor(x => x.Quantity, f => f.Random.Int(-100, -1))
            .RuleFor(x => x.OrderId, f => f.Random.Int(-100, -1))
            .RuleFor(x => x.WarehouseBookId, f => f.Random.Int(-100, -1));

        var updateOrderLineDto = faker.Generate();

        //Act
        var result = await _updateOrderLineDtoValidator.TestValidateAsync(updateOrderLineDto);

        //Assert
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.Id);
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.Quantity);
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.OrderId);
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.WarehouseBookId);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<UpdateOrderLineDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Quantity, f => f.Random.Int(1, 100))
            .RuleFor(x => x.OrderId, f => f.Random.Int(1, 100))
            .RuleFor(x => x.WarehouseBookId, f => f.Random.Int(1, 100));

        var updateOrderLineDto = faker.Generate();

        //Act
        var result = await _updateOrderLineDtoValidator.TestValidateAsync(updateOrderLineDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(orderLine => orderLine.Id);
        result.ShouldNotHaveValidationErrorFor(orderLine => orderLine.Quantity);
        result.ShouldNotHaveValidationErrorFor(orderLine => orderLine.OrderId);
        result.ShouldNotHaveValidationErrorFor(orderLine => orderLine.WarehouseBookId);
    }

    [Fact]
    public async Task Should_have_error_when_values_are_0()
    {
        //Arrange
        var faker = new Faker<UpdateOrderLineDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Quantity, f => 0)
            .RuleFor(x => x.OrderId, f => 0)
            .RuleFor(x => x.WarehouseBookId, f => 0);

        var updateOrderLineDto = faker.Generate();

        //Act
        var result = await _updateOrderLineDtoValidator.TestValidateAsync(updateOrderLineDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(orderLine => orderLine.Id);
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.Quantity);
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.OrderId);
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.WarehouseBookId);
    }
}