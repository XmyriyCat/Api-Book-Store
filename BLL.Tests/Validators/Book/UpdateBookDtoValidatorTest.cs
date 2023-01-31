using BLL.DTO.Book;
using BLL.Infrastructure.Validators.Book;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace BLL.Tests.Validators.Book;

public class UpdateBookDtoValidatorTest
{
        private readonly IValidator<UpdateBookDto> _updateBookDtoValidator;

    public UpdateBookDtoValidatorTest()
    {
        _updateBookDtoValidator = new UpdateBookDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_IdPublisher_and_Name_are_null_and_Price_and_Is_are_0()
    {
        //Arrange
        var faker = new Faker<UpdateBookDto>()
            .RuleFor(x => x.Id, f => 0)
            .RuleFor(x => x.Name, f => null)
            .RuleFor(x => x.IdPublisher, f => 0)
            .RuleFor(x => x.Price, f => -1);

        var updateBookDto = faker.Generate();
        
        //Act
        var result = await _updateBookDtoValidator.TestValidateAsync(updateBookDto);

        //Assert
        result.ShouldHaveValidationErrorFor(book => book.Id);
        result.ShouldHaveValidationErrorFor(book => book.Name);
        result.ShouldHaveValidationErrorFor(book => book.IdPublisher);
        result.ShouldHaveValidationErrorFor(book => book.Price);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<UpdateBookDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(1,200))
            .RuleFor(x => x.IdPublisher, f => f.Random.Int(1))
            .RuleFor(x=>x.Price, f => f.Random.Int(1));

        var updateBookDto = faker.Generate();
        
        //Act
        var result = await _updateBookDtoValidator.TestValidateAsync(updateBookDto);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(book => book.Id);
        result.ShouldNotHaveValidationErrorFor(book => book.Name);
        result.ShouldNotHaveValidationErrorFor(book => book.IdPublisher);
        result.ShouldNotHaveValidationErrorFor(book => book.Price);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_longer_200_and_Price_and_IdPublisher_are_less_0()
    {
        //Arrange
        var faker = new Faker<UpdateBookDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(201, 250))
            .RuleFor(x => x.IdPublisher, f => f.Random.Int(-10, -1))
            .RuleFor(x => x.Price, f => f.Random.Int(-10, -1));

        var updateBookDto = faker.Generate();
        
        //Act
        var result = await _updateBookDtoValidator.TestValidateAsync(updateBookDto);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(book => book.Id);
        result.ShouldHaveValidationErrorFor(book => book.Name);
        result.ShouldHaveValidationErrorFor(book => book.IdPublisher);
        result.ShouldHaveValidationErrorFor(book => book.Price);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<UpdateBookDto>()
            .RuleFor(x=> x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(0))
            .RuleFor(x => x.IdPublisher, f => f.Random.Int(1))
            .RuleFor(x => x.Price, f => f.Random.Int(1));

        var updateBookDto = faker.Generate();

        //Act
        var result = await _updateBookDtoValidator.TestValidateAsync(updateBookDto);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(book => book.Id);
        result.ShouldHaveValidationErrorFor(book => book.Name);
        result.ShouldNotHaveValidationErrorFor(book => book.IdPublisher);
        result.ShouldNotHaveValidationErrorFor(book => book.Price);
    }
}