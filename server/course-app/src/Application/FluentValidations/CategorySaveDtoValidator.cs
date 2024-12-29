﻿using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;

namespace Application.FluentValidations;

public class CategorySaveDtoValidator: AbstractValidator<CategorySaveDto>
{
    public CategorySaveDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.CategorySaveDto.NameIsRequired)
            .NotNull().WithError(ValidationErrors.CategorySaveDto.NameCanNotBeNull);
    }
}