using System.DirectoryServices.ActiveDirectory;

namespace Assessment1.Model;
public class Movie { 
	public string Movie_ID {get; set; }
	public string Title { get; set; } 
	public string Director { get; set; } 
	public string Genre { get; set; } 
	public int Release_Year { get; set; } 
	public string Availability { get; set; } 


public Movie(string id, string title, string director, string genre, int year, string availability)
    {
        Movie_ID = id;
        Title = title;
        Director = director;
        Genre = genre;
        Release_Year = year;
        Availability = availability;
    }

public Movie()
    {
        Movie_ID = "";
        Title = "";
        Director = "";
        Genre = "";
        Release_Year = 0;
        Availability = "Available";
    }
}