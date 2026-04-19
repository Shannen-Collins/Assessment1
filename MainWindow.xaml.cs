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
using Assessment1.Model;
using Assessment1.Services;


namespace Assessment1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private MovieService movieService;
    public MainWindow()
    {
        InitializeComponent(); 
		movieService = new MovieService(); 
		RefreshGrid();
    }

	private void RefreshGrid()  
	{  
		dtgMovies.ItemsSource = null;  
		dtgMovies.ItemsSource = movieService.GetAll().ToList();  
	} 

	private void ClearAddMovieInputs()
	{
		addPanel.Visibility = Visibility.Collapsed; 
		txtMovieID.Text = ""; 
		txtTitle.Text = ""; 
		txtDirector.Text = ""; 
		txtGenre.Text = ""; 
		txtYear.Text = ""; 
	}

	private void btnAddMovie_Click(object sender, RoutedEventArgs e) 
	{ 
		addPanel.Visibility = Visibility.Visible; 
	} 

	private void btnAddBack_Click(object sender, RoutedEventArgs e) 
	{ 
		ClearAddMovieInputs();
	} 

	private void btnAddSave_Click(object sender, RoutedEventArgs e)
	{ 
		if (string.IsNullOrWhiteSpace(txtMovieID.Text) || 
			string.IsNullOrWhiteSpace(txtTitle.Text) || 
			string.IsNullOrWhiteSpace(txtDirector.Text) || 
			string.IsNullOrWhiteSpace(txtGenre.Text) || 
			string.IsNullOrWhiteSpace(txtYear.Text)) 
			{ 
				MessageBox.Show("Please fill in all details"); 
				return; 
			} 
			
		if (!int.TryParse(txtYear.Text, out int year)) 
		{ 
			MessageBox.Show("Please enter a valid year"); 
			return; 
		} 

		Movie movie = new Movie() 
		{ 
			Movie_ID = txtMovieID.Text, 
			Title = txtTitle.Text, 
			Director = txtDirector.Text, 
			Genre = txtGenre.Text, 
			Release_Year = year, 
			Availability = "Available" 
		}; 

		string result = movieService.AddMovie(movie);

		if (result == "DuplicateID")
		{
			MessageBox.Show("This Movie ID alreadt exists, please chose another");
			return;
		}

		if (result == "InvalidYear")
		{
			MessageBox.Show("Please enter a valid year");
			return;
		}

		if (result != "Success")
		{	
			MessageBox.Show("An unknown error has occured");
			return;
		}

		RefreshGrid();
		ClearAddMovieInputs();
	}

}

