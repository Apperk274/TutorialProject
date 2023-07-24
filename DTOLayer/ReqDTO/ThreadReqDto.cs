namespace DTOLayer.ReqDTO
{
    public class ThreadReqDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int? CategoryId { get; set; }
        public int? ParentId { get; set; }
    }
}
