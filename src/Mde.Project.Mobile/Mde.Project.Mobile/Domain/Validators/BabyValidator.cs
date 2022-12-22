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
    public class BabyValidator : AbstractValidator<Baby>
    {
        public BabyValidator()
        {
            RuleFor(baby => baby.FirstName)
                        .NotEmpty()
                        .WithMessage("First name cannot be empty")
                        .MaximumLength(20)
                        .WithMessage("Length cannot be greater than 50 characters");
            RuleFor(baby => baby.DateOfBirth)
                        .Must(ValidBirthDate)
                        .WithMessage("Babies are only babies until 4 years old.");
            RuleFor(baby => baby.DateOfBirth)
                        .Must(FutureBirthDate)
                        .WithMessage("No babies from the future.");
            RuleFor(baby => baby.Weight)
                        .GreaterThan(3)
                        .WithMessage("Babies cannot weigh less than 3kg");
            RuleFor(baby => baby.Height)
                        .GreaterThan(40)
                        .WithMessage("Babies cannot be less than 40cm tall");
        }
        private bool ValidBirthDate(DateTime birthDate)
        {
            if ((DateTime.Now.Year - birthDate.Year) > 4) return false;
            else return true;
        }
        private bool FutureBirthDate(DateTime birthDate)
        {
            if (DateTime.Now < birthDate) return false;
            else return true;
        }
    }
}
