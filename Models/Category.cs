using System.ComponentModel.DataAnnotations;

namespace DACS_DAMH.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        public List<Product>? Products { get; set; }
    }
}