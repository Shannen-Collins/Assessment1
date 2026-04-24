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
	//service layer object for managing movies
	private MovieService movieService;
    public MainWindow()
    {
		//links code to XAML file
        InitializeComponent(); 
		//creates new Movie service instance
		movieService = new MovieService(); 
		//runs refresh grid method
		RefreshGrid();
    }

	//function to refresh the datagrid
	private void RefreshGrid()  
	{  	
		//empties datagrid 
		dtgMovies.ItemsSource = null;  
		//fills datagrid with updated movie list
		dtgMovies.ItemsSource = movieService.GetAll().ToList();  
	} 

	//function to clear inputs and hide Add Movie panel
	private void ClearAddMovieInputs()
	{
		//makes the add movie panel disappear 
		addPanel.Visibility = Visibility.Collapsed; 
		//clears all test in add movie panel
		txtMovieID.Text = ""; 
		txtTitle.Text = ""; 
		txtDirector.Text = ""; 
		txtGenre.Text = ""; 
		txtYear.Text = ""; 
	}

	//when Add Movie button is clicked
	private void btnAddMovie_Click(object sender, RoutedEventArgs e) 
	{ 	
		//makes the add movie panel disappear 
		addPanel.Visibility = Visibility.Visible; 
	} 

	//when Add Movie panel's Back button is clicked
	private void btnAddBack_Click(object sender, RoutedEventArgs e) 
	{ 
		//runs clear add movie inputs method
		ClearAddMovieInputs();
	} 

	//when Add Movie panel's Save button is clicked
	private void btnAddSave_Click(object sender, RoutedEventArgs e)
	{ 
		//if the textboxes are empty, display message
		if (string.IsNullOrWhiteSpace(txtMovieID.Text) || 
			string.IsNullOrWhiteSpace(txtTitle.Text) || 
			string.IsNullOrWhiteSpace(txtDirector.Text) || 
			string.IsNullOrWhiteSpace(txtGenre.Text) || 
			string.IsNullOrWhiteSpace(txtYear.Text)) 
			{ 
				MessageBox.Show("Please fill in all details"); 
				return; 
			} 

		//if input year doesn't convert to integer, display message
		if (!int.TryParse(txtYear.Text, out int year)) 
		{ 
			MessageBox.Show("Please enter a valid year"); 
			return; 
		} 

		//create new movie from input text, movie made available by default
		Movie movie = new Movie() 
		{ 
			Movie_ID = txtMovieID.Text, 
			Title = txtTitle.Text, 
			Director = txtDirector.Text, 
			Genre = txtGenre.Text, 
			Release_Year = year, 
			Availability = "Available" 
		}; 

		//set movie service returns as result 
		string result = movieService.AddMovie(movie);

		//if input Movie ID already exists in movie list, display message
		if (result == "DuplicateID")
		{
			MessageBox.Show("This Movie ID alreadt exists, please chose another");
			return;
		}

		//if input release year is invalid, display message
		if (result == "InvalidYear")
		{
			MessageBox.Show("Please enter a valid year");
			return;
		}

		//if the result is anything else that is not a success, display unknown error message
		if (result != "Success")
		{	
			MessageBox.Show("An unknown error has occured");
			return;
		}

		//runs refresh grid method
		RefreshGrid();
		//runs clear add movie inputs method
		ClearAddMovieInputs();
	}

	//when the Sort by Title button is clicked 
	private void btnSortTitle_Click(object sender, RoutedEventArgs e)
	{
		//run the movie service Bubble Sort By Title function and fill the datagrid with the new sorted movie list 
		dtgMovies.ItemsSource = movieService.BubbleSortByTitle();
	}

}

