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
        public new Thread Get(int id)
        {
            return _c.Set<Thread>()
                .Include(t => t.AppUser)
                .Include(t => t.Category)
                .FirstOrDefault(t => t.Id.Equals(id));
        }
        public List<Thread> GetList()
        {
            return _c.Set<Thread>()
                .Where(t => t.ParentId == null)
                .Include(t => t.AppUser)
                .Include(t => t.Category)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }
        public List<Thread> GetListByUserId(string userId)
        {
            return _c.Set<Thread>()
                .Where(t => t.ParentId == null)
                .Where(t => t.AppUser.Id == userId)
                .Include(t => t.AppUser)
                .Include(t => t.Category)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }
        public int GetNumOfCommentsOfThread(int threadId)
        {
            return _c.Set<Thread>().Where(t => t.ParentId.Equals(threadId)).Count();
        }
        public List<Thread> GetCommentsOfThread(int threadId)
        {
            return _c.Set<Thread>()
                .Where(t => t.ParentId.Equals(threadId))
                .Include(t => t.AppUser)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }
    }
}
