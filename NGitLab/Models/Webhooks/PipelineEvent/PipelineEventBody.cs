#nullable enable

using System.Text.Json.Serialization;
using NGitLab.Impl.Json;

namespace NGitLab.Models.Webhooks.PipelineEvent;

/// <summary>
/// The event body when Gitlab sends a Pipeline event in a webhook POST
/// </summary>
public class PipelineEventBody
{
    [JsonPropertyName("object_kind")]
    public string? ObjectKind { get; set; }

    [JsonPropertyName("object_attributes")]
    public ObjectAttributes? ObjectAttributes { get; set; }

    [JsonPropertyName("user")]
    public User? User { get; set; }

    [JsonPropertyName("project")]
    public Project? ExternalRepository { get; set; }

    [JsonPropertyName("commit")]
    public WebhookCommit? Commit { get; set; }

    /// <summary>
    /// Convert the string to a <see cref="PipelineEventBody"/>
    /// </summary>
    /// <param name="json">The string to deserialize</param>
    /// <returns></returns>
    public static PipelineEventBody Deserialize(string json)
    {
        // Since the Sha1Converter is internal, this is the only way to be able to deserialize this object
        return Serializer.Deserialize<PipelineEventBody>(json);
    }
}
