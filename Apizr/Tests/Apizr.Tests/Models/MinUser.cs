using Apizr.Mapping;

namespace Apizr.Tests.Models
{
    [MappedWith<User>]
    public record MinUser
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}
