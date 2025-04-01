using Questao5.Domain.Enumerators;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Questao5.Domain.Language
{
    public class TipoMovimentoEnumConverter : JsonConverter<TipoMovimentoEnum>
    {
        public override TipoMovimentoEnum Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            
            var value = reader.GetString()?.ToUpperInvariant();

            return value switch
            {
                "C" => TipoMovimentoEnum.C,
                "D" => TipoMovimentoEnum.D,
                _ => throw new JsonException($"Valor inválido para TipoMovimentoEnum: '{value}'")
            };
        }

        public override void Write(
            Utf8JsonWriter writer,
            TipoMovimentoEnum value,
            JsonSerializerOptions options)
        {

            var stringValue = value == TipoMovimentoEnum.C ? "C" : "D";
            writer.WriteStringValue(stringValue);
        }

    }
}
