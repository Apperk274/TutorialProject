using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class User
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }    
        public string Surname { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
        public List<Thread> Posts { get; set; }
    }
}
