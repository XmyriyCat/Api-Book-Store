using BLL.DTO.Order;
using BLL.Infrastructure.Validators.Order;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local

namespace BLL.Tests.Validators.Order;

public class CreateOrderDtoValidatorTest
{
    private readonly IValidator<CreateOrderDto> _createOrderDtoValidator;

    public CreateOrderDtoValidatorTest()
    {
        _createOrderDtoValidator = new CreateOrderDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_negative_or_empty()
    {
        //Arrange
        var faker = new Faker<CreateOrderDto>()
            .RuleFor(x => x.TotalPrice, f => f.Random.Decimal(-500, -1))
            .RuleFor(x => x.OrderDate, f => new DateTime())
            .RuleFor(x => x.ShipmentId, f => f.Random.Int(-500, -1))
            .RuleFor(x => x.CustomerId, f => f.Random.Int(-500, -1));

        var createOrderDto = faker.Generate();

        //Act
        var result = await _createOrderDtoValidator.TestValidateAsync(createOrderDto);

        //Assert
        result.ShouldHaveValidationErrorFor(order => order.TotalPrice);
        result.ShouldHaveValidationErrorFor(order => order.OrderDate);
        result.ShouldHaveValidationErrorFor(order => order.ShipmentId);
        result.ShouldHaveValidationErrorFor(order => order.CustomerId);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<CreateOrderDto>()
            .RuleFor(x => x.TotalPrice, f => f.Random.Decimal())
            .RuleFor(x => x.OrderDate, f => DateTime.Now)
            .RuleFor(x => x.ShipmentId, f => f.Random.Int(1))
            .RuleFor(x => x.CustomerId, f => f.Random.Int(1));

        var createOrderDto = faker.Generate();

        //Act
        var result = await _createOrderDtoValidator.TestValidateAsync(createOrderDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(order => order.TotalPrice);
        result.ShouldNotHaveValidationErrorFor(order => order.OrderDate);
        result.ShouldNotHaveValidationErrorFor(order => order.ShipmentId);
        result.ShouldNotHaveValidationErrorFor(order => order.CustomerId);
    }

    [Fact]
    public async Task Should_have_error_when_ShipmentId_and_CustomerId_are_0()
    {
        //Arrange
        var faker = new Faker<CreateOrderDto>()
            .RuleFor(x => x.TotalPrice, f => 0)
            .RuleFor(x => x.OrderDate, f => DateTime.Now)
            .RuleFor(x => x.ShipmentId, f => 0)
            .RuleFor(x => x.CustomerId, f => 0);

        var createOrderDto = faker.Generate();
        
        //Act
        var result = await _createOrderDtoValidator.TestValidateAsync(createOrderDto);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(order => order.TotalPrice);
        result.ShouldNotHaveValidationErrorFor(order => order.OrderDate);
        result.ShouldHaveValidationErrorFor(order => order.ShipmentId);
        result.ShouldHaveValidationErrorFor(order => order.CustomerId);
    }
}