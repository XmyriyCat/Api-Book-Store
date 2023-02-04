using BLL.DTO.Publisher;
using BLL.Infrastructure.Validators.Publisher;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace BLL.Tests.Validators.Publisher;

public class UpdatePublisherDtoValidatorTest
{
    private readonly IValidator<UpdatePublisherDto> _updatePublisherDtoValidator;

    public UpdatePublisherDtoValidatorTest()
    {
        _updatePublisherDtoValidator = new UpdatePublisherDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_null_and_Id_equals_0()
    {
        //Arrange
        var faker = new Faker<UpdatePublisherDto>()
            .RuleFor(x => x.Id, f => 0)
            .RuleFor(x => x.Name, f => null);

        var updatePublisher = faker.Generate();
        
        //Act
        var result = await _updatePublisherDtoValidator.TestValidateAsync(updatePublisher);
        
        //Assert
        result.ShouldHaveValidationErrorFor(publisher => publisher.Id);
        result.ShouldHaveValidationErrorFor(publisher => publisher.Name);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<UpdatePublisherDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(1, 150));

        var updatePublisher = faker.Generate();
        
        //Act
        var result = await _updatePublisherDtoValidator.TestValidateAsync(updatePublisher);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(publisher => publisher.Id);
        result.ShouldNotHaveValidationErrorFor(publisher => publisher.Name);
    }

    [Fact]
    public async Task Should_have_error_when_is_longer_150()
    {
        //Arrange
        var faker = new Faker<UpdatePublisherDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(151, 200));

        var updatePublisher = faker.Generate();
        
        //Act
        var result = await _updatePublisherDtoValidator.TestValidateAsync(updatePublisher);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(publisher => publisher.Id);
        result.ShouldHaveValidationErrorFor(publisher => publisher.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<UpdatePublisherDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(0));

        var updatePublisher = faker.Generate();
        
        //Act
        var result = await _updatePublisherDtoValidator.TestValidateAsync(updatePublisher);
        
        //Assert
        result.ShouldHaveValidationErrorFor(publisher => publisher.Id);
        result.ShouldNotHaveValidationErrorFor(publisher => publisher.Name);
    }
}