using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assessment1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private LinkedList<Movie> movies = new LinkedList<Movie>(); 
    public MainWindow()
    {
        InitializeComponent();
		SampleData();  
		dtgMovies.ItemsSource = movies; 
    }

	public IEnumerable<Movie> GetAll() 
	{ 
		return movies; 
	} 

	private void SampleData()
	{ 
		movies.AddLast(new Movie() { ID = "M1", Title = "Back to the Future", Director = "Robert Zemeckis", Genre = "Sci-Fi", Year = 1985, Availability = "Available"}); 
		movies.AddLast(new Movie() { ID = "M2", Title = "Star Wars", Director = "George Lucas", Genre = "Sci-Fi", Year = 1977, Availability = "Available"}); 
		movies.AddLast(new Movie() { ID = "M3", Title = "That Darn Cat!", Director = "Robert Stevenson", Genre = "Comedy", Year = 1965, Availability = "Available"}); 
		movies.AddLast(new Movie() { ID = "M4", Title = "The Final Countdown", Director = "Don Taylor", Genre = "Sci-Fi", Year = 1980, Availability = "Available"}); 

	} 
}

public class Movie { 
	public string ID { get; set; } 
	public string Title { get; set; } 
	public string Director { get; set; } 
	public string Genre { get; set; } 
	public int Year { get; set; } 
	public string Availability { get; set; } 
} 