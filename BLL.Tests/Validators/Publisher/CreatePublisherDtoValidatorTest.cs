using BLL.DTO.Publisher;
using BLL.Infrastructure.Validators.Publisher;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace BLL.Tests.Validators.Publisher;

public class CreatePublisherDtoValidatorTest
{
    private readonly IValidator<CreatePublisherDto> _createPublisherDtoValidator;

    public CreatePublisherDtoValidatorTest()
    {
        _createPublisherDtoValidator = new CreatePublisherDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_null()
    {
        //Arrange
        var faker = new Faker<CreatePublisherDto>()
            .RuleFor(x => x.Name, f => null);

        var createPublisher = faker.Generate();
        
        //Act
        var result = await _createPublisherDtoValidator.TestValidateAsync(createPublisher);
        
        //Assert
        result.ShouldHaveValidationErrorFor(publisher => publisher.Name);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<CreatePublisherDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(1,150));

        var createPublisher = faker.Generate();
        
        //Act
        var result = await _createPublisherDtoValidator.TestValidateAsync(createPublisher);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(publisher => publisher.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_longer_150()
    {
        //Arrange
        var faker = new Faker<CreatePublisherDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(151, 200));

        var createPublisher = faker.Generate();
        
        //Act
        var result = await _createPublisherDtoValidator.TestValidateAsync(createPublisher);
        
        //Assert
        result.ShouldHaveValidationErrorFor(publisher => publisher.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<CreatePublisherDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(0));

        var createPublisher = faker.Generate();
        
        //Act
        var result = await _createPublisherDtoValidator.TestValidateAsync(createPublisher);
        
        //Assert
        result.ShouldHaveValidationErrorFor(publisher => publisher.Name);
    }
}