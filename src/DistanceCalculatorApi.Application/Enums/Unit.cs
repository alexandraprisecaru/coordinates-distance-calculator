namespace DistanceCalculatorApi.Application.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Unit : byte
{
    Metric = 1,
    Imperial = 2
}