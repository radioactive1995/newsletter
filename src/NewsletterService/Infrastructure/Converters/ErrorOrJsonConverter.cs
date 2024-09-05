using Application.Articles;
using ErrorOr;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Converters;


public class ErrorOrJsonConverter<TValue> : JsonConverter<ErrorOr<TValue>>
{
    public override ErrorOr<TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (var doc = JsonDocument.ParseValue(ref reader))
        {
            // Handle the case where errors are present
            if (doc.RootElement.TryGetProperty("Errors", out var errorsElement) && errorsElement.ValueKind != JsonValueKind.Null)
            {
                var errors = JsonSerializer.Deserialize<List<Error>>(errorsElement.GetRawText(), options);
                return ErrorOr<TValue>.From(errors); // Return the error case
            }

            // Handle the success case where value is present
            if (doc.RootElement.TryGetProperty("Value", out var valueElement) && valueElement.ValueKind != JsonValueKind.Null)
            {
                var value = JsonSerializer.Deserialize<TValue>(valueElement.GetRawText(), options);
                return value; // Return the value directly for success case
            }

            throw new JsonException("Unable to deserialize ErrorOr: Neither 'Errors' nor 'Value' was present.");
        }
    }

    public override void Write(Utf8JsonWriter writer, ErrorOr<TValue> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // If it's an error, serialize the Errors
        if (value.IsError)
        {
            writer.WritePropertyName("Errors");
            JsonSerializer.Serialize(writer, value.Errors, options);
        }
        else
        {
            // Otherwise, serialize the Value for the success case
            writer.WritePropertyName("Value");
            JsonSerializer.Serialize(writer, value.Value, options);
        }

        writer.WriteEndObject();
    }
}

