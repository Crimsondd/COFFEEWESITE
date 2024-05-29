namespace DACS_DAMH.Models
{
    public class NavigationItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; } // Đường dẫn của mục
        public List<NavigationItemViewModel> Children { get; set; } = new List<NavigationItemViewModel>();
        public bool HasChildren => Children.Any(); // Kiểm tra xem mục có mục con không
    }
}
