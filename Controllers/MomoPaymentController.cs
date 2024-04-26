using DACS_DAMH.Extentions;
using DACS_DAMH.Models;
using DACS_DAMH.Paymodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DACS_DAMH.Controllers
{
    public class MomoPaymentController : Controller
    {
        public IActionResult Payment(decimal totalMOMO)
        {
            try
            {
                // Các thông tin cần thiết cho yêu cầu thanh toán
                string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                string partnerCode = "MOMOOJOI20210710";
                string accessKey = "iPXneGmrJH0G8FOP";
                string secretKey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
                string orderInfo = "Thanh toán online";
                string returnUrl = "http://localhost:54444/ShoppingCart/Checkout";
                string notifyUrl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment";
                string extraData = "";

                // Mã đơn hàng và request ID
                string orderId = DateTime.Now.Ticks.ToString();
                string requestId = DateTime.Now.Ticks.ToString();

                // Chuẩn bị chuỗi rawHash và chữ ký signature
                string amount = totalMOMO.ToString();
                string rawHash = $"partnerCode={partnerCode}&accessKey={accessKey}&requestId={requestId}&amount={amount}&orderId={orderId}&orderInfo={orderInfo}&returnUrl={returnUrl}&notifyUrl={notifyUrl}&extraData={extraData}";
                MoMoSecurity crypto = new MoMoSecurity();
                string signature = crypto.signSHA256(rawHash, secretKey);

                // Xây dựng body JSON request
                JObject message = new JObject
                {
                    { "partnerCode", partnerCode },
                    { "accessKey", accessKey },
                    { "requestId", requestId },
                    { "amount", amount },
                    { "orderId", orderId },
                    { "orderInfo", orderInfo },
                    { "returnUrl", returnUrl },
                    { "notifyUrl", notifyUrl },
                    { "extraData", extraData },
                    { "requestType", "captureMoMoWallet" },
                    { "signature", signature }
                };
                // Clear gio hang
                //var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                //cart.ClearItems();
                //HttpContext.Session.SetObjectAsJson("Cart", cart);

                // Gửi yêu cầu thanh toán đến MoMo và nhận URL thanh toán
                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                JObject jmessage = JObject.Parse(responseFromMomo);

                // Chuyển hướng người dùng đến URL thanh toán từ MoMo
                string payUrl = jmessage.GetValue("payUrl").ToString();
                return Redirect(payUrl);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest(ex.Message);
            }
        }

        //private string CalculateHMACSHA256Hash(string input, string key)
        //{
        //    using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        //    {
        //        byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        //        return Convert.ToBase64String(hashValue);
        //    }
        //}
    }
}
