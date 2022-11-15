#nullable enable

using System.Text.Json.Serialization;

namespace NGitLab.Models.Webhooks.TagEvent;

public class TagEventBody
{
    [JsonPropertyName("object_kind")]
    public string? ObjectKind { get; set; }

    [JsonPropertyName("event_name")]
    public string? EventName { get; set; }

    [JsonPropertyName("before")]
    public string? Before { get; set; }

    [JsonPropertyName("after")]
    public string? After { get; set; }

    [JsonPropertyName("ref")]
    public string? Ref { get; set; }

    [JsonPropertyName("checkout_sha")]
    public string? CheckoutSha { get; set; }

    [JsonPropertyName("message")]
    public object? Message { get; set; }

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
}
