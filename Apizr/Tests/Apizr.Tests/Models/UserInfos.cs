using System;
using Apizr.Mapping;

namespace Apizr.Tests.Models
{
    [MappedCrudEntity("https://reqres.in/api/users", typeof(UserDetails))] // this one is for crud auto registration with mapping example
    [MappedWith(typeof(UserDetails))] // this one is for classic auto registration with mapping example
    public class UserInfos
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public string Company { get; set; }

        public Uri Url { get; set; }

        public string Text { get; set; }
    }
}
