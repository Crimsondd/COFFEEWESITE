using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DACS_DAMH.Models
{
    public class ApplicationUser : IdentityUser
    {
        //ApplicationUser= IdentityUser + thuộc tính riêng
        [Required]
        public string FullName { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh d/m/y")]
        [ModelBinder(BinderType = typeof(DayMonthYearBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Birthday { set; get; }

    }
}
