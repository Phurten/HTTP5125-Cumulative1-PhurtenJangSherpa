using Microsoft.AspNetCore.Mvc;
using School.Models;
using System.Collections.Generic;

/// <summary>
/// MVC Controller to handle web pages for Teachers.
/// Retrieves data via the Teacher API controller and returns views.
/// </summary>
namespace School.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly TeacherController _api;

        /// <summary>
        /// Constructor injecting the Teacher API controller dependency.
        /// </summary>
        /// <param name="api">Teacher API controller for data access</param>
        public TeacherPageController(TeacherController api)
        {
            _api = api;
        }

        /// <summary>
        /// Displays a list of all teachers on the List view.
        /// </summary>
        /// <returns>View with a list of Teacher objects.</returns>
        public IActionResult List()
        {
            List<Teacher> teachers = _api.GetAllTeachers();
            return View(teachers);
        }

        /// <summary>
        /// Displays detailed information for a single teacher on the Show view.
        /// </summary>
        /// <param name="id">Teacher ID to display.</param>
        /// <returns>View with the selected Teacher object.</returns>
        public IActionResult Show(int id)
        {
            var teacherResult = _api.GetTeacherById(id);
            if (teacherResult.Result is NotFoundObjectResult)
                return View(null); // Pass null to the view if not found

            var teacher = teacherResult.Value;

            return View(teacher);
        }

        /// <summary>
        /// Shows the form to add a new teacher.
        /// </summary>
        public IActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Handles the POST request to create a new teacher.
        /// </summary>
        [HttpPost]
        public IActionResult Create(Teacher teacher)
        {
            if (!ModelState.IsValid)
                return View("New", teacher);

            var result = _api.AddTeacher(teacher);
            if (result.Result is CreatedAtActionResult created)
            {
                var newTeacher = (Teacher)created.Value;
                return RedirectToAction("Show", new { id = newTeacher.TeacherId });
            }
            return View("New", teacher);
        }

        /// <summary>
        /// Shows the form to edit an existing teacher.
        /// </summary>
        /// <param name="id">Teacher ID to edit.</param>
        /// <returns>Edit view with the selected Teacher object.</returns>
        public IActionResult Edit(int id)
        {
            var teacherResult = _api.GetTeacherById(id);
            if (teacherResult.Result is NotFoundObjectResult)
                return NotFound($"Teacher with ID {id} does not exist.");

            var teacher = teacherResult.Value;
            return View(teacher);
        }

        /// <summary>
        /// Shows the delete confirmation page.
        /// </summary>
        public IActionResult DeleteConfirm(int id)
        {
            var teacher = _api.GetTeacherById(id);
            if (teacher.Result is NotFoundObjectResult)
            {
                return NotFound($"Teacher with ID {id} does not exist.");
            }
            return View(teacher.Value);
        }

        /// <summary>
        /// Handles the POST request to delete a teacher.
        /// </summary>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _api.DeleteTeacher(id);
            if (result is NotFoundObjectResult)
            {
                return NotFound($"Teacher with ID {id} does not exist.");
            }
            return RedirectToAction("List");
        }

        /// <summary>
        /// Searches for teachers by hire date range and displays the results on the List view.
        /// </summary>
        /// <param name="min">Minimum hire date.</param>
        /// <param name="max">Maximum hire date.</param>
        /// <returns>View with a list of Teacher objects that match the search criteria.</returns>
        public IActionResult SearchByHireDate(DateTime min, DateTime max)
        {
            var result = _api.GetTeachersByHireDateRange(min, max);
            return View("List", result.Value);
        }
    }
}
