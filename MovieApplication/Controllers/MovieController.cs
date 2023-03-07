using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieApplication.DbContexts;
using MovieApplication.Models;
using MovieApplication.ViewModels;
using System.Net;

namespace MovieApplication.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieAppDBContext _context;
        public MovieController(MovieAppDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var query = (from movie in _context.Movies
                         join genr in _context.Genres on movie.GenreId equals genr.GenreId
                         select new
                         {
                             MovieId = movie.MovieId,
                             Title = movie.Title,
                             ReleaseDate = movie.ReleaseDate,
                             GenreId = movie.GenreId,
                             Genre = genr.Title,
                             Price = movie.Price,
                             CreatedAt = movie.CreatedAt
                         });
            ViewBag.movies = await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
            return View(ViewBag.movies);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            //ViewBag.GenreId = new SelectList(_context.Genres, "GenreId", "Title");
            PopulateGenre();
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,ReleaseDate,GenreId,Price")] MovieVM movieVM)
        {
            if (ModelState.IsValid)
            {
                var movie = _context.Movies.Where(e => e.Title == movieVM.Title);
                if (movie.Count() == 0)
                {
                    _context.Movies.Add(new Movie
                    {
                        Title = movieVM.Title,
                        ReleaseDate = movieVM.ReleaseDate,
                        GenreId = movieVM.GenreId,
                        Price = movieVM.Price,
                        CreatedAt = System.DateTime.Now
                    });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movieVM);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            MovieVM movieVM = new MovieVM();
            if (movie == null)
            {
                return NotFound();
            }
            else 
            {
                movieVM.MovieId = movie.MovieId;
                movieVM.Title = movie.Title;
                movieVM.ReleaseDate = movie.ReleaseDate;
                movieVM.GenreId = movie.GenreId;
                movieVM.Price = movie.Price;
            }
            PopulateGenre(movieVM.GenreId);
            return View(movieVM);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieVM movieVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var movie = _context.Movies.Where(e => e.MovieId == movieVM.MovieId).FirstOrDefault();
                    if (movie != null)
                    {
                        movie.Title = movieVM.Title;
                        movie.ReleaseDate = movieVM.ReleaseDate;
                        movie.GenreId = movieVM.GenreId;
                        movie.Price = movieVM.Price;
                        movie.UpdatedAt = DateTime.Now;
                        _context.Entry(movie).State = EntityState.Modified;
                    };
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to update changes. Try again, and if the problem persists, see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            var query = (from mv in _context.Movies
                         join genr in _context.Genres on mv.GenreId equals genr.GenreId
                         where mv.MovieId == id
                         select new
                         {
                             MovieId = mv.MovieId,
                             Title = mv.Title,
                             ReleaseDate = mv.ReleaseDate,
                             GenreId = mv.GenreId,
                             Genre = genr.Title,
                             Price = mv.Price,
                             CreatedAt = mv.CreatedAt
                         });
            ViewBag.movieData = await query.OrderByDescending(a => a.CreatedAt).FirstOrDefaultAsync();
            if (movie == null)
            {
                return NotFound();
            }
            return View();
        }

        // GET: Movies/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            var query = (from mv in _context.Movies
                         join genr in _context.Genres on mv.GenreId equals genr.GenreId
                         where mv.MovieId == id
                         select new
                         {
                             MovieId = mv.MovieId,
                             Title = mv.Title,
                             ReleaseDate = mv.ReleaseDate,
                             GenreId = mv.GenreId,
                             Genre = genr.Title,
                             Price = mv.Price,
                             CreatedAt = mv.CreatedAt
                         });
            ViewBag.movieData = await query.OrderByDescending(a => a.CreatedAt).FirstOrDefaultAsync();
            if (movie == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Movies/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void PopulateGenre(object selectedGenre = null)
        {
            var genres = (from gnr in _context.Genres
                                   orderby gnr.Title
                                   select gnr).ToList();
            ViewBag.GenreId = new SelectList(genres, "GenreId", "Title", selectedGenre);
        }
    }
}
