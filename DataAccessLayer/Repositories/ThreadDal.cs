using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ThreadDal : BaseDal<Thread>
    {
        public ThreadDal(Context c) : base(c)
        {
        }
        public new List<Thread> GetAll()
        {
            return _c.Set<Thread>().Include(t => t.User).Include(t => t.Category).OrderByDescending(t => t.CreatedAt).ToList();
        }
        public int GetNumOfCommentsOfThread(int threadId)
        {
            return _c.Set<Thread>().Where(t => t.ParentId.Equals(threadId)).Count();
        }
        public List<Thread> GetCommentsOfThread(int threadId)
        {
            return GetAllBy(t => t.ParentId.Equals(threadId));
        }
    }
}
