using Apizr.Mapping;

namespace Apizr.Tests.Models
{
    [MappedWith(typeof(User))]
    public class MinUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
