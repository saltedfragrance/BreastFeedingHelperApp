using FluentValidation;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            RuleFor(mother => mother.PassWord)
                   .NotEmpty()
                   .WithMessage("Password cannot be empty");
        }
    }
}
