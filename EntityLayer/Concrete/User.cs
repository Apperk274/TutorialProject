using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class User
    {
        [Key] public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<Thread> Posts { get; set; }
    }
}
