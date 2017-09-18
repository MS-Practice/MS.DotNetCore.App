using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ModelsValidation.Demo
{
    public class MoviesController : Controller
    {
        private MVCMovieContext _context;

        public MoviesController(MVCMovieContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_context.Movies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            string title,
            Genre genre,
            DateTime dateTime,
            string description,
            decimal price,
            bool preorder)
        {
            var modifiedReleaseDate = dateTime;
            if (dateTime == null) modifiedReleaseDate = DateTime.Today;
            var movie = new Movie
            {
                Title = title,
                Genre = genre,
                ReleaseDate = modifiedReleaseDate,
                Description = description,
                Price = price,
                Preorder = preorder
            };
            TryValidateModel(movie);
            if (ModelState.IsValid)
            {
                _context.AddMovie(movie);
                _context.SaveChanges();
                return RedirectToAction(actionName: nameof(Index));
            }

            return View(movie);
        }
    }
}
