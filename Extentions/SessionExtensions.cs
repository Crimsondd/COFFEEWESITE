using System.Text.Json;

namespace DACS_DAMH.Extentions
{
    public static class SessionExtensions
    {
        //hàm lấy thông tin giỏ hàng và lưu vào trong session
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        //lấy thông tin từ session và hiển thị vào giỏ hàng
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

    }
}
