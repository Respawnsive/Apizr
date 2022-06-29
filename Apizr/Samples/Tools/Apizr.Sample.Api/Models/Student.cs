using TCDev.APIGenerator.Attributes;
using TCDev.APIGenerator.Interfaces;

namespace Apizr.Sample.Api.Models
{
    [Api("/students")]
    public class Student : IObjectBase<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public void BeforeDelete(Student student)
        {
            // Before Delete hook to make custom validations
        }

    }
}
