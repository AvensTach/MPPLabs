using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Frontend.Services;

namespace Frontend.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        public LoginViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Login";
        }

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter username and password.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var success = await _apiService.LoginAsync(Username, Password);
                if (success)
                {
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    ErrorMessage = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Connection error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter username and password to register.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var success = await _apiService.RegisterAsync(Username, Password);
                if (success)
                {
                    // Notify user on success
                    await Shell.Current.DisplayAlert("Success", "Registration successful! You can now log in.", "OK");

                    // Clear password field for security before they log in
                    Password = string.Empty;
                }
                else
                {
                    ErrorMessage = "Registration failed. Username might already exist.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Connection error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}