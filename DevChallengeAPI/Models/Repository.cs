using System.Text.Json.Serialization;

namespace ChallengeAPI.Models
{
    public class Repository
    {
        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("language")]
        public string Language { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        public Owner Owner { get; set; } = new Owner();
    }
}
