// ViewModels/MainViewModel.cs
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Frontend.Models;
using Frontend.Services;

namespace Frontend.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        // ObservableCollection automatically updates the UI when items are added or removed
        public ObservableCollection<Game> Games { get; } = new();

        public MainViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Games Catalog";
        }

        [RelayCommand]
        private async Task LoadGamesAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                // Clear the current list
                Games.Clear();

                // Fetch games from API
                var games = await _apiService.GetGamesAsync();

                // Add games to the ObservableCollection
                foreach (var game in games)
                {
                    Games.Add(game);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load games: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToDetailsAsync(Game game)
        {
            if (game == null) return;

            // Pass the selected game object to the details page
            var navigationParameter = new Dictionary<string, object>
            {
                { "Game", game }
            };

            await Shell.Current.GoToAsync(nameof(Views.GameDetailsPage), true, navigationParameter);
        }

        [RelayCommand]
        private async Task GoToCreateAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.CreateGamePage));
        }
    }
}