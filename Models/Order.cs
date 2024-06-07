using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DACS_DAMH.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }
        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string ShippingAddress { get; set; }
        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^\d{1,11}$", ErrorMessage = "Số điện thoại chỉ được chứa số và không quá 11 chữ số.")]
        [StringLength(11, ErrorMessage = "Số điện thoại không được quá 11 chữ số.")]
        public string Numberphone { get; set; }
        public string? Notes { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
