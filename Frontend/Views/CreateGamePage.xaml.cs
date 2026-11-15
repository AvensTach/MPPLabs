// Views/CreateGamePage.xaml.cs
using Frontend.ViewModels;

namespace Frontend.Views
{
    public partial class CreateGamePage : ContentPage
    {
        public CreateGamePage(CreateGameViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}