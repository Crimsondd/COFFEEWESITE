using System.ComponentModel.DataAnnotations;

namespace DACS_DAMH.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Title { get; set; }        
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<BlogImage>? Images { get; set; }
        public string? Detail { get; set; }
        public DateTime? Datebegin { get; set; } = DateTime.Now;

    }
}
