using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiLogger.Models;

namespace WebApiLogger.Data
{
    public static class DbInitializer
    {
        public static void Initialize(StudentContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            var students = new Student[]
            {
                new Student{FirstName="Denny", LastName="Devito", Age=63},
                new Student{FirstName="Bruce", LastName="Willis", Age=52},
                new Student{FirstName="Antony", LastName="Hopkins", Age=73},
                new Student{FirstName="Brad", LastName="Pit", Age=43},
                new Student{FirstName="John", LastName="Travolta", Age=45},
            };
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();
        }
    }
}
