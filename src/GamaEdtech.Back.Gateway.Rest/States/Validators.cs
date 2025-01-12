﻿using FluentValidation;

namespace GamaEdtech.Back.Gateway.Rest.States;

public class AddStateDtoValidator : AbstractValidator<AddStateDto>
{
	public AddStateDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotNull().WithMessage("name is required")
			.NotEmpty().WithMessage("name is required")
			.MaximumLength(50).WithMessage("name is too long");

		RuleFor(x => x.Code)
			.NotNull().WithMessage("code is required")
			.MaximumLength(5).WithMessage("code is too long");
	}
}

