using TCDev.ApiGenerator.Attributes;
using TCDev.ApiGenerator.Interfaces;

namespace Apizr.Sample.Api.Models;

[Api("/teachers")]
public class Teacher : IObjectBase<int>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }

    public void BeforeCreate(Teacher newTeacher)
    {
        // Before Create hook to make custom validations
    }
}