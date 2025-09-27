using System.IO;
using System.Windows;
using Test_Canaan.Services;
using Test_Canaan.ViewModels;

namespace Test_Canaan;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "Заезды  - RAW.csv");
        var bookingService = new CsvTourBookingService(csvPath);
        var viewModel = new MainViewModel(bookingService);

        DataContext = viewModel;
    }
}