using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorksManagement.Data;
using WorksManagement.Models;
using WorksManagement.Data;

namespace WorksManagement.Controllers
{
    public class WorkTaskController : Controller
    {
        private readonly ApplicationDbContext _db;

        public WorkTaskController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string nameFilter, int page = 1, int pageSize = 6)
        {
            var workTasksQuery = _db.WorkTasks.AsQueryable();

            // Apply filtering by task name
            if (!string.IsNullOrEmpty(nameFilter))
            {
                workTasksQuery = workTasksQuery.Where(w => w.Name.Contains(nameFilter));
            }

            // Calculate total number of items for pagination
            var totalItems = await workTasksQuery.CountAsync();

            // Apply pagination
            var workTasks = await workTasksQuery
                .OrderBy(w => w.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Set ViewData for pagination and filtering
            ViewData["NameFilter"] = nameFilter;
            ViewData["TotalItems"] = totalItems;
            ViewData["CurrentPage"] = page;
            ViewData["PageSize"] = pageSize;

            return View(workTasks);
        }
        // Create (GET)
        public IActionResult Create()
        {
            // Fetching project names for the dropdown
            ViewBag.Projects = _db.Projects.Select(p => new { p.Name }).ToList();
            return View();
        }

        // Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (WorkTask obj)
        {
            if (!ModelState.IsValid)
            {
                _db.WorkTasks.Add(obj);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Repopulate the ViewBag in case of validation failure
            ViewBag.Projects = _db.Projects.Select(p => new { p.Name }).ToList();
            return View(obj);
        }
        // Edit (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var workTask = await _db.WorkTasks.FindAsync(id);

            if (workTask == null)
            {
                return NotFound();
            }

            return View(workTask);
        }

        // Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WorkTask workTask)
        {
            if (ModelState.IsValid)
            {
                _db.WorkTasks.Update(workTask);
                await _db.SaveChangesAsync();

                TempData["Success"] = "Task updated successfully!";
                return RedirectToAction("Index");
            }

            return View(workTask);
        }

    }
}
