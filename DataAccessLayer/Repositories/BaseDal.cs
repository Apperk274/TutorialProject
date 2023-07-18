using DataAccessLayer.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class BaseDal<T> where T : class
    {
        public T Get(int id)
        {
            using var c = new Context();
            return c.Set<T>().Find(id);
        }
        public List<T> GetAll()
        {
            using var c = new Context();
            return c.Set<T>().ToList();
        }
        public void Insert(T t)
        {
            using var c = new Context();
            c.Add(t);
        }
        public void Update(T t)
        {
            using var c = new Context();
            c.Update(t);
        }
        public void Delete(T t)
        {
            using var c = new Context();
            c.Remove(t);
        }
    }
}
