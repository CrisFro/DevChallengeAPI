using System.Text.Json.Serialization;

namespace ChallengeAPI.Models
{
    public class Owner
    {
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
