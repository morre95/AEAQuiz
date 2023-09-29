using AEAQuiz.Classes;
using Microsoft.Maui;
using Newtonsoft.Json.Linq;

namespace AEAQuiz.Pages
{
    public partial class GamePage : ContentPage
    {
        private Quiz quiz;
        public GamePage()
        {
            InitializeComponent();

            LoadTriviaQuestion();
        }

        private async void LoadTriviaQuestion()
        {
            int? catId = null;
            if (AppSettings.CategorySelected != 0) catId = Categories.GetCategoryId(AppSettings.CategorySelected);
            quiz = Quiz.Create(
                catId,
                (QType)AppSettings.TypeSelected,
                (Difficulty)AppSettings.DifficultySelected,
                AppSettings.NumQuestionsSelected);
            
            questonLabel.Text = quiz.Results[0].Question;
            LabelDebug.Text = catId.ToString();
            // TODO: Hämta data från class (Array eller list)
            //       api anropas i gamesettingspage då man har alla atribut för apiet där.
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
}


