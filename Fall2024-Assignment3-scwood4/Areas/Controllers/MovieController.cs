using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_scwood4.Models;
using Fall2024_Assignment3_scwood4.Data;
using Fall2024_Assignment3_scwood4.Views;
using System;


namespace Fall2024_Assignment3_scwood4.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;


        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        //GET: Movie/Index
        public async Task<IActionResult> Index() {
            return View(await _context.Movies.ToListAsync());
        }

        //GET: Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var reviewModel = new ReviewModel(movie.Title, movie.ReleaseYear);
            await reviewModel.GetMovieReviews();

            double[] sentimentScores = reviewModel.CalculateSentiment();
            double totalSentiment = sentimentScores.Sum();
            double avgSentiment = Math.Round(totalSentiment / sentimentScores.Length, 2);

            var viewModel = new MovieDetailsView(movie, reviewModel.Reviews, sentimentScores, avgSentiment);

            return View(viewModel);
        }



        // GET: Movie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
				_context.Movies.Remove(movie);
				await _context.SaveChangesAsync();
			}

            return RedirectToAction(nameof(Index));
        }
    }
}
