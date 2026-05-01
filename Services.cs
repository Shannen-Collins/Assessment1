using System.Collections;
using Assessment1.Model;
using System.Text.Json;
using System.IO;

namespace Assessment1.Services;

public class MovieService {

    //LinkedList for storing movies
	private LinkedList<Movie> movies = new LinkedList<Movie>(); 
    //hashtable for fast lookup of Movie ID
    private Hashtable movieIDTable = new Hashtable();
    //queue to manage waiting list for borrowing movies
    private Dictionary<string, Queue<string>> waitingQueue = new();
    //queue for borrow history
    private Queue<string> borrowHistory = new Queue<string>();

    //Returns all movies stored in the collection for display in the UI
    public IEnumerable<Movie> GetAll() 
	{ 
		return movies; 
	} 

    //runs Add Movie input checks 
    public string AddMovie(Movie movie)
    {
        //if the hashtable contains the input Movie ID, return DuplicateID
        if (movieIDTable.ContainsKey(movie.Movie_ID)) 
        return "DuplicateID"; 

        //if input Release year is less than 1888 (first film) or greater than current year, return InvalidYear
        if (movie.Release_Year < 1888 || movie.Release_Year > DateTime.Now.Year) 
        return "InvalidYear"; 

        //Sorts added movie by ID
        InsertMovieSortedID(movie);

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
        //creates swapped true or false variable to track when swaps are made
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

    //sorts movie by year with Merge Sort algorithm
    public LinkedList<Movie> MergeSortByYear()
    {
        //creates copy of the original movie Linked List
        var list = new LinkedList<Movie>(movies);
        // calls recursive merge sort
        return MergeSort(list);
    }

    //recursive method that splits the list into halves
    private LinkedList<Movie> MergeSort(LinkedList<Movie> list)
    {
        //if there is less than 2 movies in the list, no sorting is needed
        if (list.Count < 2) return list;

        //defines the middle of the list by splitting the list in half
        var middle = list.Count / 2;

        //creates new lists for each half
        var left = new LinkedList<Movie>();
        var right = new LinkedList<Movie>();

        //for each movie in the list
        //add it to either the left or right depending on if it is lower or higher than the middle
        int index = 0;
        foreach (var movie in list)
        {
            if (index < middle)
                left.AddLast(movie);
            else
                right.AddLast(movie);

            index++;
        }

        //for each of the halves, run the merge sort function again
        left = MergeSort(left);
        right = MergeSort(right);

        //merge the sorted halves
        return Merge(left, right);
    }

    //merges two sorted linked lists into one sorted list
    private LinkedList<Movie> Merge(LinkedList<Movie> left, LinkedList<Movie> right)
    {
        //set the result as the new sorted list
        var result = new LinkedList<Movie>();
        //start at the first node in each half
        var leftNode = left.First;
        var rightNode = right.First;

        //compare years from both lists and add the lowest one
        while (leftNode != null && rightNode != null)
        {
            if (leftNode.Value.Release_Year <= rightNode.Value.Release_Year)
            {
                result.AddLast(leftNode.Value);
                leftNode = leftNode.Next;
            }
            else
            {
                result.AddLast(rightNode.Value);
                rightNode = rightNode.Next;
            }
        }

        //add any remaining years from the left list
        while (leftNode != null)
        {
            result.AddLast(leftNode.Value);
            leftNode = leftNode.Next;
        }

        //add any remaining years from the right list
        while (rightNode != null)
        {
            result.AddLast(rightNode.Value);
            rightNode = rightNode.Next;
        }

        //return the sorted list
        return result;
    }


    //searches movie list using linear search and filters by title
    public LinkedList<Movie> LinearSearchByTitle(string search)
    {
        //set the results as the new linked list
        var results = new LinkedList<Movie>();
        
        //for each movie in list
        foreach(var movie in movies)
        {
            //if it contains the search term
            if (movie.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
            {
                //add movie to the new list
                results.AddLast(movie);
            }
        }     
        //return the new filtered list
        return results;  
    }

    //takes the results from the binary ID search and puts into new linked list
    public LinkedList<Movie> BinarySearchByID(string targetID)
    {
        //set results as the new empty linked list
        var results = new LinkedList<Movie>();
        //set the found ID as the result from the binary search function
        var found = BinarySearch(targetID);
        //if movie ID is found (not null), add movie to the new list
        if (found != null) results.AddLast(found);
        //return the new filtered list
        return results;
    }

    //main Binary Search function to search for movie ID
    private Movie? BinarySearch(string targetID)
    {
        //set start as the first node in linked list
        var start = movies.First;
        //set end stop point as end of list
        LinkedListNode<Movie>? end = null;

        //while start is not null and has not reached the end, loop through list
        while (start != null && start != end)
        {
            //set the middle to the Get Middle function results, to allow binary search on linked list (as part of requirements)
            var mid = GetMiddle(start, end);

            //compare the middle ID value with the target ID value and set it as an integer
            int comparison = string.Compare(mid.Value.Movie_ID, targetID, StringComparison.OrdinalIgnoreCase);

            //if the comparison integer is 0, or the middle (match found)
            if (comparison == 0)
                //then the target ID is found, and returns the ID value
                return mid.Value;
            //if the target ID is after the middle 
            else if (comparison < 0)
                //set the middle as the new start and search everything to the right of the old middle
                start = mid.Next;
            //else, then the target ID is before the middle
            else
                //move end to middle and search the everything to the left of the old middle
                end = mid;
        }
        //return null if ID target not found
        return null;
    }

    //finds middle node using slow/fast pointer technique (required for binary search on linked list)
    private LinkedListNode<Movie> GetMiddle(
        LinkedListNode<Movie> start, 
        LinkedListNode<Movie>? end)
    {
        //sets slow and fast variables
        var slow = start; //moves 1 node at a time
        var fast = start; //moves 2 nodes at a time

        //while fast is not null, not at the end, and the next fast node is not the end
        while (fast != end && fast !=null && fast.Next != end)
        {
            //set the fast as the value next to the next node (every 2 nodes)
            fast = fast.Next?.Next;
            //set the slow as the next value (every 1 node)
            slow = slow.Next!;
        }
        //return the slow node value 
        return slow;
    }

    //exports chosen data to JSON file format
    public void ExportToJson<T>(T data, string filePath)
    {
        //converts export data to json string
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            //formats json
            WriteIndented = true
        });

        //writes json string into file 
        File.WriteAllText(filePath, json);
    }

    //exports movie list to json file
    public void ExportMovies(string filePath)
    {
        //runs export to JSON function on the movie list with selected file path
        ExportToJson(movies.ToList(), filePath);
    }

    //imports movie list from json file
    public void ImportMoviesFromJson(string filePath)
    {
        //if file doesn't exist, don't import
        if (!File.Exists(filePath))
            return;

        //reads json file
        var json = File.ReadAllText(filePath);

        //converts json string file to list
        var list = JsonSerializer.Deserialize<List<Movie>>(json);

        //if file is empty or invalid, stop
        if (list == null || list.Count == 0) 
            throw new InvalidDataException("Empty or invalid movie data");

        //runs rebuild collection function
        RebuildMovieCollection(list);
    }

    //rebuilds linked list and hashtable from imported data
    private void RebuildMovieCollection(List<Movie> list)
    {
        //removes all current movies in collection
        movies.Clear();
        //resets lookup table
        movieIDTable.Clear();

        //for each movie in the imported list
       foreach (var movie in list)
        {
            InsertMovieSortedID(movie);
            //add Movie ID to table
            movieIDTable[movie.Movie_ID] = movie;
        }
    }
    
    //function that ensures stored movie list is sorted by ID for binary search
    private void InsertMovieSortedID(Movie movie)
    {
       //if there are no movies in list
        if (movies.Count == 0)
        {   
            //add new movie to first node of list
            movies.AddFirst(movie);
        }
        //if list has movies already
        else
        {
            //set current as first node of movie list
            var current = movies.First;
            //go through list until added movie ID is greater than the current movie ID on list
            while (current != null && string.Compare(current.Value.Movie_ID, movie.Movie_ID, StringComparison.OrdinalIgnoreCase) < 0)
            {
                //go to the next node
                current = current.Next;
            }
            //if ID is greater than IDs in movie list, add movie to end of list
            if (current == null) movies.AddLast(movie);
            //if node greater than new ID is found, add movie before that node
            else movies.AddBefore(current, movie);
        }
    }

    //function for borrowing movies
    public string BorrowMovie(string movieID, string username)
    {
        //looks up movie ID in hashtable and converts result into Movie object
        var movie = movieIDTable[movieID] as Movie;
        
        //if ID is not there, returns Not Found
        if (movie == null)
            return "NotFound";
        
        //if movie is available
        if(movie.Availability == "Available")
        {
            //change status to borrowed
            movie.Availability = "Borrowed";
            //adds borrowed record to borrow history
            borrowHistory.Enqueue($"User, {username}, borrowed {movieID} at {DateTime.Now}");
            //return borrowed status
            return "Borrowed";
        }

        //if a waiting queue for the Movie ID doesn't exist
        if (!waitingQueue.ContainsKey(movieID))
        {
            //creates new queue for Movie
            waitingQueue[movieID] = new Queue<string>();
        }
        //add user to waiting queue for movie
        waitingQueue[movieID].Enqueue(username);
        //adds queued record to borrow history
        borrowHistory.Enqueue($"User, {username}, queued for {movieID} at {DateTime.Now}");
        //return queued status
        return "Queued";
    }

    //function for returning movies
    public string ReturnMovie(string movieID)
    {
        //looks up movie ID in hashtable and converts result into Movie object
        var movie = movieIDTable[movieID] as Movie;
        
        //if ID is not there, returns Not Found
        if (movie == null)
            return "NotFound";

        //if a waiting queue for the Movie ID doesn't exist or is empty
        if (!waitingQueue.ContainsKey(movieID) || waitingQueue[movieID].Count == 0)
        {
            //make movie available 
            movie.Availability = "Available";

            //adds movie return record to borrow history
            borrowHistory.Enqueue($"{movieID} was returned at {DateTime.Now}");

            //mark the movie as returned
            return "Returned";
        }

        //gives movie to next user
        var nextUser = waitingQueue[movieID].Dequeue();

        //marks movie as borrowed again
        movie.Availability = "Borrowed";

        //adds movie return record to borrow history
        borrowHistory.Enqueue($"{movieID} was returned at {DateTime.Now}");

        //adds auto borrowed record to borrow history
        borrowHistory.Enqueue($"User, {nextUser}, automatically borrowed {movieID} at {DateTime.Now}");

        //returns notification information for next user
        return $"Assigned to: {nextUser}";

    }

    //exports borrow history to JSON file
    public void ExportBorrowHistory(string filePath)
    {   
        //runs export to JSON function with borrow history and chosen file path
        ExportToJson(borrowHistory, filePath);
    }

}




