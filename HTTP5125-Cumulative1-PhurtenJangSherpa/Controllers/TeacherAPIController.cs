using Microsoft.AspNetCore.Mvc;
using School.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

/// <summary>
/// API Controller for managing Teachers.
/// Provides endpoints to retrieve all teachers or a single teacher by ID.
/// </summary>
namespace School.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        /// <summary>
        /// Constructor injecting the database context.
        /// </summary>
        /// <param name="context">Database context to access school database</param>
        public TeacherController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all teachers from the database.
        /// </summary>
        /// <returns>A list of Teacher objects containing teacher details.</returns>
        /// <example>
        /// GET: api/teacher
        /// Response:
        /// [
        ///   {
        ///     "TeacherId": 1,
        ///     "TeacherFName": "Alexander",
        ///     "TeacherLName": "Bennett",
        ///     "EmployeeNumber": "T378",
        ///     "HireDate": "2016-08-05T00:00:00",
        ///     "Salary": 55.30
        ///   },
        ///   ...
        /// ]
        /// </example>
        [HttpGet]
        public List<Teacher> GetAllTeachers()
        {
            List<Teacher> teachers = new List<Teacher>();

            // Open a connection to the database
            using (MySqlConnection conn = _context.AccessDatabase())
            {
                conn.Open();

                MySqlCommand cmd = conn.CreateCommand();

                // SQL query to select all teachers
                cmd.CommandText = "SELECT * FROM teachers";

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // Read each record and map to Teacher object
                    while (reader.Read())
                    {
                        teachers.Add(new Teacher
                        {
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            TeacherFName = reader["teacherfname"].ToString(),
                            TeacherLName = reader["teacherlname"].ToString(),
                            EmployeeNumber = reader["employeenumber"].ToString(),
                            HireDate = reader["hiredate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["hiredate"]),
                            Salary = reader["salary"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["salary"])
                        });
                    }
                }
            }

            // Return the list of teachers
            return teachers;
        }

        /// <summary>
        /// Retrieves a specific teacher by their ID.
        /// </summary>
        /// <param name="id">The unique ID of the teacher.</param>
        /// <returns>A Teacher object if found; 404 Not Found otherwise.</returns>
        /// <example>
        /// GET: api/teacher/3
        /// Response:
        /// {
        ///   "TeacherId": 3,
        ///   "TeacherFName": "Linda",
        ///   "TeacherLName": "Chan",
        ///   "EmployeeNumber": "T382",
        ///   "HireDate": "2015-08-22T00:00:00",
        ///   "Salary": 60.22
        /// }
        /// </example>
        [HttpGet("{id}")]
        public ActionResult<Teacher> GetTeacherById(int id)
        {
            Teacher teacher = null;

            using (MySqlConnection conn = _context.AccessDatabase())
            {
                conn.Open();

                MySqlCommand cmd = conn.CreateCommand();

                // Parameterized query to prevent SQL injection
                cmd.CommandText = "SELECT * FROM teachers WHERE teacherid = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // If teacher found, map to object
                    if (reader.Read())
                    {
                        teacher = new Teacher
                        {
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            TeacherFName = reader["teacherfname"].ToString(),
                            TeacherLName = reader["teacherlname"].ToString(),
                            EmployeeNumber = reader["employeenumber"].ToString(),
                            HireDate = reader["hiredate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["hiredate"]),
                            Salary = reader["salary"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["salary"])
                        };
                    }
                }
            }

            if (teacher == null)
                return NotFound($"Teacher with ID {id} not found.");

            return teacher;
        }
    }
}
