using Xunit;
using Assessment1.Model;
using Assessment1.Services;

namespace Assessment1.Tests;

//Test to see if xUnit tests are working
public class SmokeTests
{
    [Fact]
    public void TestFramework_IsWorking()
    {
        Assert.True(true);
    }
}

//Tests for the Add Movie services
public class AddMovieServiceTests
{
    //Test to see if movie list starts empty
    [Fact] 
    public void GetAll_StartEmpty_ReturnsNoMovies() 
    { 
        var service = new MovieService(); 
        var result = service.GetAll(); 
        Assert.Empty(result); 
    } 

    //Tests if movie can be added to movie list
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

    //Tests that service will return duplicate ID if IDs are the same
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

    //Tests that service will return Invalid year if year is too old for a movie to be made
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

     //Tests that service will return Invalid year if year is in the future, out of boundary
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

     //Tests that service will return Success if the year is the latest boundary year, the present year
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

     //Tests that service will return Success if year is the earliest boundary year of 1888
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

     //Tests that service will return Invalid year if year is out of boundary, one year earlier than earliest boundary year
    [Fact]
    public void AddMovie_InValidYear_JustOutOfBoundary_1887()
    {
        var service = new MovieService(); 
        var movie = new Movie 
        {
            Movie_ID = "M7",
            Title = "One Year Eariler: Roundhay Garden Scene",
            Director = "Louis Le Prince",
            Genre = "Short Film",
            Release_Year = 1887,
            Availability = "Available"
        };
        var result = service.AddMovie(movie);
        Assert.Equal("InvalidYear", result);
    }

    //Stress test that sees if the Add Movie service can handle a lot (1000) of Movies
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
    //Makes sure the Bubble sort arranges the movies by title
    [Fact] 
    public void BubbleSortByTitle_Works() 
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

    //Makes sure the merge sort arrages the movies by year released 
    [Fact] 
    public void MergeSortByYear_Works() 
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

public class SearchMovieServiceTests
{
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
    [Fact]
    public void LinearSearchByTitle_Works()
    {
        var service = SearchServiceMovies();
        var result = service.LinearSearchByTitle("Back");
        Assert.Single(result);
        Assert.Equal("M13", result.First!.Value.Movie_ID);
    }

    [Fact]
    public void LinearSearchByTitle_Works_NotCaseSensitive()
    {
        var service = SearchServiceMovies();
        var result = service.LinearSearchByTitle("star");
        Assert.Single(result);
        Assert.Equal("M14", result.First!.Value.Movie_ID);
    }

    [Fact]
    public void LinearSearchByTitle_NoResultsFound()
    {
        var service = SearchServiceMovies();
        var result = service.LinearSearchByTitle("Fake Movie");
        Assert.Empty(result);
    }

    [Fact]
    public void BinarySearchBID_Works()
    {
        var service = SearchServiceMovies();
        var result = service.BinarySearchByID("M13");
        Assert.Single(result);
        Assert.Equal("M13", result.First!.Value.Movie_ID);
    }

    [Fact]
    public void BinarySearchBID_NotFound_ReturnsEmpty()
    {
        var service = SearchServiceMovies();
        var result = service.BinarySearchByID("M80");
        Assert.Empty(result);
    }

     [Fact]
    public void BinarySearchBID_Works_NotCaseSensitive()
    {
        var service = SearchServiceMovies();
        var result = service.BinarySearchByID("m13");
        Assert.Single(result);
        Assert.Equal("M13", result.First!.Value.Movie_ID);
    }

    
}
