namespace Frontend
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register route for navigation
            Routing.RegisterRoute(nameof(Views.GameDetailsPage), typeof(Views.GameDetailsPage));
            Routing.RegisterRoute(nameof(Views.CreateGamePage), typeof(Views.CreateGamePage));
            Routing.RegisterRoute(nameof(Views.EditGamePage), typeof(Views.EditGamePage));
        }
    }
}