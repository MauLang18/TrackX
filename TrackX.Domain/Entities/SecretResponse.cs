using Newtonsoft.Json;

namespace TrackX.Domain.Entities
{
    public class SecretResponse<T> where T : class
    {
        [JsonProperty("request_id")]
        public string? RequestId { get; set; }

        [JsonProperty("lease_id")]
        public string? LeaseId { get; set; }

        [JsonProperty("renewable")]
        public bool Renewable { get; set; }

        [JsonProperty("lease_duration")]
        public int LeaseDuration { get; set; }

        [JsonProperty("data")]
        public SecretDataWrapper<T>? Data { get; set; }

        [JsonProperty("wrap_info")]
        public object? WrapInfo { get; set; }

        [JsonProperty("warnings")]
        public object? Warnings { get; set; }

        [JsonProperty("auth")]
        public object? Auth { get; set; }
    }

    public class SecretDataWrapper<T> where T : class
    {
        [JsonProperty("data")]
        public T? Data { get; set; }

        [JsonProperty("metadata")]
        public Metadata? Metadata { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("created_time")]
        public DateTime? CreatedTime { get; set; }

        [JsonProperty("custom_metadata")]
        public object? CustomMetadata { get; set; }

        [JsonProperty("deletion_time")]
        public string? DeletionTime { get; set; }

        [JsonProperty("destroyed")]
        public bool Destroyed { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }
}
