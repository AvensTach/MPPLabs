// ViewModels/CreateGameViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Frontend.Models;
using Frontend.Services;
using System.Collections.ObjectModel;

namespace Frontend.ViewModels
{
    public partial class CreateGameViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        public CreateGameViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Add New Game";
        }

        [ObservableProperty]
        private string _gameTitle = string.Empty;

        [ObservableProperty]
        private string _genre = string.Empty;

        [ObservableProperty]
        private string _price = string.Empty;

        public ObservableCollection<Publisher> Publishers { get; } = new();

        [ObservableProperty]
        private Publisher? _selectedPublisher;

        [RelayCommand]
        private async Task SaveAsync()
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(GameTitle) || string.IsNullOrWhiteSpace(Genre))
            {
                await Shell.Current.DisplayAlert("Error", "Title and Genre are required.", "OK");
                return;
            }

            if (SelectedPublisher == null)
            {
                await Shell.Current.DisplayAlert("Error", "Please select a publisher.", "OK");
                return;
            }

            // Parse price safely
            if (!decimal.TryParse(Price, out decimal priceValue))
            {
                await Shell.Current.DisplayAlert("Error", "Invalid Price format.", "OK");
                return;
            }

            IsBusy = true;

            try
            {
                var newGame = new CreateGameDto
                {
                    Title = GameTitle,
                    Genre = Genre,
                    Price = priceValue,
                    PublisherId = SelectedPublisher.Id
                };

                bool success = await _apiService.CreateGameAsync(newGame);

                if (success)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to create game. Check input or backend permissions.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task LoadPublishersAsync()
        {
            try
            {
                var publishers = await _apiService.GetPublishersAsync();
                Publishers.Clear();
                foreach (var pub in publishers)
                {
                    Publishers.Add(pub);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load publishers: {ex.Message}", "OK");
            }
        }
    }
}