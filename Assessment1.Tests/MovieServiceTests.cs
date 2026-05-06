using Xunit;
using Assessment1.Model;
using Assessment1.Services;
using System.Text.Json;

namespace Assessment1.Tests;

//Test to check if xUnit tests are set up and running correctly
public class SmokeTests
{
    [Fact]
    public void TestFramework_IsWorking()
    {
        Assert.True(true);
    }
}

//Tests for the Add Movie service
public class AddMovieServiceTests
{
    //Verifies movie list starts empty
    [Fact] 
    public void GetAll_StartsEmpty_ReturnsNoMovies() 
    { 
        var service = new MovieService(); 
        var result = service.GetAll(); 
        Assert.Empty(result); 
    } 

    //Verifies that a valid movie can be added to the movie list using the Add Movie service function
    [Fact] 
    public void AddMovie_ValidMovie_ReturnsSuccess() 
    { 
        var service = new MovieService(); 
        var movie = new Movie 
        {
            Movie_ID = "M1",
            Title = "Back to the Future",
            Director = "Robert Zemeckis",
            Genre = "Sci-Fi",
            Release_Year = 1985,
            Availability = "Available"
        };
        var result = service.AddMovie(movie);
        Assert.Equal("Success", result);
        Assert.Single(service.GetAll());
    }

    //Verifies that the service will return "DuplicateID" if IDs are the same
    [Fact]
    public void AddMovie_DuplicateID_ReturnsDuplicateID()
    {
        var service = new MovieService(); 
        service.AddMovie(new Movie
        {
            Movie_ID = "M1",
            Title = "Back to the Future",
            Director = "Robert Zemeckis",
            Genre = "Sci-Fi",
            Release_Year = 1985,
            Availability = "Available"
        });

        var result = service.AddMovie(new Movie
        {            
            Movie_ID = "M1",
            Title = "Back to the Future 2",
            Director = "Robert Zemeckis",
            Genre = "Sci-Fi",
            Release_Year = 1989,
            Availability = "Available"
        });

        Assert.Equal("DuplicateID", result);
        Assert.Single(service.GetAll());

    }

    //Verifies that the service will return "InvalidYear" if release year is too old for a movie to be made
    [Fact]
    public void AddMovie_InvalidYear_PastYear()
    {
        var service = new MovieService(); 
        var movie = new Movie 
        {
            Movie_ID = "M3",
            Title = "Past Back to the Future",
            Director = "Robert Zemeckis",
            Genre = "Sci-Fi",
            Release_Year = 1885,
            Availability = "Available"
        };
        var result = service.AddMovie(movie);
        Assert.Equal("InvalidYear", result);
        
    }

    //Verifies that the service will return "InvalidYear" if release year is in the future, out of boundary
    [Fact]
    public void AddMovie_InvalidYear_FutureYear()
    {
        var service = new MovieService(); 
        var movie = new Movie 
        {
            Movie_ID = "M4",
            Title = "Future Back to the Future",
            Director = "Robert Zemeckis",
            Genre = "Sci-Fi",
            Release_Year = DateTime.Now.Year+1,
            Availability = "Available"
        };
        var result = service.AddMovie(movie);
        Assert.Equal("InvalidYear", result);
    }

    //Verifies that the service will return "Success" if the release year is the present year (just within boundary)
    [Fact]
    public void AddMovie_ValidYear_LatestBoundaryYear_PresentYear()
    {
        var service = new MovieService(); 
        var movie = new Movie 
        {
            Movie_ID = "M5",
            Title = "Present Back to the Future",
            Director = "Robert Zemeckis",
            Genre = "Sci-Fi",
            Release_Year = DateTime.Now.Year,
            Availability = "Available"
        };
        var result = service.AddMovie(movie);
        Assert.Equal("Success", result);
    }

    //Verifies that the service will return "Success" if release year is the earliest boundary year of 1888
    [Fact]
    public void AddMovie_ValidYear_EarliestBoundaryYear_1888()
    {
        var service = new MovieService(); 
        var movie = new Movie 
        {
            Movie_ID = "M6",
            Title = "Roundhay Garden Scene",
            Director = "Louis Le Prince",
            Genre = "Short Film",
            Release_Year = 1888,
            Availability = "Available"
        };
        var result = service.AddMovie(movie);
        Assert.Equal("Success", result);
    }

    //Verifies that the service will return "InvalidYear" if release year is just out of boundary, one year earlier than earliest boundary year
    [Fact]
    public void AddMovie_InvalidYear_JustOutOfBoundary_1887()
    {
        var service = new MovieService(); 
        var movie = new Movie 
        {
            Movie_ID = "M7",
            Title = "One Year Earlier: Roundhay Garden Scene",
            Director = "Louis Le Prince",
            Genre = "Short Film",
            Release_Year = 1887,
            Availability = "Available"
        };
        var result = service.AddMovie(movie);
        Assert.Equal("InvalidYear", result);
    }

    //Verifies that the service will return "InvalidMovie" if the movie is null, ensuring null movies are rejected
    [Fact]
    public void AddMovie_NullMovie_ReturnsInvalidMovie()
    {
        var service = new MovieService();
        var result = service.AddMovie(null);
        Assert.Equal("InvalidMovie", result);
    }

    //Verifies that the service will return "InvalidMovieData" if the required movie fields are empty
    [Fact]
    public void AddMovie_EmptyFields_ReturnsInvalidMovieData()
    {
        var service = new MovieService();
        var movie = new Movie
        {
            Movie_ID = "",
            Title = "",
            Director = "",
            Genre = "",
            Release_Year = 2000,
            Availability = "Available"
        };

        var result = service.AddMovie(movie);
        Assert.Equal("InvalidMovieData", result);
    }

    //Stress test that verifies the Add Movie service can handle a large amount (1000) of movies
    [Fact]
    public void AddMovie_StressTest_1000Movies()
    {
        var service = new MovieService();
        for (int i = 0; i < 1000; i++)
        {
            var movie = new Movie
            {
                Movie_ID = "M" + i,
                Title = "Movie" + i,
                Director = "Me",
                Genre = "Fantasy",
                Release_Year = 2000,
                Availability = "Available"
            };
            var result = service.AddMovie(movie);
            Assert.Equal("Success", result);
        }
        Assert.Equal(1000, service.GetAll().Count());
    }

}

//Tests for the Sort Movie services
public class SortMovieServiceTests
{
    //Verifies that the bubble sort arranges the movies by title
    [Fact] 
    public void BubbleSortByTitle_ReturnsSortedTitles() 
    { 
        var service = new MovieService(); 
        service.AddMovie(new Movie
         { 
            Movie_ID = "M10",
            Title = "B",
            Director = "Me",
            Genre = "Fantasy",
            Release_Year = 2000,
            Availability = "Available"
        });
        service.AddMovie(new Movie
        { 
            Movie_ID = "M11",
            Title = "C",
            Director = "Me",
            Genre = "Fantasy",
            Release_Year = 1995,
            Availability = "Available"
        });
        service.AddMovie(new Movie
        { 
            Movie_ID = "M12",
            Title = "A",
            Director = "Me",
            Genre = "Fantasy",
            Release_Year = 2010,
            Availability = "Available"
        });

        var sorted = service.BubbleSortByTitle().ToList();
        Assert.Equal("A", sorted[0].Title);
        Assert.Equal("B", sorted[1].Title);
        Assert.Equal("C", sorted[2].Title);
    }

    //Verifies that the merge sort arranges the movies by release year  
    [Fact] 
    public void MergeSortByYear_ReturnsSortedYears() 
    { 
        var service = new MovieService(); 
        service.AddMovie(new Movie
         { 
            Movie_ID = "M10",
            Title = "B",
            Director = "Me",
            Genre = "Fantasy",
            Release_Year = 2000,
            Availability = "Available"
        });
        service.AddMovie(new Movie
        { 
            Movie_ID = "M11",
            Title = "C",
            Director = "Me",
            Genre = "Fantasy",
            Release_Year = 1995,
            Availability = "Available"
        });
        service.AddMovie(new Movie
        { 
            Movie_ID = "M12",
            Title = "A",
            Director = "Me",
            Genre = "Fantasy",
            Release_Year = 2010,
            Availability = "Available"
        });

        var sorted = service.MergeSortByYear().ToList();
        Assert.Equal(1995, sorted[0].Release_Year);
        Assert.Equal(2000, sorted[1].Release_Year);
        Assert.Equal(2010, sorted[2].Release_Year);
    }
}

//Tests for the Search services
public class SearchMovieServiceTests
{
    //list of movies to be used in the search tests
    private MovieService SearchServiceMovies()
    {
        var service = new MovieService(); 
        service.AddMovie(new Movie
        {
            Movie_ID = "M13",
            Title = "Back to the Future",
            Director = "Robert Zemeckis",
            Genre = "Sci-Fi",
            Release_Year = 1985,
            Availability = "Available"
        });
        service.AddMovie(new Movie
        { 
            Movie_ID = "M14",
            Title = "Star Wars",
            Director = "George Lucas",
            Genre = "Sci-Fi",
            Release_Year = 1977,
            Availability = "Available"
        });
        service.AddMovie(new Movie
        { 
            Movie_ID = "M15",
            Title = "That Darn Cat!",
            Director = "Robert Stevenson",
            Genre = "Comedy",
            Release_Year = 1965,
            Availability = "Available"
        });

        return service;
        
    }

    //Verifies that Linear Search by Title finds the movie matching the search term title
    [Fact]
    public void LinearSearchByTitle_ReturnsMatchingMovie()
    {
        var service = SearchServiceMovies();
        var result = service.LinearSearchByTitle("Back");
        Assert.Single(result);
        Assert.Equal("M13", result.First!.Value.Movie_ID);
    }

    //Verifies that Linear Search by Title is not case sensitive
    [Fact]
    public void LinearSearchByTitle_ReturnsMatchingMovie_NotCaseSensitive()
    {
        var service = SearchServiceMovies();
        var result = service.LinearSearchByTitle("star");
        Assert.Single(result);
        Assert.Equal("M14", result.First!.Value.Movie_ID);
    }

    //Verifies that Linear Search by Title returns empty when no matching results are found
    [Fact]
    public void LinearSearchByTitle_NoResultsFound_ReturnsEmpty()
    {
        var service = SearchServiceMovies();
        var result = service.LinearSearchByTitle("Fake Movie");
        Assert.Empty(result);
    }

    //Verifies that Binary Search by ID finds the movie matching the search term ID
    [Fact]
    public void BinarySearchByID_ReturnsMatchingMovie()
    {
        var service = SearchServiceMovies();
        var result = service.BinarySearchByID("M13");
        Assert.Single(result);
        Assert.Equal("M13", result.First!.Value.Movie_ID);
    }

    //Verifies that Binary Search by ID returns empty when no matching result is found
    [Fact]
    public void BinarySearchByID_NotFound_ReturnsEmpty()
    {
        var service = SearchServiceMovies();
        var result = service.BinarySearchByID("M80");
        Assert.Empty(result);
    }

    //Verifies that Binary Search by ID is not case sensitive
    [Fact]
    public void BinarySearchByID_ReturnsMatchingMovie_NotCaseSensitive()
    {
        var service = SearchServiceMovies();
        var result = service.BinarySearchByID("m13");
        Assert.Single(result);
        Assert.Equal("M13", result.First!.Value.Movie_ID);
    }

    
}

//Tests for Import and Export services
public class ImportExportMovieServicesTests
{
    //Verifies that exporting to a JSON file creates a file with valid content
    [Fact]
    public void ExportMovies_ValidFile_CreatesFileWithContent()
    {
        var service = new MovieService();
        var tempFile = Path.GetTempFileName();
        try{
            service.AddMovie(new Movie
            {
                Movie_ID = "M20",
                Title = "Back to the Future",
                Director = "Robert Zemeckis",
                Genre = "Sci-Fi",
                Release_Year = 1985,
                Availability = "Available"
            });
            service.ExportMovies(tempFile);
            Assert.True(File.Exists(tempFile));

            var content = File.ReadAllText(tempFile);
            Assert.False(string.IsNullOrWhiteSpace(content));
        }
        finally{
            File.Delete(tempFile);
        }
    }

    //Verifies that importing movies from a valid JSON file correctly adds to the collection
    [Fact]
    public void ImportMovies_ValidFile_PopulatesCollection()
    {
        var service = new MovieService();
        var tempFile = Path.GetTempFileName();
        try{
            var movies = new List<Movie>
            { 
                new Movie
                {
                    Movie_ID = "M21",
                    Title = "Back to the Future",
                    Director = "Robert Zemeckis",
                    Genre = "Sci-Fi",
                    Release_Year = 1985,
                    Availability = "Available"
                }
            };
            var json = JsonSerializer.Serialize(movies);
            File.WriteAllText(tempFile, json);
            service.ImportMoviesFromJson(tempFile);

            var result = service.GetAll();
            Assert.Single(result);
            Assert.Equal("M21", result.First().Movie_ID);
        }
        finally{
            File.Delete(tempFile);
        }
    }

    //Verifies that importing from a non-existent file does nothing and does not crash
    [Fact]
    public void ImportMovies_FileDoesNotExist_DoesNothing()
    {
        var service = new MovieService();
        var fakePath = "fake_file.json";
        service.ImportMoviesFromJson(fakePath);
        Assert.Empty(service.GetAll());
    }

    //Verifies that importing movies replaces existing movies in collection
    [Fact]
    public void ImportMovies_ValidFile_ReplacesExistingMovies()
    {
        var service = new MovieService();
        var tempFile = Path.GetTempFileName();
        try{
            service.AddMovie(new Movie
            {
                Movie_ID = "OLD",
                Title = "OLD Back to the Future",
                Director = "Robert Zemeckis",
                Genre = "Sci-Fi",
                Release_Year = 1985,
                Availability = "Available"
            });

            var newMovie = new List<Movie>
            {
                new Movie
                {
                Movie_ID = "NEW",
                Title = "NEW Back to the Future",
                Director = "Robert Zemeckis",
                Genre = "Sci-Fi",
                Release_Year = 1985,
                Availability = "Available"
                }
            };

            File.WriteAllText(tempFile, JsonSerializer.Serialize(newMovie));
            service.ImportMoviesFromJson(tempFile);

            var result = service.GetAll();
            Assert.Single(result);
            Assert.Equal("NEW", result.First().Movie_ID);
        }
        finally{
            File.Delete(tempFile);
        }
    }

    //Verifies that movies are sorted by ID when imported (required for the binary search to work)
    [Fact]
    public void ImportMovies_ValidFile_MoviesSortedByID()
    {
        var service = new MovieService();
        var tempFile = Path.GetTempFileName();
        try{
            var movies = new List<Movie>
            { 
                new Movie
                {
                    Movie_ID = "M3",
                    Title = "Back to the Future 3",
                    Director = "Robert Zemeckis",
                    Genre = "Sci-Fi",
                    Release_Year = 1985,
                    Availability = "Available"
                },
                new Movie
                {
                    Movie_ID = "M1",
                    Title = "Back to the Future 1",
                    Director = "Robert Zemeckis",
                    Genre = "Sci-Fi",
                    Release_Year = 1985,
                    Availability = "Available"
                },
                new Movie
                {
                    Movie_ID = "M2",
                    Title = "Back to the Future 2",
                    Director = "Robert Zemeckis",
                    Genre = "Sci-Fi",
                    Release_Year = 1985,
                    Availability = "Available"
                }
            };
            File.WriteAllText(tempFile, JsonSerializer.Serialize(movies));
            service.ImportMoviesFromJson(tempFile);

            var result = service.GetAll().ToList();
            Assert.Equal("M1",result[0].Movie_ID);
            Assert.Equal("M2",result[1].Movie_ID);
            Assert.Equal("M3",result[2].Movie_ID);
        }
        finally{
            File.Delete(tempFile);
        }
    }

    //Verifies that importing invalid JSON does not crash the program or modify the movie collection 
    [Fact]
    public void ImportMovies_InvalidJson_ThrowsJsonException()
    {
        var service = new MovieService();
        var tempFile = Path.GetTempFileName();
        try{
            File.WriteAllText(tempFile, "Invalid JSON");
            Assert.Throws<System.Text.Json.JsonException>(() => 
            service.ImportMoviesFromJson(tempFile));
        }
        finally{
            File.Delete(tempFile);
        }
    }

    //Stress test that verifies the import and export services can handle a large number (1000) of Movies
    [Fact]
    public void ImportExport_StressTest_1000Movies()
    {
        var service = new MovieService();
        var tempFile = Path.GetTempFileName();

        try{
            for (int i = 0; i<1000; i++)
            {
                service.AddMovie(new Movie
                {
                    Movie_ID = "M" + i,
                    Title = "Back to the Future" + i,
                    Director = "Robert Zemeckis",
                    Genre = "Sci-Fi",
                    Release_Year = 1985,
                    Availability = "Available"
                });
            }
            service.ExportMovies(tempFile);

            var newService = new MovieService();
            newService.ImportMoviesFromJson(tempFile);
            var result = newService.GetAll();
            Assert.Equal(1000,result.Count());
        }
        finally{
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void ImportMovie_DuplicateID_SkipsDuplicate()
    {
        var service = new MovieService();
        var tempFile = Path.GetTempFileName();
        try{
            var movies = new List<Movie>
            { 
                new Movie
                {
                    Movie_ID = "M1",
                    Title = "Back to the Future",
                    Director = "Robert Zemeckis",
                    Genre = "Sci-Fi",
                    Release_Year = 1985,
                    Availability = "Available"
                },
                new Movie
                {
                    Movie_ID = "M1",
                    Title = "Back to the Future 2",
                    Director = "Robert Zemeckis",
                    Genre = "Sci-Fi",
                    Release_Year = 1989,
                    Availability = "Available"
                }
            };
            File.WriteAllText(tempFile, JsonSerializer.Serialize(movies));
            service.ImportMoviesFromJson(tempFile);
            var result = service.GetAll();
            Assert.Single(result);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}

//Tests for borrow and return services
public class BorrowReturnMovieServiceTests
{
    //movie used in the borrow and return tests
    private MovieService BorrowReturnService()
    {
        var service = new MovieService();
        service.AddMovie(new Movie
        {
            Movie_ID = "M1",
            Title = "Back to the Future",
            Director = "Robert Zemeckis",
            Genre = "Sci-Fi",
            Release_Year = 1985,
            Availability = "Available"
        });

        return service;
    }

    //Verifies that borrowing an available movie returns "Borrowed"
    [Fact]
    public void BorrowMovie_AvailableMovie_ReturnsBorrowed()
    {
        var service = BorrowReturnService();
        var result = service.BorrowMovie("M1", "Username");
        Assert.Equal("Borrowed", result);
    }

    //Verifies that borrowing a movie with an invalid ID returns "NotFound"
    [Fact]
    public void BorrowMovie_InvalidID_ReturnsNotFound()
    {
        var service = BorrowReturnService();
        var result = service.BorrowMovie("Invalid", "Username");
        Assert.Equal("NotFound", result);
    }

    //Verifies that borrowing an already borrowed movie adds user to queue and returns "Queued"
    [Fact]
    public void BorrowMovie_AlreadyBorrowed_AddsToQueue()
    {
        var service = BorrowReturnService();
        service.BorrowMovie("M1", "Username");
        var result = service.BorrowMovie("M1", "SecondUsername");
        Assert.Equal("Queued", result);
    }

    //Verifies that returning a movie that has no one in the waiting queue returns "Returned"
    [Fact]
    public void ReturnMovie_NoQueue_ReturnsReturned()
    {
        var service = BorrowReturnService();
        service.BorrowMovie("M1", "Username");
        var result = service.ReturnMovie("M1");
        Assert.Equal("Returned", result);
    }

    //Verifies that returning a movie with an invalid ID returns "NotFound"
    [Fact]
    public void ReturnMovie_InvalidID_ReturnsNotFound()
    {
        var service = BorrowReturnService();
        var result = service.ReturnMovie("Invalid");
        Assert.Equal("NotFound", result);
    }

    //Verifies that returning a movie with a waiting queue assigns the movie to next user, returns "Assigned to..."
    [Fact]
    public void ReturnMovie_WithQueue_AssignsToNextUser()
    {
        var service = BorrowReturnService();
        service.BorrowMovie("M1", "Username");
        service.BorrowMovie("M1", "SecondUsername");
        var result = service.ReturnMovie("M1");
        Assert.StartsWith("Assigned to:", result);
        Assert.Contains("SecondUsername", result);
    }

    //Verifies that when a movie is returned and has a queue, the availability remains "Borrowed"
    [Fact]
    public void ReturnMovie_WithQueue_KeepsMovieBorrowed()
    {
        var service = BorrowReturnService();
        service.BorrowMovie("M1", "Username");
        service.BorrowMovie("M1", "SecondUsername");
        service.ReturnMovie("M1");
        var movie = service.GetAll().First();
        Assert.Equal("Borrowed", movie.Availability);
    }

    //Verifies that users are added to the queue in order
    [Fact]
    public void ReturnMovie_MultipleQueues_IsInOrder()
    {
        var service = BorrowReturnService();
        service.BorrowMovie("M1", "Username1");
        service.BorrowMovie("M1", "Username2");
        service.BorrowMovie("M1", "Username3");
        var result = service.ReturnMovie("M1");
        Assert.Contains("Username2", result);
    }

    //Verifies that borrow history exports to file
    [Fact]
    public void BorrowHistory_ExportRecordHistory()
    {
        var service = BorrowReturnService();
        var tempFile = Path.GetTempFileName();

        try
        {
            service.BorrowMovie("M1", "Username");
            service.ExportBorrowHistory(tempFile);
            var content = File.ReadAllText(tempFile);
            Assert.Contains("User, Username, borrowed M1", content);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }


}