// Views/EditGamePage.xaml.cs
using Frontend.ViewModels;

namespace Frontend.Views
{
    public partial class EditGamePage : ContentPage
    {
        public EditGamePage(EditGameViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}