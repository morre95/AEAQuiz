using AEAQuiz.Pages;
namespace AEAQuiz
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SettingsBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
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
