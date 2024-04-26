﻿
using DACS_DAMH.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DACS_DAMH.Extentions;
using Newtonsoft.Json.Linq;
using DACS_DAMH.Paymodel;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DACS_DAMH.Repository;

namespace DACS_DAMH.Controllers
{
    [Authorize]   
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        //quản lý đăng nhập người dùng
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingCartController(IProductRepository productRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            try
            {
                var product = await GetProductFromDatabase(productId);
                var cartItem = new CartItem
                {
                    ProductId = productId,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                };
                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
                cart.AddItem(cartItem);
                HttpContext.Session.SetObjectAsJson("Cart", cart);

                // Trả về một phản hồi JSON thành công
                return Ok (new { success = true, message = "Sản phẩm đã được thêm  vào giỏ hàng.", data = cart.Items.Count() });


            }
            catch (Exception ex)
            {
                // Trả về một phản hồi JSON lỗi nếu có lỗi xảy ra
                return BadRequest(new { success = false, message = "Đã xảy ra lỗi  khi thêm sản phẩm vào giỏ hàng.", error = ex.Message });

            }


        }

        public IActionResult IndexAJAX()
        {
            try
            {
                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
                return Ok(new { success = true, data = cart.Items.Count() });
            }
            catch (Exception ex)
            {
                // Trả về một phản hồi JSON lỗi nếu có lỗi xảy ra
                return BadRequest(new { success = false, message = "Đã xảy ra lỗi  khi thêm sản phẩm vào giỏ hàng.", error = ex.Message });
            }
        }


        //tìm sản phẩm trong DB dựa vào productID
        private async Task<Product> GetProductFromDatabase(int productID)
        {
            var product = await _productRepository.GetByIdAsync(productID);
            return product;
        }
        public IActionResult Index()
        {

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                //return RedirectToAction("Index", "Home");
            }
            return View(cart);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int newQuantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.UpdateQuantity(productId, newQuantity);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(productId);
                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        //Xóa Hết giỏ hàng
        public IActionResult RemoveAllFromCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart is not null)
            {
                cart.ClearItems(); // Xóa tất cả các mặt hàng từ giỏ hàng
                                   // Lưu lại giỏ hàng vào Session sau khi đã xóa tất cả các mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        //Checkout: dùng để người dùng nhập thông tin về địa chỉ giao hàng/ ghi chú khi đặt hàng
        [HttpGet]
        public IActionResult Checkout()
        {
            return View(new Order());
        }
        [HttpPost]
        //khi người dùng nhấn nút đặt hàng thì lưu thông tin vào DB
        public async Task<IActionResult> Checkout(Order order)
        {

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.GetUserAsync(User);//lấy thông tin người dùng đã đăng nhập
            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");

            await MailUtils.MailUtils.SendMailGoogleSmtp("nguyenthanhnhan1000pro@gmail.com", user.Email, "Thanh toán thành công",
                      "Bạn đã đặt hàng thàng công. Cảm ơn bạn đã chọn chúng tôi !",
                                             "nguyenthanhnhan1000pro@gmail.com", "yqyn mmyx zjyw hqiu");

            return View("OrderCompleted", order.Id); // Trang xác nhận hoàn thành đơn hàng

        }


        public IActionResult OrderCompleted()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            cart.ClearItems();
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return View();
        }
        [Authorize]
        public async Task<IActionResult> OrderHistory()
        {
            // Lấy ID người dùng hiện tại
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Lấy lịch sử đơn hàng của người dùng từ cơ sở dữ liệu
            var orders = await _context.Orders
                                        .Include(o => o.OrderDetails)
                                        .ThenInclude(od => od.Product)
                                        .Where(o => o.UserId == userId)
                                        .OrderByDescending(o => o.OrderDate)
                                        .ToListAsync();

            return View(orders);
        }

        // xac nhan khuyen mai
        // 
        //[HttpGet]
        //public IActionResult VerifyVoucher(int totalMOMO, double giaCuoiCung, string? voucherCode)
        //{
        //    if (voucherCode == null)
        //    {
        //        ViewData["FinalPrice"] = giaCuoiCung;

        //    }
        //    else if (totalMOMO == giaCuoiCung)
        //    {
        //        ViewData["thongBao"] = "Rất tiếc. Voucher này đã hết hạn hoặc không tồn tại😣😣";
        //        ViewData["FinalPrice"] = giaCuoiCung;

        //    }
        //    else
        //    {
        //        ViewData["thongBao"] = "Chúc mừng. Bạn đã áp dụng Voucher thành công😍😍";
        //        ViewData["FinalPrice"] = giaCuoiCung;

        //    }
        //    return View();
        //}
        [HttpPost]
        public IActionResult VerifyVoucher(int totalMOMO, string? voucherCode)
        {
            int giaCuoiCung = 0;

            var voucher = _context.Discounts.FirstOrDefault(d => d.Code == voucherCode && d.Expdate > DateTime.Now);
            if (voucher != null)
            {
                if (voucher.Remain == 0)
                {
                    giaCuoiCung = totalMOMO;
                }
                else
                {

                    TempData["voucher"] = "Áp dụng thành công";
                    voucher.Remain -= 1;
                    int giaDuocGiam = (int)(totalMOMO * (voucher.Percentage / 100));
                    giaCuoiCung = totalMOMO - giaDuocGiam;
                    TempData["Totalprice"] = giaCuoiCung;
                    _context.Update(voucher);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                giaCuoiCung = totalMOMO;

            }
            return RedirectToAction("Index"); // Redirect đến action để hiển thị thông báo
        }

        public IActionResult ShowVoucherMessage()
        {
            return View(); // Trả về view để hiển thị thông báo
        }

    }
}