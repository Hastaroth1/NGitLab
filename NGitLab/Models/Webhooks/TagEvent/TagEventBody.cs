#nullable enable

using System.Text.Json.Serialization;
using NGitLab.Impl.Json;

namespace NGitLab.Models.Webhooks.TagEvent;

/// <summary>
/// The event body when Gitlab sends a Tag event in a webhook POST
/// </summary>
public class TagEventBody
{
    [JsonPropertyName("object_kind")]
    public string? ObjectKind { get; set; }

    [JsonPropertyName("event_name")]
    public string? EventName { get; set; }

    [JsonPropertyName("before")]
    public Sha1? Before { get; set; }

    [JsonPropertyName("after")]
    public Sha1? After { get; set; }

    [JsonPropertyName("ref")]
    public string? Ref { get; set; }

    [JsonPropertyName("checkout_sha")]
    public Sha1? CheckoutSha { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }

    [JsonPropertyName("user_username")]
    public string? UserUsername { get; set; }

    [JsonPropertyName("user_email")]
    public string? UserEmail { get; set; }

    [JsonPropertyName("user_avatar")]
    public string? UserAvatar { get; set; }

    [JsonPropertyName("project_id")]
    public int ProjectId { get; set; }

    [JsonPropertyName("project")]
    public Project? Project { get; set; }

    [JsonPropertyName("commits")]
    public List<TagCommit>? Commits { get; set; }

    [JsonPropertyName("total_commits_count")]
    public int TotalCommitsCount { get; set; }

    /// <summary>
    /// Convert the string to a <see cref="TagEventBody"/>
    /// </summary>
    /// <param name="json">The string to deserialize</param>
    /// <returns></returns>
    public static TagEventBody Deserialize(string json)
    {
        // Since the Sha1Converter is internal, this is the only way to be able to deserialize this object
        return Serializer.Deserialize<TagEventBody>(json);
    }
}
