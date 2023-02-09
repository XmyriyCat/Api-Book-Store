using BLL.DTO.Author;
using BLL.Infrastructure.Validators.Author;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

// ReSharper disable UnusedParameter.Local
#pragma warning disable CS8603

namespace BLL.Tests.Validators.Author
{
    public class UpdateAuthorDtoValidatorTest
    {
        private readonly IValidator<UpdateAuthorDto> _updateAuthorDtoValidator;

        public UpdateAuthorDtoValidatorTest()
        {
            _updateAuthorDtoValidator = new UpdateAuthorDtoValidator();
        }

        [Fact]
        public async Task Should_have_error_when_FirstName_and_LastName_are_null_and_Id_is_0()
        {
            // Arrange
            var faker = new Faker<UpdateAuthorDto>()
                .RuleFor(x => x.Id, f => 0)
                .RuleFor(x => x.FirstName, f => null)
                .RuleFor(x => x.LastName, f => null);

            var updateAuthorDto = faker.Generate();

            //Act
            var result = await _updateAuthorDtoValidator.TestValidateAsync(updateAuthorDto);

            //Assert
            result.ShouldHaveValidationErrorFor(author => author.Id);
            result.ShouldHaveValidationErrorFor(author => author.FirstName);
            result.ShouldHaveValidationErrorFor(author => author.LastName);
        }

        [Fact]
        public async Task Should_not_have_error()
        {
            // Arrange
            var faker = new Faker<UpdateAuthorDto>()
                .RuleFor(x => x.Id, f => f.Random.Int(1))
                .RuleFor(x => x.FirstName, f => f.Random.String2(1, 150))
                .RuleFor(x => x.LastName, f => f.Random.String2(1, 150));

            var updateAuthorDto = faker.Generate();

            //Act
            var result = await _updateAuthorDtoValidator.TestValidateAsync(updateAuthorDto);

            //Assert
            result.ShouldNotHaveValidationErrorFor(author => author.Id);
            result.ShouldNotHaveValidationErrorFor(author => author.FirstName);
            result.ShouldNotHaveValidationErrorFor(author => author.LastName);
        }

        [Fact]
        public async Task Should_have_error_when_LastName_and_FirstName_are_longer_150()
        {
            // Arrange
            var faker = new Faker<UpdateAuthorDto>()
                .RuleFor(x => x.Id, f => f.Random.Int(1))
                .RuleFor(x => x.FirstName, f => f.Random.String2(151, 200))
                .RuleFor(x => x.LastName, f => f.Random.String2(151, 200));

            var updateAuthorDto = faker.Generate();

            //Act
            var result = await _updateAuthorDtoValidator.TestValidateAsync(updateAuthorDto);

            //Assert
            result.ShouldNotHaveValidationErrorFor(author => author.Id);
            result.ShouldHaveValidationErrorFor(author => author.FirstName);
            result.ShouldHaveValidationErrorFor(author => author.LastName);
        }

        [Fact]
        public async Task Should_have_error_when_LastName_and_FirstName_are_empty()
        {
            // Arrange
            var faker = new Faker<UpdateAuthorDto>()
                .RuleFor(x => x.Id, f => f.Random.Int(1))
                .RuleFor(x => x.FirstName, f => string.Empty)
                .RuleFor(x => x.LastName, f => string.Empty);

            var createAuthorDto = faker.Generate();

            //Act
            var result = await _updateAuthorDtoValidator.TestValidateAsync(createAuthorDto);

            //Assert
            result.ShouldNotHaveValidationErrorFor(author => author.Id);
            result.ShouldHaveValidationErrorFor(author => author.FirstName);
            result.ShouldHaveValidationErrorFor(author => author.LastName);
        }
    }
}
