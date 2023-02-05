using BLL.DTO.PaymentWay;
using BLL.Infrastructure.Validators.PaymentWay;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

namespace BLL.Tests.Validators.PaymentWay;

public class UpdatePaymentWayDtoValidatorTest
{
    private readonly IValidator<UpdatePaymentWayDto> _updatePaymentWayDtoValidator;

    public UpdatePaymentWayDtoValidatorTest()
    {
        _updatePaymentWayDtoValidator = new UpdatePaymentWayDtoValidator();
    }
    
    [Fact]
    public async Task Should_have_error_when_Name_is_null_and_Id_is_0()
    {
        //Arrange
        var faker = new Faker<UpdatePaymentWayDto>()
            .RuleFor(x => x.Id, f => 0)
            .RuleFor(x => x.Name, f => null);

        var updayePaymentWay = faker.Generate();
        
        //Act
        var result = await _updatePaymentWayDtoValidator.TestValidateAsync(updayePaymentWay);
        
        //Assert
        result.ShouldHaveValidationErrorFor(paymentWay => paymentWay.Id);
        result.ShouldHaveValidationErrorFor(paymentWay => paymentWay.Name);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<UpdatePaymentWayDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(1,150));

        var updatePaymentWay = faker.Generate();
        
        //Act
        var result = await _updatePaymentWayDtoValidator.TestValidateAsync(updatePaymentWay);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(paymentWay => paymentWay.Id);
        result.ShouldNotHaveValidationErrorFor(paymentWay => paymentWay.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_longer_150()
    {
        //Arrange
        var faker = new Faker<UpdatePaymentWayDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(151,200));

        var updatePaymentWay = faker.Generate();
        
        //Act
        var result = await _updatePaymentWayDtoValidator.TestValidateAsync(updatePaymentWay);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(paymentWay => paymentWay.Id);
        result.ShouldHaveValidationErrorFor(paymentWay => paymentWay.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<UpdatePaymentWayDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(0));

        var updatePaymentWay = faker.Generate();
        
        //Act
        var result = await _updatePaymentWayDtoValidator.TestValidateAsync(updatePaymentWay);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(paymentWay => paymentWay.Id);
        result.ShouldHaveValidationErrorFor(paymentWay => paymentWay.Name);
    }
}