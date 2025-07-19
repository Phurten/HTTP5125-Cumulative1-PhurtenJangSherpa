using School.Models;
using School.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Register DB Context
builder.Services.AddScoped<SchoolDbContext>();

// Register API Controller (so MVC controllers can inject if needed)
builder.Services.AddScoped<TeacherController>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map API controllers automatically
app.MapControllers();

// MVC default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TeacherPage}/{action=List}/{id?}");

app.Run();
