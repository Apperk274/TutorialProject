using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }    
        public string Surname { get; set; }
        public List<Thread> Posts { get; set; }
    }
}
