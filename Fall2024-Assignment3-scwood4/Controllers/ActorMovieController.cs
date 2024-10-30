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
            var applicationDbContext = _context.ActorMovies.Include(a => a.actor).Include(a => a.movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ActorMovie/Create
        public IActionResult Create()
        {
            // Populate ViewData with actor names and movie titles
            ViewData["actorId"] = new SelectList(_context.Actors, "Id", "Name");
            ViewData["movieId"] = new SelectList(_context.Movies, "Id", "Title");
            return View();
        }

        // POST: ActorMovie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActorMovie actorMovie)
        {
            // Log the incoming actorMovie values
            Console.WriteLine($"ActorId: {actorMovie.actorId}, MovieId: {actorMovie.movieId}");

            if (ModelState.IsValid)
            {
                _context.Add(actorMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Log ModelState errors
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"Key: {entry.Key}, Error: {error.ErrorMessage}");
                }
            }

            // Repopulate actor and movie lists in case of an error
            ViewData["actorId"] = new SelectList(_context.Actors, "Id", "Name");
            ViewData["movieId"] = new SelectList(_context.Movies, "Id", "Title");

            return View(actorMovie);
        }

        // GET: ActorMovis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorMovie = await _context.ActorMovies.FindAsync(id);
            if (actorMovie == null)
            {
                return NotFound();
            }

            // Populate ViewData with actor names and movie titles
            ViewData["actorId"] = new SelectList(_context.Actors, "Id", "Name", actorMovie.actorId);
            ViewData["movieId"] = new SelectList(_context.Movies, "Id", "Title", actorMovie.movieId);

            return View(actorMovie);
        }

        // POST: ActorMovie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActorMovie actorMovie)
        {
            if (id != actorMovie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actorMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorMovieExists(actorMovie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate actor and movie lists in case of an error
            ViewData["actorId"] = new SelectList(_context.Actors, "Id", "Name", actorMovie.actorId);
            ViewData["movieId"] = new SelectList(_context.Movies, "Id", "Title", actorMovie.movieId);

            return View(actorMovie);
        }

        // GET: ActorMovie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorMovie = await _context.ActorMovies
                .Include(a => a.actor)
                .Include(a => a.movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actorMovie == null)
            {
                return NotFound();
            }

            return View(actorMovie);
        }

        // POST: ActorMovie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorMovie = await _context.ActorMovies.FindAsync(id);
            if (actorMovie != null)
            {
                _context.ActorMovies.Remove(actorMovie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorMovieExists(int id)
        {
            return _context.ActorMovies.Any(e => e.Id == id);
        }
    }
}
