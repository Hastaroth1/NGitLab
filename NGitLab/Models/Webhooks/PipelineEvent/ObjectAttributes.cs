#nullable enable

using System.Text.Json.Serialization;

namespace NGitLab.Models.Webhooks.PipelineEvent;

public class ObjectAttributes
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("ref")]
    public string? Ref { get; set; }

    [JsonPropertyName("tag")]
    public bool Tag { get; set; }

    [JsonPropertyName("sha")]
    public Sha1? Sha { get; set; }

    [JsonPropertyName("before_sha")]
    public Sha1? BeforeSha { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("detailed_status")]
    public string? DetailedStatus { get; set; }

    [JsonPropertyName("stages")]
    public List<string>? Stages { get; set; }

    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("finished_at")]
    public string? FinishedAt { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("queued_duration")]
    public int? QueuedDuration { get; set; }
}
