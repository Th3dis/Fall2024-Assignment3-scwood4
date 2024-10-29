using Fall2024_Assignment3_scwood4.Data;
using Fall2024_Assignment3_scwood4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fall2024_Assignment3_scwood4.Controllers
{
    public class ActorMovieController : Controller
    {
        ApplicationDbContext _context;

        public ActorMovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: ActorMovie/Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActorMovies.ToListAsync());
        }

        // GET: ActorMovie/Create
        public IActionResult Create()
        {
            // Populate ViewData with actor names and movie titles
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            return View();
        }

        // POST: ActorMovie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActorMovie actorMovie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actorMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate actor and movie lists in case of an error
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", actorMovie.actorId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", actorMovie.movieId);

            return View(actorMovie);
        }
    }
}
