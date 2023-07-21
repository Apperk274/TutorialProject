using EntityLayer.Concrete;

namespace TutorialProject.Models
{
    public class ThreadViewModel
    {
        public Thread Thread { get; set; }
        public int NumOfComments { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
    }
}
