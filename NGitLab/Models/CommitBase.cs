using System.Text.Json.Serialization;

namespace NGitLab.Models;

public abstract class CommitBase
{
    [JsonPropertyName("id")]
    public Sha1 Id;

    [JsonPropertyName("title")]
    public string Title;

    [JsonPropertyName("message")]
    public string Message;
}
