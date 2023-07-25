using System;
using System.ComponentModel.DataAnnotations;


namespace EntityLayer.Concrete
{
    public class Thread : ISoftDelete
    {
        [Key] public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int? ParentId { get; set; }
        public virtual Thread Parent { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
