namespace DACS_DAMH.Models
{
    public class Topping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceTp { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
