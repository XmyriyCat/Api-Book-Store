using BLL.DTO.Book;
using BLL.Infrastructure.Validators.Book;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

namespace BLL.Tests.Validators.Book;

public class CreateBookDtoValidatorTest
{
    private readonly IValidator<CreateBookDto> _createBookDtoValidator;

    public CreateBookDtoValidatorTest()
    {
        _createBookDtoValidator = new CreateBookDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_negative_or_null()
    {
        //Arrange
        var faker = new Faker<CreateBookDto>()
            .RuleFor(x => x.Name, f => null)
            .RuleFor(x => x.IdPublisher, f => f.Random.Int(-10, 0))
            .RuleFor(x => x.Price, f => f.Random.Int(-500, -1))
            .RuleFor(x => x.AuthorsId, f => new List<int>
            {
                f.Random.Int(-10, 0),
                f.Random.Int(-10, 0),
                f.Random.Int(-10, 0)
            })
            .RuleFor(x => x.GenresId, f => new List<int>
            {
                f.Random.Int(-10, 0),
                f.Random.Int(-10, 0),
                f.Random.Int(-10, 0)
            });

        var createBookDto = faker.Generate();

        //Act
        var result = await _createBookDtoValidator.TestValidateAsync(createBookDto);

        //Assert
        result.ShouldHaveValidationErrorFor(book => book.Name);
        result.ShouldHaveValidationErrorFor(book => book.IdPublisher);
        result.ShouldHaveValidationErrorFor(book => book.Price);
        result.ShouldHaveValidationErrorFor(book => book.AuthorsId);
        result.ShouldHaveValidationErrorFor(book => book.GenresId);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<CreateBookDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(1, 200))
            .RuleFor(x => x.IdPublisher, f => f.Random.Int(1))
            .RuleFor(x => x.Price, f => f.Random.Decimal(1))
            .RuleFor(x => x.AuthorsId, f => new List<int>
            {
                f.Random.Int(1, 100),
                f.Random.Int(1, 100),
                f.Random.Int(1, 100)
            })
            .RuleFor(x => x.GenresId, f => new List<int>
            {
                f.Random.Int(1, 100),
                f.Random.Int(1, 100),
                f.Random.Int(1, 100)
            });

        var createBookDto = faker.Generate();

        //Act
        var result = await _createBookDtoValidator.TestValidateAsync(createBookDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(book => book.Name);
        result.ShouldNotHaveValidationErrorFor(book => book.IdPublisher);
        result.ShouldNotHaveValidationErrorFor(book => book.Price);
        result.ShouldNotHaveValidationErrorFor(book => book.AuthorsId);
        result.ShouldNotHaveValidationErrorFor(book => book.GenresId);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_longer_200_and_Price_and_IdPublisher_AuthorsId_GenresId_are_less_0()
    {
        //Arrange
        var faker = new Faker<CreateBookDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(201, 250))
            .RuleFor(x => x.IdPublisher, f => f.Random.Int(-10, 0))
            .RuleFor(x => x.Price, f => f.Random.Decimal(-10, -1))
            .RuleFor(x => x.AuthorsId, f => new List<int>
            {
                f.Random.Int(-10, 0),
                f.Random.Int(-10, 0),
                f.Random.Int(-10, 0)
            })
            .RuleFor(x => x.GenresId, f => new List<int>
            {
                f.Random.Int(-10, 0),
                f.Random.Int(-10, 0),
                f.Random.Int(-10, 0)
            });

        var createBookDto = faker.Generate();

        //Act
        var result = await _createBookDtoValidator.TestValidateAsync(createBookDto);

        //Assert
        result.ShouldHaveValidationErrorFor(book => book.Name);
        result.ShouldHaveValidationErrorFor(book => book.IdPublisher);
        result.ShouldHaveValidationErrorFor(book => book.Price);
        result.ShouldHaveValidationErrorFor(book => book.AuthorsId);
        result.ShouldHaveValidationErrorFor(book => book.GenresId);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<CreateBookDto>()
            .RuleFor(x => x.Name, f => string.Empty)
            .RuleFor(x => x.IdPublisher, f => f.Random.Int(1))
            .RuleFor(x => x.Price, f => f.Random.Int(1))
            .RuleFor(x => x.AuthorsId, f => new List<int>
            {
                f.Random.Int(1, 100),
                f.Random.Int(1, 100),
                f.Random.Int(1, 100)
            })
            .RuleFor(x => x.GenresId, f => new List<int>
            {
                f.Random.Int(1, 100),
                f.Random.Int(1, 100),
                f.Random.Int(1, 100)
            });

        var createBookDto = faker.Generate();

        //Act
        var result = await _createBookDtoValidator.TestValidateAsync(createBookDto);

        //Assert
        result.ShouldHaveValidationErrorFor(book => book.Name);
        result.ShouldNotHaveValidationErrorFor(book => book.IdPublisher);
        result.ShouldNotHaveValidationErrorFor(book => book.Price);
        result.ShouldNotHaveValidationErrorFor(book => book.AuthorsId);
        result.ShouldNotHaveValidationErrorFor(book => book.GenresId);
    }
}