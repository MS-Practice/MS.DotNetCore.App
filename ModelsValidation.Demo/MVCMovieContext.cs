using System.Collections;
using System.Collections.Generic;

namespace ModelsValidation.Demo
{
    public class MVCMovieContext
    {
        public IList<Movie> Movies { get; } = new List<Movie>();

        public void AddMovie(Movie movie)
        {
            movie.Id = Movies.Count;
            Movies.Add(movie);
        }

        public void SaveChanges()
        {

        }
    }
}