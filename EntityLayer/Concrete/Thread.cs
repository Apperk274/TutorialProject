using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Thread
    {
        [Key] public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int? ParentId { get; set; }
        public virtual Thread Parent { get; set; }
    }
}
