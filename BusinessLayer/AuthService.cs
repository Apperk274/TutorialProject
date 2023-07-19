using DataAccessLayer.Repositories;
using DTOLayer.ReqDTO;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
