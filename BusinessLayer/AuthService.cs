using DataAccessLayer.Repositories;
using DTOLayer.ReqDTO;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class AuthService
    {
        private readonly UserDal _userDal;
        public AuthService(UserDal userDal)
        {
            _userDal = userDal;
        }

        public User Register(RegisterReqDTO registerReq)
        {
            if (_userDal.GetBy(e => e.Email.Equals(registerReq.Email)) != null) throw new InvalidOperationException("This user already exists");
            User user = new User { Email = registerReq.Email, Name = registerReq.Name, Surname = registerReq.Surname, Password = registerReq.Password };
            return _userDal.Insert(user);
        }

        public async void LogIn(LogInReqDTO registerReq)
        {
            var user = _userDal.GetBy(e => e.Email.Equals(registerReq.Email) && e.Password.Equals(registerReq.Password));
            if (user == null) throw new InvalidOperationException("Email or password is wrong");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, registerReq.Email),
            };
            var userIdentity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(userIdentity);

            HttpContext.Current.User = user;
        }
    }
}
