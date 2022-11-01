using FluentValidation;
using Mde.Project.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Validators
{
    public class UserValidator : AbstractValidator<Mother>
    {
        public UserValidator()
        {
            RuleFor(mother => mother.Email)
                    .NotEmpty()
                    .WithMessage("E-mail cannot be empty")
                    .EmailAddress()
                    .WithMessage("Please enter a valid e-mail address");
        }
    }
}
