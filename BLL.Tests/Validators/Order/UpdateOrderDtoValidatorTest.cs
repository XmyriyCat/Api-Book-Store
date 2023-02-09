using BLL.DTO.Order;
using BLL.Infrastructure.Validators.Order;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local

namespace BLL.Tests.Validators.Order;

public class UpdateOrderDtoValidatorTest
{
    private readonly IValidator<UpdateOrderDto> _createOrderDtoValidator;

    public UpdateOrderDtoValidatorTest()
    {
        _createOrderDtoValidator = new UpdateOrderDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_negative_or_empty()
    {
        //Arrange
        var faker = new Faker<UpdateOrderDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(-500, 0))
            .RuleFor(x => x.TotalPrice, f => f.Random.Decimal(-500, -1))
            .RuleFor(x => x.OrderDate, f => new DateTime())
            .RuleFor(x => x.ShipmentId, f => f.Random.Int(-500, -1))
            .RuleFor(x => x.CustomerId, f => f.Random.Int(-500, -1));

        var updateOrderDto = faker.Generate();

        //Act
        var result = await _createOrderDtoValidator.TestValidateAsync(updateOrderDto);

        //Assert
        result.ShouldHaveValidationErrorFor(order => order.Id);
        result.ShouldHaveValidationErrorFor(order => order.TotalPrice);
        result.ShouldHaveValidationErrorFor(order => order.OrderDate);
        result.ShouldHaveValidationErrorFor(order => order.ShipmentId);
        result.ShouldHaveValidationErrorFor(order => order.CustomerId);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<UpdateOrderDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.TotalPrice, f => f.Random.Decimal())
            .RuleFor(x => x.OrderDate, f => DateTime.Now)
            .RuleFor(x => x.ShipmentId, f => f.Random.Int(1, 100))
            .RuleFor(x => x.CustomerId, f => f.Random.Int(1, 100));

        var updateOrderDto = faker.Generate();

        //Act
        var result = await _createOrderDtoValidator.TestValidateAsync(updateOrderDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(order => order.Id);
        result.ShouldNotHaveValidationErrorFor(order => order.TotalPrice);
        result.ShouldNotHaveValidationErrorFor(order => order.OrderDate);
        result.ShouldNotHaveValidationErrorFor(order => order.ShipmentId);
        result.ShouldNotHaveValidationErrorFor(order => order.CustomerId);
    }

    [Fact]
    public async Task Should_have_error_when_ShipmentId_and_CustomerId_are_0()
    {
        //Arrange
        var faker = new Faker<UpdateOrderDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.TotalPrice, f => 0)
            .RuleFor(x => x.OrderDate, f => DateTime.Now)
            .RuleFor(x => x.ShipmentId, f => 0)
            .RuleFor(x => x.CustomerId, f => 0);

        var updateOrderDto = faker.Generate();

        //Act
        var result = await _createOrderDtoValidator.TestValidateAsync(updateOrderDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(order => order.Id);
        result.ShouldNotHaveValidationErrorFor(order => order.TotalPrice);
        result.ShouldNotHaveValidationErrorFor(order => order.OrderDate);
        result.ShouldHaveValidationErrorFor(order => order.ShipmentId);
        result.ShouldHaveValidationErrorFor(order => order.CustomerId);
    }
}