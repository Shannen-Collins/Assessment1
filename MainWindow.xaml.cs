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

	private void btnAddMovie_Click(object sender, RoutedEventArgs e) 
	{ 
		addPanel.Visibility = Visibility.Visible; 
	} 

	private void btnAddBack_Click(object sender, RoutedEventArgs e) 
	{ 
		addPanel.Visibility = Visibility.Collapsed; 
		txtMovieID.Text = ""; 
		txtTitle.Text = ""; 
		txtDirector.Text = ""; 
		txtGenre.Text = ""; 
		txtYear.Text = ""; 
	} 

}

