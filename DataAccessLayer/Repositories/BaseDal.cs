using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class BaseDal<T> where T : class
    {
        protected readonly Context _c;
        public BaseDal(Context c)
        {
            _c = c;
        }
        public T Get(int id)
        {
            return _c.Set<T>().Find(id);
        }
        public T GetBy(Expression<Func<T, bool>> predicate)
        {
            return _c.Set<T>().FirstOrDefault(predicate);
        }
        public List<T> GetAll()
        {
            return _c.Set<T>().ToList();
        }
        public List<T> GetAllBy(Expression<Func<T, bool>> predicate)
        {
            return _c.Set<T>().Where(predicate).ToList();
        }
        public void Insert(T t)
        {
            _c.Add(t);
        }
        public void Update(T t)
        {
            _c.Update(t);
        }
        public void Delete(T t)
        {
            _c.Remove(t);
        }
    }
}
