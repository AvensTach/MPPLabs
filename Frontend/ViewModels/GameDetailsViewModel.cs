using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Frontend.Models;
using Frontend.Services;

namespace Frontend.ViewModels
{
    // Receive the Game object and assign it to SelectedGame
    [QueryProperty(nameof(SelectedGame), "Game")]
    public partial class GameDetailsViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        public GameDetailsViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        // Rename from _game to _selectedGame to avoid type conflict, and make it nullable (?)
        [ObservableProperty]
        private Game? _selectedGame;

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (SelectedGame == null) return;

            bool answer = await Shell.Current.DisplayAlert(
                "Confirm Delete",
                $"Are you sure you want to delete {SelectedGame.Title}?",
                "Yes",
                "No");

            if (!answer) return;

            IsBusy = true;

            try
            {
                bool success = await _apiService.DeleteGameAsync(SelectedGame.Id);

                if (success)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "You don't have permission to delete this game.", "OK");
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
            // Shell navigation ".." means go up one level (back)
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task GoToEditAsync()
        {
            if (SelectedGame == null) return;

            var parameters = new Dictionary<string, object>
            {
                { "Game", SelectedGame }
            };

            await Shell.Current.GoToAsync(nameof(Views.EditGamePage), true, parameters);
        }
    }
}