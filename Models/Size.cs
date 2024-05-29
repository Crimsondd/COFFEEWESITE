using System.ComponentModel.DataAnnotations;

namespace DACS_DAMH.Models
{
    public class Size
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        public int AddPrice { get; set; }

    }
}
