using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityService.src.Web.Common.Helpers
{
    public class VietnamDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetDateTime().ToUniversalTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Chuyển từ UTC sang giờ Việt Nam (GMT+7)
            var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(value,
                TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            writer.WriteStringValue(vietnamTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
