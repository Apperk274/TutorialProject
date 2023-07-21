using DTOLayer.ReqDTO;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class RegisterValidator : AbstractValidator<RegisterReqDTO>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please write your name.");
            RuleFor(x => x.Name).MinimumLength(2).WithMessage("Please enter minimum two characters");
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Please enter maximum fifty characters");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Please write your surname.");
            RuleFor(x => x.Surname).MinimumLength(2).WithMessage("Please enter minimum two characters");
            RuleFor(x => x.Surname).MaximumLength(50).WithMessage("Please enter maximum fifty characters");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please write your e-mail.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter a valid e-mail address");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please write your password.");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Please enter minimum six characters");
        }

    }
}
