using BLL.DTO.PaymentWay;
using BLL.Infrastructure.Validators.PaymentWay;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

namespace BLL.Tests.Validators.PaymentWay;

public class CreatePaymentWayDtoValidatorTest
{
    private readonly IValidator<CreatePaymentWayDto> _createPaymentWayDtoValidator;

    public CreatePaymentWayDtoValidatorTest()
    {
        _createPaymentWayDtoValidator = new CreatePaymentWayDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_null()
    {
        //Arrange
        var faker = new Faker<CreatePaymentWayDto>()
            .RuleFor(x => x.Name, f => null);

        var createPaymentWay = faker.Generate();
        
        //Act
        var result = await _createPaymentWayDtoValidator.TestValidateAsync(createPaymentWay);
        
        //Assert
        result.ShouldHaveValidationErrorFor(paymentWay => paymentWay.Name);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<CreatePaymentWayDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(1,150));

        var createPaymentWay = faker.Generate();
        
        //Act
        var result = await _createPaymentWayDtoValidator.TestValidateAsync(createPaymentWay);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(paymentWay => paymentWay.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_longer_150()
    {
        //Arrange
        var faker = new Faker<CreatePaymentWayDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(151,200));

        var createPaymentWay = faker.Generate();
        
        //Act
        var result = await _createPaymentWayDtoValidator.TestValidateAsync(createPaymentWay);
        
        //Assert
        result.ShouldHaveValidationErrorFor(paymentWay => paymentWay.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<CreatePaymentWayDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(0));

        var createPaymentWay = faker.Generate();
        
        //Act
        var result = await _createPaymentWayDtoValidator.TestValidateAsync(createPaymentWay);
        
        //Assert
        result.ShouldHaveValidationErrorFor(paymentWay => paymentWay.Name);
    }
}