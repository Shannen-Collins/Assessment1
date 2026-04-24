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

   // private void SampleData()
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

        //if input Release year is less than 1888 (first film) or greater than current year, return InvalidYear
        if (movie.Release_Year < 1888 || movie.Release_Year > DateTime.Now.Year) 
        return "InvalidYear"; 

        //add new movie to movie list
        movies.AddLast(movie); 
        //add Movie ID to hashtable
        movieIDTable[movie.Movie_ID] = movie; 
        //return success to indicate the add movie worked
        return "Success"; 
    }

    //sorts movie by title with Bubble Sort algorithm
    public LinkedList<Movie> BubbleSortByTitle()
    {
        //creates copy of the original movie Linked List
        var list = new LinkedList<Movie>(movies);
        //if there is less than 2 movies in the list, no sorting is needed
        if (list.Count < 2) return list;
        //create swapped true or false varible to track when swapped are made
        bool swapped;
        do
        {   //start with swapped set to false
            swapped = false;
            //set the current position in list to the start (first node)
            var current = list.First;
            //loop through the list until the next node is null (last node)
            while (current?.Next != null)
            {   //compare the current movie title with the next movie title
                //if current movie title > next movie title, then they are in wrong order
                if (string.Compare(current.Value.Title, current.Next.Value.Title) > 0)
                {
                    //store the current movie temporarily
                    var temp = current.Value;
                    //move the next movie into the current position
                    current.Value = current.Next.Value;
                    //put the original current movie title saved in temp into the next position
                    current.Next.Value = temp;
                    //a swap occurred, so another loop is needed
                    swapped = true;
                }
                //move to the next node in list
                current = current.Next;
            }
            //repeat loop until no swaps occur, meaning list is sorted
        } while (swapped);
        //return the sorted Linked List
        return list;
    }


}




