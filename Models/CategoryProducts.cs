namespace DACS_DAMH.Models
{
    public class CategoryProducts
    {
        public string CategoryName { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
