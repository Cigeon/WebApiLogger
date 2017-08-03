using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiLogger.Data;
using WebApiLogger.Models;
using WebApiLogger.Serivces;
using Microsoft.Extensions.Logging;
using WebApiLogger.Loggers;

namespace WebApiLogger.Controllers
{
    [Produces("application/json")]
    [Route("api/Students")]
    public class StudentsController : Controller
    {
        private readonly AcademyService _academy;
        private readonly IStudentsService _recruiter;
        private readonly ILogger _logger;


        public StudentsController(  AcademyService academy, 
                                    IStudentsService recruiter, 
                                    ILogger<StudentsController> logger  )
        {
            _academy = academy;
            _recruiter = recruiter;
            _logger = logger;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<IEnumerable<Student>> GetStudents()
        {
            var students = await _academy.GetStudentsAsync();
            _logger.LogInformation("Get list of students");
            foreach (var student in students)
            {
                _logger.LogInformation("Id={0}, FirstName={1}, LastName={2}, Age={3}",
                                        student.Id, student.FirstName, student.LastName, student.Age);
            }
            return students;
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Bad request, model is not valid!");
                return BadRequest(ModelState);
            }

            var student = await _recruiter.GetStudentByIdAsync(id);

            if (student == null)
            {
                _logger.LogWarning("Bad request, student with id: {0}, not found!", id);
                return NotFound();
            }

            _logger.LogInformation("Get student, Id={0}, FirstName={1}, LastName={2}, Age={3}",
                                    student.Id, student.FirstName, student.LastName, student.Age);
            return Ok(student);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] int id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Bad request, model is not valid!");
                return BadRequest(ModelState);
            }

            if (id != student.Id)
            {
                _logger.LogWarning("Bad request, ids not equal! Request id={0} and student id={1}", id, student.Id);
                return BadRequest();
            }

            try
            {
                await _recruiter.UpdateStudentAsync(student);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    _logger.LogWarning("Bad request, student with id: {0}, not found!", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _logger.LogInformation("Update student, Id={0}, FirstName={1}, LastName={2}, Age={3}",
                                    student.Id, student.FirstName, student.LastName, student.Age);
            return NoContent();
        }

        // POST: api/Students
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Bad request, model is not valid!");
                return BadRequest(ModelState);
            }

            await _recruiter.AddStudentAsync(student);
            _logger.LogInformation("Add new student, FirstName={0}, LastName={1}, Age={2}", 
                                    student.FirstName, student.LastName, student.Age);
            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Bad request, model is not valid!");
                return BadRequest(ModelState);
            }

            var student = await _recruiter.GetStudentByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Bad request, student with id: {0}, not found!", id);
                return NotFound();
            }

            await _recruiter.DeleteStudentAsync(student);
            _logger.LogInformation("Delete student, Id={0}, FirstName={1}, LastName={2}, Age={3}",
                                    student.Id, student.FirstName, student.LastName, student.Age);
            return Ok(student);
        }

        private bool StudentExists(int id)
        {
            return _recruiter.GetStudentsAsync().Result.Any(e => e.Id == id);
        }
    }
}