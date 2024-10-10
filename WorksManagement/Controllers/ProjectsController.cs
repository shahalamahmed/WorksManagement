using Microsoft.AspNetCore.Mvc;
using WorksManagement.Models;
using WorksManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WorksManagement.Data;
using WorksManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Index(string NameFilter, int page = 1, int pageSize = 10)
        {
            // Retrieve all projects
            var projects = from p in _db.Projects
                           select p;
            // Apply search filter
            if (!string.IsNullOrEmpty(NameFilter))
            {
                projects = projects.Where(p => p.Name.Contains(NameFilter)
                || p.Code.Contains(NameFilter)) ;

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

        // GET: Project/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Check if a project with the same name already exists
                bool projectExists = await _db.Projects.AnyAsync(p => p.Name == project.Name);
                if (projectExists)
                {
                    // Add a model error if the project name already exists
                    ModelState.AddModelError("Name", "This name already exists. Please choose a different name.");
                    return View(project);
                }

                // If the name is unique, proceed to add the project
                _db.Add(project);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, redisplay the form with validation errors
            return View(project);
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
        // Delete (POST)
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

            // Find and delete related WorkTasks
            var relatedTasks = _db.WorkTasks.Where(t => t.ProjectId == project.Id).ToList();
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
