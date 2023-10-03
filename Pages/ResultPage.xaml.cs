namespace AEAQuiz.Pages;

public partial class ResultPage : ContentPage
{
    public ResultPage(string message)
    {
        InitializeComponent();
        result.Text = message;
    }

    async void PlayAgainClick(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    void QuitClick(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }
}