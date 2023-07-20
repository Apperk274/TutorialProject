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

        public void Register(RegisterReqDTO registerReq)
        {

        }

        public async void LogIn(LogInReqDTO registerReq)
        {
         
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, registerReq.Email),
            };
            var userIdentity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(userIdentity);
        }
    }
}
