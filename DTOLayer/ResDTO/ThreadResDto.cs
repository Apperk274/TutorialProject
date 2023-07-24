using EntityLayer.Concrete;


namespace DTOLayer.ResDTO
{
    public class ThreadResDto
    {
        public Thread Thread { get; set; }
        public int NumOfComments { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public bool? IsLiked { get; set; }
    }
}
