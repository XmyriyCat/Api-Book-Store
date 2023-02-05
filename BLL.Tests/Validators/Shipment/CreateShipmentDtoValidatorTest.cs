using BLL.DTO.Shipment;
using BLL.Infrastructure.Validators.Shipment;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local

namespace BLL.Tests.Validators.Shipment;

public class CreateShipmentDtoValidatorTest
{
    private readonly IValidator<CreateShipmentDto> _createShipmentDtoValidator;

    public CreateShipmentDtoValidatorTest()
    {
        _createShipmentDtoValidator = new CreateShipmentDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_negative()
    {
        //Arrange
        var faker = new Faker<CreateShipmentDto>()
            .RuleFor(x => x.DeliveryId, f => f.Random.Int(-10, -1))
            .RuleFor(x => x.PaymentWayId, f => f.Random.Int(-10, -1));

        var createShipment = faker.Generate();
        
        //Act
        var result = await _createShipmentDtoValidator.TestValidateAsync(createShipment);
        
        //Assert
        result.ShouldHaveValidationErrorFor(shipment => shipment.DeliveryId);
        result.ShouldHaveValidationErrorFor(shipment => shipment.PaymentWayId);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<CreateShipmentDto>()
            .RuleFor(x => x.DeliveryId, f => f.Random.Int(1))
            .RuleFor(x => x.PaymentWayId, f => f.Random.Int(1));

        var createShipment = faker.Generate();
        
        //Act
        var result = await _createShipmentDtoValidator.TestValidateAsync(createShipment);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(shipment => shipment.DeliveryId);
        result.ShouldNotHaveValidationErrorFor(shipment => shipment.PaymentWayId);
    }

    [Fact]
    public async Task Should_have_error_when_values_are_equals_0()
    {
        //Arrange
        var faker = new Faker<CreateShipmentDto>()
            .RuleFor(x => x.DeliveryId, f => 0)
            .RuleFor(x => x.PaymentWayId, f => 0);

        var createShipment = faker.Generate();
        
        //Act
        var result = await _createShipmentDtoValidator.TestValidateAsync(createShipment);
        
        //Assert
        result.ShouldHaveValidationErrorFor(shipment => shipment.DeliveryId);
        result.ShouldHaveValidationErrorFor(shipment => shipment.PaymentWayId);
    }
}