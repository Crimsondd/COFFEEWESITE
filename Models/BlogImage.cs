namespace DACS_DAMH.Models
{
    public class BlogImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
    }
}
