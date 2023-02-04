using BLL.DTO.OrderLine;
using BLL.Infrastructure.Validators.OrderLine;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

#pragma warning disable CS8603

namespace BLL.Tests.Validators.OrderLine;

public class CreateOrderLineDtoValidatorTest
{
    private readonly IValidator<CreateOrderLineDto> _createOrderLineDtoValidator;

    public CreateOrderLineDtoValidatorTest()
    {
        _createOrderLineDtoValidator = new CreateOrderLineDtoValidator();
    }
    
    [Fact]
    public async Task Should_have_error_when_values_are_negative()
    {
        //Arrange
        var faker = new Faker<CreateOrderLineDto>()
            .RuleFor(x => x.Quantity, f => f.Random.Int(-100, -1))
            .RuleFor(x => x.OrderId, f => f.Random.Int(-100, -1))
            .RuleFor(x => x.WarehouseBookId, f => f.Random.Int(-100, -1));

        var createOrderLineDto = faker.Generate();

        //Act
        var result = await _createOrderLineDtoValidator.TestValidateAsync(createOrderLineDto);

        //Assert
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.Quantity);
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.OrderId);
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.WarehouseBookId);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<CreateOrderLineDto>()
            .RuleFor(x => x.Quantity, f => f.Random.Int(1, 100))
            .RuleFor(x => x.OrderId, f => f.Random.Int(1, 100))
            .RuleFor(x => x.WarehouseBookId, f => f.Random.Int(1, 100));

        var createOrderLineDto = faker.Generate();

        //Act
        var result = await _createOrderLineDtoValidator.TestValidateAsync(createOrderLineDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(orderLine => orderLine.Quantity);
        result.ShouldNotHaveValidationErrorFor(orderLine => orderLine.OrderId);
        result.ShouldNotHaveValidationErrorFor(orderLine => orderLine.WarehouseBookId);
    }

    [Fact]
    public async Task Should_have_error_when_values_are_0()
    {
        //Arrange
        var faker = new Faker<CreateOrderLineDto>()
            .RuleFor(x => x.Quantity, f => 0)
            .RuleFor(x => x.OrderId, f => 0)
            .RuleFor(x => x.WarehouseBookId, f => 0);

        var createOrderLineDto = faker.Generate();

        //Act
        var result = await _createOrderLineDtoValidator.TestValidateAsync(createOrderLineDto);

        //Assert
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.Quantity);
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.OrderId);
        result.ShouldHaveValidationErrorFor(orderLine => orderLine.WarehouseBookId);
    }
}