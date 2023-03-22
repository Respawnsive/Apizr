using Innofactor.EfCoreJsonValueConverter;
using TCDev.APIGenerator.Attributes;
using TCDev.APIGenerator.Interfaces;

namespace Apizr.Sample.Api.Models;

[Api("/courses")]
public class Course : IObjectBase<int>
{
    public int Id { get; set; }
    public List<Student> Students { get; set; }
    public Teacher Teacher { get; set; }

    [JsonField]
    public List<DateTime> Schedule { get; set; }
}