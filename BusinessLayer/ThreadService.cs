using DataAccessLayer.Repositories;
using DTOLayer.ReqDTO;
using DTOLayer.ResDTO;
using EntityLayer.Concrete;

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
        public ThreadResDto CreateThread(ThreadReqDto dto, string userId)
        {
            Thread thread = new()
            {
                Content = dto.Content,
                Title = dto.Title,
                AppUserId = userId,
                CategoryId = dto.CategoryId,
                ParentId = dto.ParentId,
            };
            int id = _threadDal.Insert(thread).Id;
            thread = _threadDal.Get(id);
            return new ThreadResDto
            {
                Thread = thread,
                NumOfComments = 0,
                UpVotes = 0,
                DownVotes = 0
            };
        }
    }
}
