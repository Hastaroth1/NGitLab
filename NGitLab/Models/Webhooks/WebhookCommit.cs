#nullable enable

using System.Text.Json.Serialization;

namespace NGitLab.Models.Webhooks;

public class WebhookCommit : CommitBase
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("author")]
    public Author? Author { get; set; }
}
