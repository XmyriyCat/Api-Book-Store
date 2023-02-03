using BLL.DTO.Delivery;
using BLL.Infrastructure.Validators.Delivery;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace BLL.Tests.Validators.Delivery;

public class CreateDeliveryDtoValidatorTest
{
    private readonly IValidator<CreateDeliveryDto> _createDeliveryDtoValidator;

    public CreateDeliveryDtoValidatorTest()
    {
        _createDeliveryDtoValidator = new CreateDeliveryDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_negative_or_null()
    {
        //Arrange
        var faker = new Faker<CreateDeliveryDto>()
            .RuleFor(x => x.Name, f => null)
            .RuleFor(x => x.Price, f => f.Random.Int(-100, -1));

        var createDeliveryDro = faker.Generate();
        
        //Act
        var result = await _createDeliveryDtoValidator.TestValidateAsync(createDeliveryDro);
        
        //Assert
        result.ShouldHaveValidationErrorFor(delivery => delivery.Name);
        result.ShouldHaveValidationErrorFor(delivery => delivery.Price);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<CreateDeliveryDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(1,150))
            .RuleFor(x => x.Price, f => f.Random.Int(0, 100));

        var createDeliveryDro = faker.Generate();
        
        //Act
        var result = await _createDeliveryDtoValidator.TestValidateAsync(createDeliveryDro);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(delivery => delivery.Name);
        result.ShouldNotHaveValidationErrorFor(delivery => delivery.Price);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_longer_150()
    {
        //Arrange
        var faker = new Faker<CreateDeliveryDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(151,200))
            .RuleFor(x => x.Price, f => f.Random.Int(1, 100));

        var createDeliveryDro = faker.Generate();
        
        //Act
        var result = await _createDeliveryDtoValidator.TestValidateAsync(createDeliveryDro);
        
        //Assert
        result.ShouldHaveValidationErrorFor(delivery => delivery.Name);
        result.ShouldNotHaveValidationErrorFor(delivery => delivery.Price);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<CreateDeliveryDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(0))
            .RuleFor(x => x.Price, f => f.Random.Int(0, 100));

        var createDeliveryDro = faker.Generate();
        
        //Act
        var result = await _createDeliveryDtoValidator.TestValidateAsync(createDeliveryDro);
        
        //Assert
        result.ShouldHaveValidationErrorFor(delivery => delivery.Name);
        result.ShouldNotHaveValidationErrorFor(delivery => delivery.Price);
    }
}