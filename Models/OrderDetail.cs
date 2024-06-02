namespace DACS_DAMH.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public int SizeId { get; set; }
        public int ToppingId { get; set; }
        public Size? size { get; set; }
        public Topping? topping { get; set; }
    }
}