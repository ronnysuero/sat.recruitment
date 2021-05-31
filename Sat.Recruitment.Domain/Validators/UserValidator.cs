using FluentValidation;
using Sat.Recruitment.Domain.Models;

namespace Sat.Recruitment.Domain.Validators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            //Checking Required
            RuleFor(x => x.Name).NotEmpty().WithMessage("The name is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("The email is required");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email must be an valid email");
            RuleFor(x => x.Address).NotEmpty().WithMessage("The address is required");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("The phone is required");
        }
    }
}