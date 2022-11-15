#nullable enable

using System.Text.Json.Serialization;

namespace NGitLab.Models.Webhooks.TagEvent;

public class TagCommit : WebhookCommit
{
    [JsonPropertyName("added")]
    public List<string>? Added { get; set; }

    [JsonPropertyName("modified")]
    public List<string>? Modified { get; set; }

    [JsonPropertyName("removed")]
    public List<string>? Removed { get; set; }
}
