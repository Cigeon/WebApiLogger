using Microsoft.EntityFrameworkCore;
using WebApiLogger.Models;

namespace WebApiLogger.Data
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

    }
}
