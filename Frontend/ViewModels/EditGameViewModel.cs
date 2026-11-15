using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Frontend.Models;
using Frontend.Services;
using System.Collections.ObjectModel;

namespace Frontend.ViewModels
{
    [QueryProperty(nameof(GameToEdit), "Game")]
    public partial class EditGameViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        public EditGameViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Edit Game";
        }

        [ObservableProperty]
        private Game? _gameToEdit;

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

                // Auto-select the existing publisher for the game being edited
                if (GameToEdit != null)
                {
                    SelectedPublisher = Publishers.FirstOrDefault(p => p.Name == GameToEdit.PublisherName);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load publishers: {ex.Message}", "OK");
            }
        }

        partial void OnGameToEditChanged(Game? value)
        {
            if (value != null)
            {
                GameTitle = value.Title;
                Genre = value.Genre;
                Price = value.Price.ToString("F2");
                // SelectedPublisher is handled after publishers are loaded
            }
        }

        [RelayCommand]
        private async Task SaveChangesAsync()
        {
            if (GameToEdit == null) return;

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

            if (!decimal.TryParse(Price, out decimal priceValue))
            {
                await Shell.Current.DisplayAlert("Error", "Invalid Price format.", "OK");
                return;
            }

            IsBusy = true;

            try
            {
                var updatedData = new CreateGameDto
                {
                    Title = GameTitle,
                    Genre = Genre,
                    Price = priceValue,
                    PublisherId = SelectedPublisher.Id // Use the ID from the selected object
                };

                bool success = await _apiService.UpdateGameAsync(GameToEdit.Id, updatedData);

                if (success)
                {
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to update. You might not have permission.", "OK");
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
    }
}