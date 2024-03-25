using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Controllers.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, User")]
    public class StudentSubjectsController : ControllerBase
    {
        private readonly FinalProjectContext _context;

        public StudentSubjectsController(FinalProjectContext context)
        {
            _context = context;
        }

        // GET: api/StudentSubjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentSubject>>> GetStudentSubject()
        {
            return await _context.StudentSubject.ToListAsync();
        }

        // GET: api/StudentSubjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentSubject>> GetStudentSubject(int id)
        {
            var studentSubject = await _context.StudentSubject.FindAsync(id);

            if (studentSubject == null)
            {
                return NotFound();
            }

            return studentSubject;
        }

        // PUT: api/StudentSubjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentSubject(int id, StudentSubject studentSubject)
        {
            if (id != studentSubject.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentSubject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentSubjectExists(id))
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

        // POST: api/StudentSubjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentSubject>> PostStudentSubject(StudentSubject studentSubject)
        {
            _context.StudentSubject.Add(studentSubject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentSubject", new { id = studentSubject.Id }, studentSubject);
        }

        // DELETE: api/StudentSubjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentSubject(int id)
        {
            var studentSubject = await _context.StudentSubject.FindAsync(id);
            if (studentSubject == null)
            {
                return NotFound();
            }

            _context.StudentSubject.Remove(studentSubject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentSubjectExists(int id)
        {
            return _context.StudentSubject.Any(e => e.Id == id);
        }
    }
}
