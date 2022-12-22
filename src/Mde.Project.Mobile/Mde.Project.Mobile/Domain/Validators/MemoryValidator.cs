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
    public class MemoryValidator : AbstractValidator<Memory>
    {
        public MemoryValidator()
        {
            RuleFor(memory => memory.Title)
                        .NotEmpty()
                        .WithMessage("Title cannot be empty")
                        .MaximumLength(50)
                        .WithMessage("Length cannot be greater than 50 characters");
            RuleFor(memory => memory.Description)
                        .NotEmpty()
                        .WithMessage("Description cannot be empty")
                        .MaximumLength(100)
                        .WithMessage("Length cannot be greater than 100 characters");
            RuleFor(memory => memory.Baby)
                        .NotNull()
                        .WithMessage("Please select a baby");
        }
    }
}
