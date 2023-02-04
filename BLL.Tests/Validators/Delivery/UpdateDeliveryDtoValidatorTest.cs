using BLL.DTO.Delivery;
using BLL.Infrastructure.Validators.Delivery;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace BLL.Tests.Validators.Delivery;

public class UpdateDeliveryDtoValidatorTest
{
    private readonly IValidator<UpdateDeliveryDto> _updateDeliveryDtoValidator;

    public UpdateDeliveryDtoValidatorTest()
    {
        _updateDeliveryDtoValidator = new UpdateDeliveryDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_negative_or_null()
    {
        //Arrange
        var faker = new Faker<UpdateDeliveryDto>()
            .RuleFor(x => x.Id, f => 0)
            .RuleFor(x => x.Name, f => null)
            .RuleFor(x => x.Price, f => f.Random.Int(-100,-1));

        var updateDeliveryDto = faker.Generate();
        
        //Act
        var result = await _updateDeliveryDtoValidator.TestValidateAsync(updateDeliveryDto);
        
        //Assert
        result.ShouldHaveValidationErrorFor(delivery => delivery.Id);
        result.ShouldHaveValidationErrorFor(delivery => delivery.Name);
        result.ShouldHaveValidationErrorFor(delivery => delivery.Price);
    }
    
    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<UpdateDeliveryDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(1,150))
            .RuleFor(x => x.Price, f => f.Random.Int(0, 150));

        var updateDeliveryDto = faker.Generate();
        
        //Act
        var result = await _updateDeliveryDtoValidator.TestValidateAsync(updateDeliveryDto);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(delivery => delivery.Id);
        result.ShouldNotHaveValidationErrorFor(delivery => delivery.Name);
        result.ShouldNotHaveValidationErrorFor(delivery => delivery.Price);
    }

    [Fact] public async Task Should_have_error_when_Name_is_longer_150()
    {
        //Arrange
        var faker = new Faker<UpdateDeliveryDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(151,250))
            .RuleFor(x => x.Price, f => f.Random.Int(5));

        var updateDeliveryDto = faker.Generate();
        
        //Act
        var result = await _updateDeliveryDtoValidator.TestValidateAsync(updateDeliveryDto);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(delivery => delivery.Id);
        result.ShouldHaveValidationErrorFor(delivery => delivery.Name);
        result.ShouldNotHaveValidationErrorFor(delivery => delivery.Price);
    }
    
    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<UpdateDeliveryDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(0))
            .RuleFor(x => x.Price, f => f.Random.Int(0, 150));

        var updateDeliveryDto = faker.Generate();
        
        //Act
        var result = await _updateDeliveryDtoValidator.TestValidateAsync(updateDeliveryDto);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(delivery => delivery.Id);
        result.ShouldHaveValidationErrorFor(delivery => delivery.Name);
        result.ShouldNotHaveValidationErrorFor(delivery => delivery.Price);
    }
}