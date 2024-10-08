using Microsoft.AspNetCore.Mvc;
using WorksManagement.Models;
using WorksManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WorksManagement.Data;
using WorksManagement.Models;

namespace WorksManagement.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProjectsController(ApplicationDbContext db)
        {
            _db = db;
        }


        // Index - Display List of Projects with Search and Pagination
        public IActionResult Index(string NameFilter, int page = 1, int pageSize = 6)
        {
            // Retrieve all projects
            var projects = from p in _db.Projects
                           select p;

            // Apply search filter
            if (!string.IsNullOrEmpty(NameFilter))
            {
                projects = projects.Where(p => p.Name.Contains(NameFilter));
            }

            // Get the total count of projects after filtering
            int totalItems = projects.Count();

            // Apply pagination
            projects = projects.Skip((page - 1) * pageSize).Take(pageSize);

            // Store the filtered project list and other data to pass it to the view
            ViewData["NameFilter"] = NameFilter;
            ViewData["TotalItems"] = totalItems;
            ViewData["CurrentPage"] = page;
            ViewData["PageSize"] = pageSize;

            return View(projects.ToList());
        }
        
        // Create (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent CSRF attacks
        public async Task<IActionResult> Create(Project obj)
        {
            if (ModelState.IsValid)
            {
                _db.Projects.Add(obj);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(obj);
        }
        // Edit (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _db.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                _db.Projects.Update(project);
                await _db.SaveChangesAsync();

                TempData["Success"] = "Project updated successfully!";
                return RedirectToAction("Index");
            }

            return View(project);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Find the project by ID
            var project = await _db.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            // Find and delete related WorkTasks with the same Project Name
            var relatedTasks = _db.WorkTasks.Where(t => t.ProjectName == project.Name).ToList();
            if (relatedTasks.Any())
            {
                _db.WorkTasks.RemoveRange(relatedTasks);
            }

            // Delete the project
            _db.Projects.Remove(project);

            // Save all changes in one transaction
            await _db.SaveChangesAsync();

            TempData["Success"] = "Project and related tasks deleted successfully!";

            return RedirectToAction("Index");
        }

    }
}
