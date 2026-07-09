using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13
{
    public class DateConverter: JsonConverter<DateTime>
    {
        private const string Format = "yyyy.MM.dd";
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            if (value == null)
            {
                throw new JsonException("Дата не может быть null!");
            }
            if (!DateTime.TryParseExact(value, Format, null, System.Globalization.DateTimeStyles.None, out var date))
            {
                throw new JsonException($"Формат даты не верен: {value}, должен быть {Format}");
            }
            return date;
        }
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}

