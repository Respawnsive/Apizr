using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Mapping;
using Apizr.Sample.Models;

namespace Apizr.Sample.Console.Models
{
    [MappedWith<User>]
    public class MinUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
