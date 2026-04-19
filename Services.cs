using System.Collections;
using Assessment1.Model;

namespace Assessment1.Services;

public class MovieService {
	private LinkedList<Movie> movies = new LinkedList<Movie>(); 
    private Hashtable movieIDTable = new Hashtable();

    public MovieService()
    {
        SampleData();
    }

    public IEnumerable<Movie> GetAll() 
	{ 
		return movies; 
	} 

    private void SampleData()
	{ 
		movies.AddLast(new Movie() { Movie_ID = "M1", Title = "Back to the Future", Director = "Robert Zemeckis", Genre = "Sci-Fi", Release_Year = 1985, Availability = "Available"}); 
		movies.AddLast(new Movie() { Movie_ID = "M2", Title = "Star Wars", Director = "George Lucas", Genre = "Sci-Fi", Release_Year = 1977, Availability = "Available"}); 
		movies.AddLast(new Movie() { Movie_ID = "M3", Title = "That Darn Cat!", Director = "Robert Stevenson", Genre = "Comedy", Release_Year = 1965, Availability = "Available"}); 
		movies.AddLast(new Movie() { Movie_ID = "M4", Title = "The Final Countdown", Director = "Don Taylor", Genre = "Sci-Fi", Release_Year = 1980, Availability = "Available"}); 
	} 
}



