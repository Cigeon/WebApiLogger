using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiLogger.Data;
using WebApiLogger.Models;
using WebApiLogger.Serivces;
using Microsoft.Extensions.Logging;

namespace WebApiLogger.Controllers
{
    [Produces("application/json")]
    [Route("api/Students")]
    public class StudentsController : Controller
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly AcademyService _academy;
        private readonly IStudentsService _recruiter;

        public StudentsController(ILoggerFactory loggerFactory, AcademyService academy, IStudentsService recruiter)
        {
            _loggerFactory = loggerFactory;
            _academy = academy;
            _recruiter = recruiter;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _academy.GetStudentsAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _recruiter.GetStudentByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] int id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.Id)
            {
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _recruiter.AddStudentAsync(student);

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _recruiter.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            await _recruiter.DeleteStudentAsync(student);

            return Ok(student);
        }

        private bool StudentExists(int id)
        {
            return _recruiter.GetStudentsAsync().Result.Any(e => e.Id == id);
        }
    }
}