## Assignment Name: C# Cumulative Assignment 3

## Name: Phurten Jang Sherpa

## Tasks

- [✔] **Web API – Get All Teachers**  
      `GET http://localhost:{port}/api/teacher`  
      Returns a list of all teachers from the MySQL database.

- [✔] **Web API – Get Teacher by ID**  
      `GET http://localhost:{port}/api/teacher/{id}`  
      Returns detailed information for a specific teacher by their ID.

- [✔] **Web API – Add Teacher**  
      `POST http://localhost:{port}/api/teacher`  
      Adds a new teacher to the database. Includes validation for required fields, unique employee number, correct format, and hire date not in the future.

- [✔] **Web API – Delete Teacher**  
      `DELETE http://localhost:{port}/api/teacher/{id}`  
      Deletes a teacher by ID. Returns 404 if the teacher does not exist.

- [✔] **Web API – Update Teacher (PUT)** 
      `PUT http://localhost:5215/api/teacher/{id}` 
      Updates a teacher’s information with full server-side validation.
      Handles errors: not found, empty/invalid fields, duplicate employee number, future hire date, negative salary.

- [✔] **MVC Page – List All Teachers**  
      `/TeacherPage/List`  
      Displays a server-rendered list of all teachers, including work phone and action buttons.

- [✔] **MVC Page – Show Teacher Details**  
      `/TeacherPage/Show/{id}`  
      Displays all available information for a specific teacher. Shows a friendly error if the teacher is not found.

- [✔] **MVC Page – Add New Teacher**  
      `/TeacherPage/New`  
      Form to add a new teacher with validation for all fields.

- [✔] **MVC Page – Delete Confirmation**  
      `/TeacherPage/DeleteConfirm/{id}`  
      Asks the user to confirm deletion of a teacher.

- [✔] **MVC Page – Edit Teacher**
      `/TeacherPage/Edit/{id}`
      - AJAX form for editing a teacher with client-side and server-side validation.
      - User-friendly error and success messages.
      - "Back to List" button for easy navigation.

- [✔] **Validation & Error Handling**  
      - Required fields, unique employee number, correct format, hire date not in the future, and positive salary.
      - User-friendly error messages in forms and details view.

- [✔] **Styling**  
      - Used Bootstrap for consistent button and form styling.
      - "Add New Teacher", "Search", and "Details" buttons are styled black (`btn-dark`).

- [✔] **Initiative Features**
      - Server-side validation for all fields and uniqueness.
      - Client-side validation for all fields.
      - AJAX update for seamless user experience.
      - Duplicate employee number prevention on both add and update.

## Learning curve and challenges

- Connecting ASP.NET Core to MySQL on Mac
- Differentiating between Web API controllers and MVC page controllers
- Debugging null data issues and testing endpoint routing
- Writing clean and descriptive `<summary>` blocks for API methods
- Implementing model validation and user-friendly error handling
- Implementing both client-side and server-side validation
- Handling AJAX form submissions and displaying API errors
- Preventing duplicate employee numbers on update
- Writing clear XML documentation for API and model
- Using cURL and Postman for thorough API testing

## Resources

- Class Lectures  
- Class Exercises  
- https://learn.microsoft.com/en-us/dotnet/?view=net-8.0  
- https://learn.microsoft.com/en-us/aspnet/core/mvc/views/overview?view=aspnetcore-8.0  
- https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0  
- https://curl.se/docs/manpage.html
