using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Mapping;
using Apizr.Sample.Api.Models;

namespace Apizr.Sample.Console.Models
{
    [MappedCrudEntity("https://reqres.in/api/users", typeof(UserDetails))]
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
