using DataAccessLayer.Repositories;
using DTOLayer.ResDTO;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ThreadService
    {
        private readonly ThreadDal _threadDal;
        public ThreadService(ThreadDal threadDal)
        {
            _threadDal = threadDal;
        }
        public ThreadResDto GetThreadDetails(int threadId)
        {
            Thread thread = _threadDal.Get(threadId);
            int numOfComments = _threadDal.GetNumOfCommentsOfThread(threadId);
            return new ThreadResDto() { NumOfComments = numOfComments, Thread = thread };
        }
    }
}
