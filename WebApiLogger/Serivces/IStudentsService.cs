using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiLogger.Models;

namespace WebApiLogger.Serivces
{
    public interface IStudentsService
    {
        Task<IEnumerable<Student>> GetStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(Student student);
    }
}
