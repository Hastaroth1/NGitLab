#nullable enable

using System.Text.Json.Serialization;

namespace NGitLab.Models.Webhooks.PipelineEvent;

public class PipelineEventBody
{
    [JsonPropertyName("object_kind")]
    public string? ObjectKind { get; set; }

    [JsonPropertyName("object_attributes")]
    public ObjectAttributes? ObjectAttributes { get; set; }

    [JsonPropertyName("merge_request")]
    public object? MergeRequest { get; set; }

    [JsonPropertyName("user")]
    public User? User { get; set; }

    [JsonPropertyName("project")]
    public Project? ExternalRepository { get; set; }

    [JsonPropertyName("commit")]
    public Commit? Commit { get; set; }
}
