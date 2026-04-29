using System.Diagnostics;
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
using System.IO;
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

	private Movie? selectedMovie;
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
			MessageBox.Show("This Movie ID already exists, please choose another");
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
			MessageBox.Show("An unknown error has occurred");
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

	//when the Sort by Year button is clicked 
	private void btnSortYear_Click(object sender, RoutedEventArgs e)
	{
		//run the movie service Merge Sort By Year function and fill the datagrid with the new sorted movie list 
		dtgMovies.ItemsSource = movieService.MergeSortByYear();
	}

	//function to check movie list and search textbox are not empty
	private bool SearchPreCheckSuccess()
	{
		//if movie list is empty, display message and declare pre-check success is false
		if (!movieService.GetAll().Any())
		{
			MessageBox.Show("Please add a movie to the list before searching");
			return false;
		}

		//if nothing is entered into the search textbox, display message and declare pre-check success is false
		if (string.IsNullOrWhiteSpace(txtSearch.Text))
			{ 
				MessageBox.Show("Please enter information into the search field before searching"); 
				return false; 
			} 
		//else, return pre-check success is true
		return true; 
	}

	//when the Search by Title button is clicked
	private void btnSearchTitle_Click(object sender, RoutedEventArgs e)
	{
		//runs Search pre-check function, if it fails then stop search
		if (SearchPreCheckSuccess() == false)
		{
			ClearSearchInput();
			return;
		}
	
		//defines search results
		var results = movieService.LinearSearchByTitle(txtSearch.Text);

		//if there are no movies matching the search filter input, display message and clear datagrid
		if (results.Count == 0)
		{
			dtgMovies.ItemsSource = null;
			MessageBox.Show("No results found");
			ClearSearchInput();
			return;
		}

		//run the movie service Linear Search function and fill the datagrid with filtered search results from the search textbox
		dtgMovies.ItemsSource = results;
		ClearSearchInput();
	}

	//when the Search by ID button is clicked
	private void btnSearchID_Click(object sender, RoutedEventArgs e)
	{
		//runs Search pre-check function, if it fails then stop search
		if (!SearchPreCheckSuccess())
		{
			ClearSearchInput();
			return;
		}

		//defines search results
		var results = movieService.BinarySearchByID(txtSearch.Text);

		//if there are no movies matching the search filter input, display message and clear datagrid
		if (results.Count == 0)
		{
			dtgMovies.ItemsSource = null;
			MessageBox.Show("No results found");
			ClearSearchInput();
			return;
		}

		//run the movie service Binary Search function and fill the datagrid with filtered search results from the search textbox
		dtgMovies.ItemsSource = results;
		ClearSearchInput();
	}

	//clears the search textbox
	private void ClearSearchInput()
	{
		//if the search textbox is not empty
		if(!string.IsNullOrWhiteSpace(txtSearch.Text))
		{
			//clear text from search textbox
			txtSearch.Text= "";
		}
	}

	//when Show All button is clicked
	private void btnShowAll_Click(object sender, RoutedEventArgs e)
	{
		//run refresh grid function
		RefreshGrid();
	}


	//when import button is clicked
	private void btnImport_Click(object sender, RoutedEventArgs e)
	{
		//starts error handling
		try
		{
			//creates file picker window 
			var dialog = new Microsoft.Win32.OpenFileDialog
			{
				//JSON file option only
				Filter = "JSON Files (*.json)|*.json"
			};

			//checks user selected file and clicked open
			if (dialog.ShowDialog() == true)
			{
				//runs import service to load movies into system
				movieService.ImportMoviesFromJson(dialog.FileName);
				//run refresh grid function
				RefreshGrid();
				//Shows success message
				MessageBox.Show("Movies imported successfully!");
			}
		}
		//file error handling
		catch (IOException ex)
		{
   		 	MessageBox.Show("File error: " + ex.Message);
		}
		//permission error handling
		catch (UnauthorizedAccessException ex)
		{
   			MessageBox.Show("You do not have permission to access this file. " + ex.Message);
		}
		//wrong JSON structure error handling 
        catch (System.Text.Json.JsonException) 
        { 
        	MessageBox.Show("Invalid JSON file. Please select a valid Movie List file."); 
        } 
		//empty or invalid movie data error handling
		catch (InvalidDataException)
		{
			MessageBox.Show("Invalid JSON File. Please select a valid Movie List file.");
		}
		//any other exception error handling
		catch (Exception ex)
		{
			MessageBox.Show("Import failed: " + ex.Message);
		}
	}
		
	
	//when export button is clicked
	private void btnExport_Click(object sender, RoutedEventArgs e)
	{
		//starts error handling
		try
		{
			//Opens Save as dialog to choose file location
			var dialog = new Microsoft.Win32.SaveFileDialog
			{
				Filter = "JSON Files (*.json)|*.json",
				FileName = "movies.json"
			};
			//checks user selected a file and clicked open
			if (dialog.ShowDialog() == true)
			{
				//runs export movie service to convert to JSON and save it
				movieService.ExportMovies(dialog.FileName);
				MessageBox.Show("Movies exported successfully!");
			}
		}
		//file error handling
		catch (IOException ex)
		{
   		 MessageBox.Show("File error: " + ex.Message);
		}
		//permission error handling
		catch (UnauthorizedAccessException ex)
		{
   		MessageBox.Show("You do not have permission to access this file. " + ex.Message);
		}
		//any other exception error handling
		catch (Exception ex)
		{
			MessageBox.Show("Export failed: " + ex.Message);
		}
	}

	//when borrow button is clicked
	private void btnBorrow_Click (object sender, RoutedEventArgs e)
	{
		//gets movie from selected movie in datagrid
		selectedMovie = dtgMovies.SelectedItem as Movie;

		//if no movie is selected, displays message
		if(selectedMovie == null)
		{
			MessageBox.Show("Please select a movie to borrow");
			return;
		}
		//changes content in Borrow Panel to selected movie details
		lblBorrowMovieID.Content = selectedMovie.Movie_ID;
    	lblBorrowMovieTitle.Content = selectedMovie.Title;
		lblBorrowMovieAvailability.Content = selectedMovie.Availability;
		
		//if movie is already borrowed
		if (selectedMovie.Availability == "Borrowed")
		{
			//make borrowed message visible
			lblBorrowedMessage.Visibility = Visibility.Visible;
		}
		
		//make borrow panel visible
		borrowPanel.Visibility = Visibility.Visible; 
	}

	//when borrow back button is clicked
	private void btnBorrowBack_Click (object sender, RoutedEventArgs e)
	{
		//run clear borrow panel function
		ClearBorrowPanel();
	}

	//function for clearing details in borrow panel
	private void ClearBorrowPanel()
	{
		//removes selected item
		dtgMovies.SelectedItem = null;
		dtgMovies.UnselectAll();
		//clears borrow movie details content 
		lblBorrowMovieID.Content = "";
    	lblBorrowMovieTitle.Content = "";
		lblBorrowMovieAvailability.Content = "";
		//makes borrowed message not visible
		lblBorrowedMessage.Visibility = Visibility.Collapsed;
		//clears entered username details
		txtUsername.Text = "";
		//makes borrow panel not visible
		borrowPanel.Visibility = Visibility.Collapsed; 
	}

	//when borrow save button is clicked
	public void btnBorrowSave_Click (object sender, RoutedEventArgs e)
	{
		//if no movie is selected, display message and stop
		if (selectedMovie == null)
		{
			MessageBox.Show("No movie selected");
			return;
		}
		//if nothing is entered in username textbox, display message and stop
		if (string.IsNullOrWhiteSpace(txtUsername.Text))
		{
			MessageBox.Show("Please enter a username");
			return;
		}

		//run borrow movie service and set result to Movie ID of Borrowed movie and inputted username
		string result = movieService.BorrowMovie(
			selectedMovie.Movie_ID,
			txtUsername.Text
		);

		//if movie has been borrowed successfully, display message
		if (result == "Borrowed")
		{
			MessageBox.Show("Movie borrowed successfully");
		}
		//if movie has been queued, display message
		else if (result == "Queued")
		{
			MessageBox.Show("You have been added to the waiting queue");
		}

		//run refresh grid function
		RefreshGrid();
		//run clear borrow panel function
		ClearBorrowPanel();
	}

	//when return button is clicked
	public void btnReturn_Click(object sender, RoutedEventArgs e)
	{
		//gets movie from selected movie in datagrid
		selectedMovie = dtgMovies.SelectedItem as Movie;

		//if no movie is selected, displays message
		if(selectedMovie == null)
		{
			MessageBox.Show("Please select a movie to return");
			return;
		}

		//if movie is available, it can't be returned, displays message
		if (selectedMovie.Availability == "Available")
		{
			MessageBox.Show("This movie has not been borrowed, so it can't be returned.");
			return;
		}

		//run return movie service and set result to Movie ID of selected movie
		var result = movieService.ReturnMovie(selectedMovie.Movie_ID);

		//if movie is not found, display message
		if (result == "NotFound")
		{
			MessageBox.Show("Movie not found");
		}

		//if movie has been returned successfully, display message
		else if (result == "Returned")
		{
			MessageBox.Show("Movie returned successfully");
		}
		//if there is a user in a waiting queue for returned movie
		else if (result.StartsWith("Assigned to:"))
		{
			//get the username from the returned result
			string user = result.Split(':')[1].Trim();
			//display message notifying next user
			MessageBox.Show($"Movie assigned to next user in the queue: {user}");
		}
		//run refresh grid function
		RefreshGrid();
	}


	//when export borrow history is clicked
	public void btnExportHistory_Click(object sender, RoutedEventArgs e)
	{	
		//starts error handling
		try{
			//Opens Save as dialog to choose file location
		 	var dialog = new Microsoft.Win32.SaveFileDialog
			{
			Filter = "JSON Files (*.json)|*.json",
			FileName = "borrow_history.json"
			};

			//checks user selected a file and clicked open
			if (dialog.ShowDialog() == true)
			{
				//runs export borrow history service to convert to JSON and save it
				movieService.ExportBorrowHistory(dialog.FileName);
				MessageBox.Show("Borrow history exported successfully!");
			}
		}
		 //file error handling
		catch (IOException ex)
		{
			MessageBox.Show("File error: " + ex.Message);
		}
		//permission error handling
		catch (UnauthorizedAccessException ex)
		{
			MessageBox.Show("You do not have permission to access this file. " + ex.Message);
		}
		//any other exception error handling
		catch (Exception ex)
		{
			MessageBox.Show("Export failed: " + ex.Message);
		}
	}

}

