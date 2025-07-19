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
            var teacher = _api.GetTeacherById(id);
            if (teacher.Result is NotFoundObjectResult)
            {
                return NotFound($"Teacher with ID {id} does not exist.");
            }
            return View(teacher.Value);
        }
    }
}
