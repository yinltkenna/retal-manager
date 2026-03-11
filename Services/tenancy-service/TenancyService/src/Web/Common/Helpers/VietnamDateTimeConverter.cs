using System.Text.Json;
using System.Text.Json.Serialization;

namespace TenancyService.src.Web.Common.Helpers
{
    public class VietnamDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            return reader.GetDateTime().ToUniversalTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (!value.HasValue)
            {
                writer.WriteNullValue();
                return;
            }

            TimeZoneInfo vietnamZone;
            try
            {
                vietnamZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Windows
            }
            catch
            {
                vietnamZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh"); // Linux/Docker
            }

            var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(value.Value, vietnamZone);
            writer.WriteStringValue(vietnamTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
