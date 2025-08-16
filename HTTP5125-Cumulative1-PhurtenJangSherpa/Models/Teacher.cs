using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    /// <summary>
    /// Represents a teacher in the school database.
    /// </summary>
    public class Teacher
    {
        /// <summary>
        /// The teacher id.
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// The first name of the teacher.
        /// </summary>
        [Required(ErrorMessage = "First name is required.")]
        public string? TeacherFName { get; set; }

        /// <summary>
        /// The last name of the teacher.
        /// </summary>
        [Required(ErrorMessage = "Last name is required.")]
        public string? TeacherLName { get; set; }

        /// <summary>
        /// The employee number of the teacher.
        /// </summary>
        [Required(ErrorMessage = "Employee number is required.")]
        [StringLength(10)]
        [RegularExpression(@"^T\d+$", ErrorMessage = "Employee number must start with 'T' followed by digits (e.g., T123).")]
        public string? EmployeeNumber { get; set; }

        /// <summary>
        /// The hire date of the teacher.
        /// </summary>
        [HireDateNotInFuture(ErrorMessage = "Hire date cannot be in the future.")]
        public DateTime? HireDate { get; set; }

        /// <summary>
        /// The salary of the teacher.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be non-negative.")]
        public decimal? Salary { get; set; }

        /// <summary>
        /// The work phone number of the teacher.
        /// </summary>
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? TeacherWorkPhone { get; set; }

        /// <summary>
        /// The list of courses taught by the teacher.
        /// </summary>
        public List<Course>? CoursesTaught { get; set; }
    }

    /// <summary>
    /// Validation attribute to ensure hire date is not in the future.
    /// </summary>
    public class HireDateNotInFutureAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                return date <= DateTime.Today;
            }
            return true; // Null is allowed
        }
    }
}
