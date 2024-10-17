using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Apizr.Tests.Models
{
    public record CreateUser
    {
        [JsonPropertyName("id")] public string Id { get; init; }

        [JsonPropertyName("first_name")] public string FirstName { get; init; }

        [JsonPropertyName("last_name")] public string LastName { get; init; }

        [JsonPropertyName("avatar")] public string Avatar { get; init; }

        [JsonPropertyName("email")] public string Email { get; init; }

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
