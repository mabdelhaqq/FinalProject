using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Controllers.MVCControllers
{
    //[Authorize]
    public class StudentSubjectsController : Controller
    {
        private readonly FinalProjectContext _context;

        public StudentSubjectsController(FinalProjectContext context)
        {
            _context = context;
        }

        // GET: StudentSubjects
        public async Task<IActionResult> Index()
        {
            var finalProjectContext = _context.StudentSubject.Include(s => s.Student).Include(s => s.Subject);
            return View(await finalProjectContext.ToListAsync());
        }

        // GET: StudentSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentSubject = await _context.StudentSubject
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentSubject == null)
            {
                return NotFound();
            }

            return View(studentSubject);
        }

        // GET: StudentSubjects/Create
        public IActionResult Create()
        {
            var students = _context.Student.ToList();
            if (students != null && students.Any())
            {
                var studentList = students.Select(s => new { Id = s.Id, FullName = $"{s.FirstName} {s.LastName}" }).ToList();
                ViewData["StudentId"] = new SelectList(studentList, "Id", "FullName");
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Name");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,SubjectId")] StudentSubject studentSubject)
        {
            var existingRecord = await _context.StudentSubject.FirstOrDefaultAsync(ss => ss.StudentId == studentSubject.StudentId && ss.SubjectId == studentSubject.SubjectId);

            if (existingRecord != null)
            {
                ModelState.AddModelError(string.Empty, "This student is already registered for this subject.");
            }

            if (ModelState.IsValid)
            {
                if (existingRecord == null)
                {
                    _context.Add(studentSubject);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            var students = _context.Student.ToList();
            if (students != null && students.Any())
            {
                var studentList = students.Select(s => new { Id = s.Id, FullName = $"{s.FirstName} {s.LastName}" }).ToList();
                ViewData["StudentId"] = new SelectList(studentList, "Id", "FullName", studentSubject.StudentId);
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Name", studentSubject.SubjectId);
            return View(studentSubject);
        }


        // GET: StudentSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentSubject = await _context.StudentSubject.FindAsync(id);
            if (studentSubject == null)
            {
                return NotFound();
            }

            var students = _context.Student.ToList();
            if (students != null && students.Any())
            {
                var studentList = students.Select(s => new { Id = s.Id, FullName = $"{s.FirstName} {s.LastName}" }).ToList();
                ViewData["StudentId"] = new SelectList(studentList, "Id", "FullName", studentSubject.StudentId);
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Name", studentSubject.SubjectId);
            return View(studentSubject);
        }

        // POST: StudentSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,SubjectId")] StudentSubject studentSubject)
        {
            if (id != studentSubject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentSubjectExists(studentSubject.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var students = _context.Student.ToList();
            if (students != null && students.Any())
            {
                var studentList = students.Select(s => new { Id = s.Id, FullName = $"{s.FirstName} {s.LastName}" }).ToList();
                ViewData["StudentId"] = new SelectList(studentList, "Id", "FullName", studentSubject.StudentId);
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Name", studentSubject.SubjectId);
            return View(studentSubject);
        }


        // GET: StudentSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentSubject = await _context.StudentSubject
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentSubject == null)
            {
                return NotFound();
            }

            return View(studentSubject);
        }

        // POST: StudentSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentSubject = await _context.StudentSubject.FindAsync(id);
            if (studentSubject != null)
            {
                _context.StudentSubject.Remove(studentSubject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentSubjectExists(int id)
        {
            return _context.StudentSubject.Any(e => e.Id == id);
        }
    }
}
