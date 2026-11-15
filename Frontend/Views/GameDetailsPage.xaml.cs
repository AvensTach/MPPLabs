// Views/GameDetailsPage.xaml.cs
using Frontend.ViewModels;

namespace Frontend.Views
{
    public partial class GameDetailsPage : ContentPage
    {
        public GameDetailsPage(GameDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}