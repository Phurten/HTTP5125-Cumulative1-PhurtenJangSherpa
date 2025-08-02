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
                cmd.CommandText = "SELECT * FROM teachers WHERE teacherid = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        teacher = new Teacher
                        {
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            TeacherFName = reader["teacherfname"].ToString(),
                            TeacherLName = reader["teacherlname"].ToString(),
                            EmployeeNumber = reader["employeenumber"].ToString(),
                            HireDate = reader["hiredate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["hiredate"]),
                            Salary = reader["salary"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["salary"]),
                            TeacherWorkPhone = reader["teacherworkphone"]?.ToString()
                        };
                    }
                }
            }

            if (teacher == null)
                return NotFound();

            return teacher;
        }

        /// <summary>
        /// Adds a new teacher to the database.
        /// </summary>
        /// <param name="teacher">The Teacher object to add.</param>
        /// <returns>The created Teacher object with its new ID.</returns>
        /// <example>
        /// POST: api/teacher
        /// Request Body:
        /// {
        ///   "TeacherFName": "John",
        ///   "TeacherLName": "Doe",
        ///   "EmployeeNumber": "T400",
        ///   "HireDate": "2022-09-01T00:00:00",
        ///   "Salary": 50.00
        /// }
        /// Response:
        /// {
        ///   "TeacherId": 10,
        ///   "TeacherFName": "John",
        ///   "TeacherLName": "Doe",
        ///   "EmployeeNumber": "T400",
        ///   "HireDate": "2022-09-01T00:00:00",
        ///   "Salary": 50.00
        /// }
        /// </example>
        [HttpPost]
        public ActionResult<Teacher> AddTeacher([FromBody] Teacher teacher)
        {
            if (teacher == null || string.IsNullOrEmpty(teacher.TeacherFName) || string.IsNullOrEmpty(teacher.TeacherLName) || string.IsNullOrEmpty(teacher.EmployeeNumber))
                return BadRequest("Missing required teacher information.");

            using (MySqlConnection conn = _context.AccessDatabase())
            {
                conn.Open();

                // Check for duplicate EmployeeNumber
                MySqlCommand checkCmd = conn.CreateCommand();
                checkCmd.CommandText = "SELECT COUNT(*) FROM teachers WHERE employeenumber = @empnum";
                checkCmd.Parameters.AddWithValue("@empnum", teacher.EmployeeNumber);
                var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                    return BadRequest("Employee number already exists.");

                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO teachers (teacherfname, teacherlname, employeenumber, hiredate, salary, teacherworkphone)
                            VALUES (@fname, @lname, @empnum, @hiredate, @salary, @workphone)";
                cmd.Parameters.AddWithValue("@fname", teacher.TeacherFName);
                cmd.Parameters.AddWithValue("@lname", teacher.TeacherLName);
                cmd.Parameters.AddWithValue("@empnum", teacher.EmployeeNumber);
                cmd.Parameters.AddWithValue("@hiredate", teacher.HireDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@salary", teacher.Salary ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@workphone", teacher.TeacherWorkPhone ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
                teacher.TeacherId = (int)cmd.LastInsertedId;
            }

            return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.TeacherId }, teacher);
        }

        /// <summary>
        /// Updates an existing teacher in the database.
        /// </summary>
        /// <param name="id">The ID of the teacher to update.</param>
        /// <param name="teacher">The updated Teacher object.</param>
        /// <returns>No content if successful; 404 if not found.</returns>
        /// <example>
        /// PUT: api/teacher/3
        /// Request Body:
        /// {
        ///   "TeacherId": 3,
        ///   "TeacherFName": "Linda",
        ///   "TeacherLName": "Chan",
        ///   "EmployeeNumber": "T382",
        ///   "HireDate": "2015-08-22T00:00:00",
        ///   "Salary": 62.00
        /// }
        /// Response: 204 No Content
        /// </example>
        [HttpPut("{id}")]
        public IActionResult UpdateTeacher(int id, [FromBody] Teacher teacher)
        {
            if (teacher == null || id != teacher.TeacherId)
                return BadRequest("Teacher ID mismatch.");

            using (MySqlConnection conn = _context.AccessDatabase())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"UPDATE teachers SET teacherfname=@fname, teacherlname=@lname, employeenumber=@empnum, hiredate=@hiredate, salary=@salary, teacherworkphone=@workphone
                            WHERE teacherid=@id";
                cmd.Parameters.AddWithValue("@fname", teacher.TeacherFName);
                cmd.Parameters.AddWithValue("@lname", teacher.TeacherLName);
                cmd.Parameters.AddWithValue("@empnum", teacher.EmployeeNumber);
                cmd.Parameters.AddWithValue("@hiredate", teacher.HireDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@salary", teacher.Salary ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@workphone", teacher.TeacherWorkPhone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id", id);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    return NotFound($"Teacher with ID {id} not found.");
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a teacher from the database.
        /// </summary>
        /// <param name="id">The ID of the teacher to delete.</param>
        /// <returns>No content if successful; 404 if not found.</returns>
        /// <example>
        /// DELETE: api/teacher/3
        /// Response: 204 No Content
        /// </example>
        [HttpDelete("{id}")]
        public IActionResult DeleteTeacher(int id)
        {
            using (MySqlConnection conn = _context.AccessDatabase())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM teachers WHERE teacherid=@id";
                cmd.Parameters.AddWithValue("@id", id);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    return NotFound($"Teacher with ID {id} not found.");
            }

            return NoContent();
        }

        /// <summary>
        /// Gets a list of teachers hired between the specified dates (inclusive).
        /// </summary>
        /// <param name="min">Minimum hire date (yyyy-MM-dd)</param>Z
        /// <param name="max">Maximum hire date (yyyy-MM-dd)</param>
        /// <returns>List of teachers hired in the date range.</returns>
        /// <example>
        /// GET: api/teacher/hired?min=2020-01-01&max=2024-12-31
        /// </example>
        [HttpGet("hired")]
        public ActionResult<IEnumerable<Teacher>> GetTeachersByHireDateRange([FromQuery] DateTime min, [FromQuery] DateTime max)
        {
            var teachers = new List<Teacher>();
            using (MySqlConnection conn = _context.AccessDatabase())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT * FROM teachers WHERE hiredate >= @min AND hiredate <= @max";
                cmd.Parameters.AddWithValue("@min", min);
                cmd.Parameters.AddWithValue("@max", max);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        teachers.Add(new Teacher
                        {
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            TeacherFName = reader["teacherfname"].ToString(),
                            TeacherLName = reader["teacherlname"].ToString(),
                            EmployeeNumber = reader["employeenumber"].ToString(),
                            HireDate = reader["hiredate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["hiredate"]),
                            Salary = reader["salary"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["salary"]),
                            TeacherWorkPhone = reader["teacherworkphone"]?.ToString()
                        });
                    }
                }
            }
            return teachers;
        }

        /// <summary>
        /// Gets a list of courses taught by a specific teacher.
        /// </summary>
        /// <param name="id">The teacher's ID.</param>
        /// <returns>List of courses taught by the teacher.</returns>
        /// <example>
        /// GET: api/teacher/5/courses
        /// </example>
        [HttpGet("{id}/courses")]
        public ActionResult<IEnumerable<Course>> ListCoursesForTeacher(int id)
        {
            var courses = new List<Course>();
            using (MySqlConnection conn = _context.AccessDatabase())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT c.courseid, c.coursename, c.coursecode
                                    FROM courses c
                                    INNER JOIN teachers_courses tc ON c.courseid = tc.courseid
                                    WHERE tc.teacherid = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(new Course
                        {
                            CourseId = Convert.ToInt32(reader["courseid"]),
                            CourseName = reader["coursename"].ToString(),
                            CourseCode = reader["coursecode"].ToString()
                        });
                    }
                }
            }
            return courses;
        }

    }
}
