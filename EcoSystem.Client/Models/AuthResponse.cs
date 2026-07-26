using System.Text.Json.Serialization;

namespace EcoSystem.Client.Models;

public class AuthResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("expiration")]
    public DateTime Expiration { get; set; }
}