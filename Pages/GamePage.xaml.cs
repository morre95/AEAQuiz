namespace AEAQuiz.Pages;


public partial class GamePage : ContentPage
{
    public GamePage()
    {
        InitializeComponent();

        // TODO: Anropa metoden för att hämta fråga och svar från API när sidan laddas
        LoadTriviaQuestion();
    }

    private async void LoadTriviaQuestion()
    {
        // TODO: Hämta data från API
        // Exempel:
        // var triviaData = await apiService.GetTriviaData();

        // TODO: Uppdatera questonLabel.Text med den hämtade frågan
        // TODO: Uppdatera svarsknapparnas texter med de hämtade svaren

        // Binda händelsehanterare till knapparna för att hantera svar
        answerButton1.Clicked += OnAnswerButtonClicked;
        answerButton2.Clicked += OnAnswerButtonClicked;
        answerButton3.Clicked += OnAnswerButtonClicked;
        answerButton4.Clicked += OnAnswerButtonClicked;
    }

    private void OnAnswerButtonClicked(object sender, EventArgs e)
    {
        var selectedButton = sender as Button;
        if (selectedButton != null)
        {
            // TODO: Rätta den svarade frågan baserat på vilken knapp som klickades
            // Exempel:
            // bool isCorrect = CheckAnswer(selectedButton.Text);
            // if (isCorrect) { /* Hantera rätt svar */ }
            // else { /* Hantera fel svar */ }
        }
    }
}