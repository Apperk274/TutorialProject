using DTOLayer.ReqDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class LogInValidator : AbstractValidator<LogInReqDTO>
    {
        public LogInValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please write your e-mail.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please write your password.");
        }
    }
}
