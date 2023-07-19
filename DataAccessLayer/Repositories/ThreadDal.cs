using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
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
        public List<Thread> GetCommentsOfThread(int parentId)
        {
            return GetAllBy(t => t.ParentId.Equals(parentId));
        }
    }
}
