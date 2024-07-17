using System;
using Apizr.Configuring;
using Apizr.Mapping;
using Apizr.Requesting;

namespace Apizr.Tests.Models
{
    [AutoRegister("https://reqres.in/api/users")] // this one is for crud auto registration with mapping example
    [MappedWith<UserDetails>] // this one is for classic auto registration with mapping example
    public record UserInfos
    {
        public int Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Avatar { get; init; }

        public string Email { get; init; }

        public string Company { get; init; }

        public Uri Url { get; init; }

        public string Text { get; init; }
    }
}
