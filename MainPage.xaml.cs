using AEAQuiz.Pages;
namespace AEAQuiz
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void AboutBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }

        private async void SinglePLayerBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GameSettingsPage());
        }

        private async void MultiplayerBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MutliplayerSettingsPage());
        }
    }
}
