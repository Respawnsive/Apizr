using Apizr.Mapping;

namespace Apizr.Tests.Models
{
    [MappedWith<User>]
    public class MinUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
