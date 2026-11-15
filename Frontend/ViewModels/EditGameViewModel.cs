// ViewModels/EditGameViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Frontend.Models;
using Frontend.Services;

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

        [ObservableProperty]
        private string _publisherId = string.Empty;

        // Triggered when GameToEdit is set by Shell navigation
        partial void OnGameToEditChanged(Game? value)
        {
            if (value != null)
            {
                GameTitle = value.Title;
                Genre = value.Genre;
                Price = value.Price.ToString("F2");
                PublisherId = value.PublisherId.ToString();
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

            if (!decimal.TryParse(Price, out decimal priceValue) || !int.TryParse(PublisherId, out int publisherIdValue))
            {
                await Shell.Current.DisplayAlert("Error", "Invalid Price or Publisher ID format.", "OK");
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
                    PublisherId = publisherIdValue
                };

                bool success = await _apiService.UpdateGameAsync(GameToEdit.Id, updatedData);

                if (success)
                {
                    // Go back to the main catalog (double back to refresh)
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