﻿using BLL.DTO.Publisher;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Publisher
{
    public class CreatePublisherDtoValidator : AbstractValidator<CreatePublisherDto>
    {
        public CreatePublisherDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().Length(1, 150);
        }
    }
}