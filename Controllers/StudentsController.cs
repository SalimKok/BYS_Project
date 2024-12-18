﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace SmartCourseSelectorWeb.Controllers
{
    [Route("api/StudentController")]
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("SelectCourses")]
        public IActionResult SelectCourses()
        {
            // Tüm dersleri getiriyoruz
            var courses = _context.Courses.ToList();
            return View(courses);
        }
        [HttpPost("SubmitSelectedCourses")]
        public async Task<IActionResult> SubmitSelectedCourses(int id, List<int> selectedCourses)
        {
            
            var student = await _context.Students
                                         .Include(s => s.StudentCourseSelections)
                                             .ThenInclude(sc => sc.Course)
                                         .Include(s => s.Advisor)
                                         .FirstOrDefaultAsync(s => s.StudentID == id);
            // Yeni seçilen dersleri ekleyelim
            foreach (var courseId in selectedCourses)
            {
                var courseSelection = new StudentCourseSelection
                {
                    StudentID = id,
                    CourseID = courseId
                };

                _context.StudentCourseSelections.Add(courseSelection);
            }

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Selection completed successfully!";

            // Ana sayfaya yönlendirme
            return NoContent();
        }

        [HttpGet("CourseSelection")]
        public async Task<IActionResult> CourseSelection(int id)
        {
           
           
            var student = await _context.Students
                                         .Include(s => s.StudentCourseSelections)
                                             .ThenInclude(sc => sc.Course)
                                         .Include(s => s.Advisor) 
                                         .FirstOrDefaultAsync(s => s.StudentID == id);


            // Eğer öğrenci bulunamazsa hata mesajı gönderin
            if (student == null)
            {
                ViewBag.Message = "Student not found.";
                return View(); // Boş bir View döner
            }

            // Öğrenci modelini View'a gönderin
            return View(student); // Student modelini gönderiyoruz
        }
        


        // GET: api/Students
        [HttpGet("getStudentList")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("getById")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound(new { Message = "Student not found." });
            }

            return student;
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest(new { Message = "Student ID mismatch." });
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound(new { Message = "Student not found." });
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
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentID }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { Message = "Student not found." });
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
        // GET: api/StudentController/getByAdvisorId
        [HttpGet("getByAdvisorId")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByAdvisorId(int advisorId)
        {
            // AdvisorID'ye göre filtrelenmiş öğrencileri al
            var students = await _context.Students
                                          .Where(s => s.AdvisorID == advisorId) // AdvisorID ile filtreleme
                                          .ToListAsync();

            // Eğer öğrenciler bulunamadıysa NotFound döner
            if (!students.Any())
            {
                return NotFound(new { Message = $"No students found for AdvisorID {advisorId}." });
            }

            // Öğrenci listesi başarılı şekilde döner
            return Ok(students);
        }

    }
}