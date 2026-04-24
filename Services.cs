using System.Collections;
using Assessment1.Model;

namespace Assessment1.Services;

public class MovieService {

    //LinkedList for storing movies
	private LinkedList<Movie> movies = new LinkedList<Movie>(); 
    //hashtable for fast lookup of Movie ID
    private Hashtable movieIDTable = new Hashtable();

    //public MovieService()
    //{
        //shows sample data on datagrid
      //  SampleData();
   // }

    //Returns all movies stored in the collection for display in the UI
    public IEnumerable<Movie> GetAll() 
	{ 
		return movies; 
	} 

    //private void SampleData()
	//{ 
        //Sample Movie data for UI testing
	//	AddMovie(new Movie() { Movie_ID = "M1", Title = "Back to the Future", Director = "Robert Zemeckis", Genre = "Sci-Fi", Release_Year = 1985, Availability = "Available"}); 
	//	AddMovie(new Movie() { Movie_ID = "M2", Title = "Star Wars", Director = "George Lucas", Genre = "Sci-Fi", Release_Year = 1977, Availability = "Available"}); 
	//	AddMovie(new Movie() { Movie_ID = "M3", Title = "That Darn Cat!", Director = "Robert Stevenson", Genre = "Comedy", Release_Year = 1965, Availability = "Available"}); 
	//	AddMovie(new Movie() { Movie_ID = "M4", Title = "The Final Countdown", Director = "Don Taylor", Genre = "Sci-Fi", Release_Year = 1980, Availability = "Available"}); 
	//} 

    //runs Add Movie input checks 
    public string AddMovie(Movie movie)
    {
        //if the hashtable contains the input Movie ID, return DuplicateID
        if (movieIDTable.ContainsKey(movie.Movie_ID)) 
        return "DuplicateID"; 

        //if input Release year is less than 1888 (first vehicle) or greater than current year, return InvalidYear
        if (movie.Release_Year < 1888 || movie.Release_Year > DateTime.Now.Year) 
        return "InvalidYear"; 

        //add new movie to movie list
        movies.AddLast(movie); 
        //add Movie ID to hashtable
        movieIDTable[movie.Movie_ID] = movie; 
        //return success to indicate the add movie worked
        return "Success"; 
    }
}



