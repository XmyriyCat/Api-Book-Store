using BLL.DTO.Genre;
using BLL.Infrastructure.Validators.Genre;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

namespace BLL.Tests.Validators.Genre;

public class CreateGenreDtoValidatorTest
{
    private readonly IValidator<CreateGenreDto> _createGenreDtoValidator;

    public CreateGenreDtoValidatorTest()
    {
        _createGenreDtoValidator = new CreateGenreDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_null()
    {
        //Arrange
        var faker = new Faker<CreateGenreDto>()
            .RuleFor(x => x.Name, f => null);

        var createGenreDto = faker.Generate();

        //Act
        var result = await _createGenreDtoValidator.TestValidateAsync(createGenreDto);

        //Assert
        result.ShouldHaveValidationErrorFor(genre => genre.Name);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<CreateGenreDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(1, 150));

        var createGenreDto = faker.Generate();

        //Act
        var result = await _createGenreDtoValidator.TestValidateAsync(createGenreDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(genre => genre.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_longer_150()
    {
        //Arrange
        var faker = new Faker<CreateGenreDto>()
            .RuleFor(x => x.Name, f => f.Random.String2(151, 200));

        var createGenreDto = faker.Generate();

        //Act
        var result = await _createGenreDtoValidator.TestValidateAsync(createGenreDto);

        //Assert
        result.ShouldHaveValidationErrorFor(genre => genre.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<CreateGenreDto>()
            .RuleFor(x => x.Name, f => string.Empty);

        var createGenreDto = faker.Generate();

        //Act
        var result = await _createGenreDtoValidator.TestValidateAsync(createGenreDto);

        //Assert
        result.ShouldHaveValidationErrorFor(genre => genre.Name);
    }
}