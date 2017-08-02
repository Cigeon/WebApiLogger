using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiLogger.Data;
using WebApiLogger.Models;

namespace WebApiLogger.Serivces
{
    public class AcademyService
    {
        private readonly StudentContext _context;

        public AcademyService(StudentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }
    }
}
