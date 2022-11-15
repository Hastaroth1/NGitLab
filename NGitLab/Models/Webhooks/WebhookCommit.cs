#nullable enable

using System.Text.Json.Serialization;

namespace NGitLab.Models.Webhooks;

/// <summary>
/// Commit data sent in Webhook POST
/// This is different from <see cref="Commit"/> since the author here is a nested object
/// </summary>
public class WebhookCommit : CommitBase
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("author")]
    public Author? Author { get; set; }
}
