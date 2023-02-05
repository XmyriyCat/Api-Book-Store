using BLL.DTO.Genre;
using BLL.Infrastructure.Validators.Genre;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

namespace BLL.Tests.Validators.Genre;

public class UpdateGenreDtoValidatorTest
{
    private readonly IValidator<UpdateGenreDto> _updateGenreDtoValidator;

    public UpdateGenreDtoValidatorTest()
    {
        _updateGenreDtoValidator = new UpdateGenreDtoValidator();
    }

    [Fact]
    public async Task Should_have_error_when_values_are_negative_or_null()
    {
        //Arrange
        var faker = new Faker<UpdateGenreDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(-10, 0))
            .RuleFor(x => x.Name, f => null);

        var updateGenreDto = faker.Generate();

        //Act
        var result = await _updateGenreDtoValidator.TestValidateAsync(updateGenreDto);

        //Assert
        result.ShouldHaveValidationErrorFor(genre => genre.Id);
        result.ShouldHaveValidationErrorFor(genre => genre.Name);
    }

    [Fact]
    public async Task Should_not_have_error()
    {
        //Arrange
        var faker = new Faker<UpdateGenreDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(1, 150));

        var createGenreDto = faker.Generate();

        //Act
        var result = await _updateGenreDtoValidator.TestValidateAsync(createGenreDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(genre => genre.Id);
        result.ShouldNotHaveValidationErrorFor(genre => genre.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_longer_150()
    {
        //Arrange
        var faker = new Faker<UpdateGenreDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(151, 200));

        var createGenreDto = faker.Generate();

        //Act
        var result = await _updateGenreDtoValidator.TestValidateAsync(createGenreDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(genre => genre.Id);
        result.ShouldHaveValidationErrorFor(genre => genre.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        //Arrange
        var faker = new Faker<UpdateGenreDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1))
            .RuleFor(x => x.Name, f => f.Random.String2(0));

        var createGenreDto = faker.Generate();

        //Act
        var result = await _updateGenreDtoValidator.TestValidateAsync(createGenreDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(genre => genre.Id);
        result.ShouldHaveValidationErrorFor(genre => genre.Name);
    }
}
