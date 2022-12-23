﻿using BLL.DTO.Book;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Book
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().Length(1, 200);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price is less than zero.");
            RuleFor(x => x.IdPublisher).NotEmpty().GreaterThan(0);
        }
    }
}
