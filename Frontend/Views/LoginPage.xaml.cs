// Views/LoginPage.xaml.cs
using Frontend.ViewModels;

namespace Frontend.Views
{
    public partial class LoginPage : ContentPage
    {
        // Inject LoginViewModel via Dependency Injection
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}