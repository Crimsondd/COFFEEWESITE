using DACS_DAMH.Models;
using DACS_DAMH.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DACS_DAMH.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DiscountController : Controller
    {
        private readonly IDiscountRepository _discountRepository;
        public DiscountController(IDiscountRepository discountRepository )
        {
            _discountRepository = discountRepository;
        }
        // Hiển thị danh sách sản phẩm
        [AllowAnonymous]
        public async Task<IActionResult> Index(string postTitle)
        {
            IEnumerable<Discount> discounts;

            if (postTitle != null)
            {
                discounts = await _discountRepository.SearchAsync(int.Parse(postTitle));
            }
            else
            {
                discounts = await _discountRepository.GetAllAsync();
            }

            return View(discounts);
        }
        // Hiển thị form thêm sản phẩm mới
        [AllowAnonymous]
       
        public async Task<IActionResult> Add()
        {
            var discounts = await _discountRepository.GetAllAsync();
            ViewBag.Discounts = new SelectList(discounts, "Id", "Name");
            return View();
        }
        // Xử lý thêm sản phẩm mới
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add(Discount discount)
        {
            if (ModelState.IsValid)
            {
                await _discountRepository.AddAsync(discount);
                return RedirectToAction(nameof(Index));
            }
            var discounts = await _discountRepository.GetAllAsync();
            ViewBag.Discounts = new SelectList(discounts, "Id", "Name");
            return View(discounts);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Update(int id)
        {
            var discount = await _discountRepository.GetByIdAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, Discount discount)
        {
            if (id != discount.IdDiscount)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingdiscount = await
                _discountRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync


                // Cập nhật các thông tin khác của sản phẩm
                existingdiscount.Code = discount.Code;
                existingdiscount.Percentage = discount.Percentage;
                existingdiscount.Expdate = discount.Expdate;
                existingdiscount.Remain = discount.Remain;
                await _discountRepository.UpdateAsync(existingdiscount);
                return RedirectToAction(nameof(Index));
            }
            return View(discount);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var DiscountToDelete = await _discountRepository.GetByIdAsync(id);
            if (DiscountToDelete == null)
            {
                return NotFound();
            }

            await _discountRepository.DeleteAsync(DiscountToDelete);

            return RedirectToAction(nameof(Discount)); // Chuyển hướng về trang danh sách sau khi xóa
        }

        //public async Task<IActionResult> Search(int term)
        //{
        //    var discounts = await _discountRepository.GetAllAsync();
        //    var discountIds = discounts.Select(x => x.IdDiscount).ToList();
        //    var filteredDiscount = discountIds.Where(p => p == term);
        //    return Json(filteredDiscount);
        //}
    }
}
