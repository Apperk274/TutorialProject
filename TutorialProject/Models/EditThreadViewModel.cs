using EntityLayer.Concrete;
using System.Collections.Generic;

namespace TutorialProject.Models
{
    public class EditThreadViewModel
    {
        public Thread? Thread { get; set; }
        public List<Category> AvailableCategories { get; set; }
    }
}
