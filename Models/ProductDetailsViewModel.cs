namespace DACS_DAMH.Models
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Size> Sizes { get; set; }
        public IEnumerable<Topping> Toppings { get; set; }
        public IEnumerable<Product> RelatedProducts { get; set; }
    }
}
