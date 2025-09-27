using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Test_Canaan.Helpers;
using Test_Canaan.Models;
using Test_Canaan.Services;

namespace Test_Canaan.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly ITourBookingService _bookingService;
    private ObservableCollection<TourBooking> _bookings;
    private TourBooking? _selectedBooking;
    private string _searchText;
    private bool _isLoading;

    public MainViewModel(ITourBookingService bookingService)
    {
        _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        _bookings = new ObservableCollection<TourBooking>();
        _searchText = string.Empty;

        LoadDataCommand = new RelayCommand(async _ => await LoadDataAsync());
        SearchCommand = new RelayCommand(_ => Search());
        ClearSearchCommand = new RelayCommand(_ => ClearSearch());
    }

    public ObservableCollection<TourBooking> Bookings
    {
        get => _bookings;
        set => SetProperty(ref _bookings, value);
    }

    public TourBooking? SelectedBooking
    {
        get => _selectedBooking;
        set => SetProperty(ref _selectedBooking, value);
    }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoadDataCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand ClearSearchCommand { get; }

    private async System.Threading.Tasks.Task LoadDataAsync()
    {
        IsLoading = true;
        try
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            Bookings.Clear();
            foreach (var booking in bookings)
            {
                Bookings.Add(booking);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void Search()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
            return;

        var filtered = Bookings
            .Where(b =>
                (b.DealNumber?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (b.Customer?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (b.ProgramName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();

        Bookings.Clear();
        foreach (var booking in filtered)
        {
            Bookings.Add(booking);
        }
    }

    private void ClearSearch()
    {
        SearchText = string.Empty;
        _ = LoadDataAsync();
    }
}