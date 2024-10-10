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
            // Query to fetch tasks including related Project data
            var workTasksQuery = _db.WorkTasks.Include(t => t.Project).AsQueryable();

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

            // Pass the paginated and filtered tasks to the view
            return View(workTasks);
        }


        // GET: WorkTask/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Projects = new SelectList(await _db.Projects.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: WorkTask/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkTask obj)
        {
            if (!ModelState.IsValid)
            {
                _db.Add(obj);
                await _db.SaveChangesAsync();

                // Optionally display a success message or redirect to a list view
                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, reload projects list and return the view
            ViewBag.Projects = new SelectList(await _db.Projects.ToListAsync(), "Id", "Name", obj.ProjectId);
            return View(obj);
        }


        // Edit (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Projects = new SelectList(await _db.Projects.ToListAsync(), "Id", "Name");
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
            if (!ModelState.IsValid)
            {
                _db.WorkTasks.Update(workTask);
                await _db.SaveChangesAsync();

                TempData["Success"] = "Task updated successfully!";
                return RedirectToAction("Index");
            }
            // If model state is invalid, reload projects list and return the view
            ViewBag.Projects = new SelectList(await _db.Projects.ToListAsync(), "Id", "Name", workTask.ProjectId);
            return View(workTask);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var workTask = await _db.WorkTasks.FindAsync(id);
            if (workTask == null)
            {
                return NotFound();
            }

            return View(workTask); // Optional: return a confirmation view
        }

        // Delete (POST) - ensure this matches your form action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workTask = await _db.WorkTasks.FindAsync(id);
            if (workTask == null)
            {
                return NotFound();
            }

            _db.WorkTasks.Remove(workTask);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Task deleted successfully!";
            return RedirectToAction("Index");
        }
    }
  
}
