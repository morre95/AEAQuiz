using AEAQuiz.Classes;
using System.Net;

namespace AEAQuiz.Pages
{
    public partial class GamePage : ContentPage
    {
        private Quiz quiz;

        private int numberOfQuestions;

        private int currentIndex = 0;

        private List<Button> buttonsToDelete;

        public GamePage()
        {
            InitializeComponent();

            numberOfQuestions = AppSettings.NumQuestionsSelected;

            _ = LoadTriviaQuestion();
        }


        private async Task LoadTriviaQuestion()
        {
            int? catId = null;
            if (AppSettings.CategorySelected != 0) catId = Categories.GetCategoryId(AppSettings.CategorySelected);
            // TODO: Spara token i Preferences.Default.Set() och ett datum som kontrolleras förnyas efter 6 timmar 
            // TODO: Create är ett ganksa vilseledande namn. Det borde vara Fetch() eller Get() eller likande
            ImageSource old = questionImage.Source;
            questionImage.Source = ImageSource.FromFile("loading_spinner.gif");
            quiz = await Quiz.Create(
                catId,
                (QType)AppSettings.TypeSelected,
                (Difficulty)AppSettings.DifficultySelected,
                numberOfQuestions,
                await Token.Get());
            questionImage.Source = old;
            NextQuastion();
        }


        private void NextQuastion()
        {
            if (quiz.Results.Count > 0 && currentIndex < quiz.Results.Count)
            {
                // TODO: Någon slags timer bör startas här så att användaren inte har för lång betänke tid
                // EXEMPEL: 
                /*if (AppSettings.UseTimerToThink)
                {
                    int sec = AppSettings.TimeToThinkSeconds;
                    var myTimer = new Timer((e) =>
                    {
                        TimerLable.Text = TimeSpan.FromSeconds(sec).ToString("mm':'ss");
                        sec--;
                    }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
                }*/


                //questonLabel.Text = quiz.Results[currentIndex].Question;
                questonLabel.Text = WebUtility.HtmlDecode(quiz.Results[currentIndex].Question);

                var answers = new List<string>
                {
                    quiz.Results[currentIndex].CorrectAnswer
                };
                answers.AddRange(quiz.Results[currentIndex].IncorrectAnswers);


                buttonsToDelete = new List<Button>();
                Button btn;
                Random r = new Random();
                foreach (string answer in answers.OrderBy(x => r.Next()))
                {
                    btn = new Button();
                    btn.Text = WebUtility.HtmlDecode(answer);
                    btn.Clicked += OnAnswerButtonClicked;
                    StackLayoutQ.Add(btn);
                    buttonsToDelete.Add(btn);
                }

            }


        }

        private void OnAnswerButtonClicked(object sender, EventArgs e)
        {
            var selectedButton = sender as Button;
            if (selectedButton != null)
            {
                bool isCorrect = CheckAnswer(selectedButton.Text);
                if (isCorrect)
                {
                    // TODO: Hantera rätt svar
                    // EXEMPEL: results.CorrectAnswer(userId, questionId);
                }
                else
                {
                    // TODO: Hantera fel svar
                    // EXEMPEL: results.IncorrectAnswer(userId, questionId, (answer: default = "F you"));
                }

                if (currentIndex < quiz.Results.Count - 1)
                {
                    currentIndex++;
                    buttonsToDelete.ForEach(x => { StackLayoutQ.Remove(x); });
                    NextQuastion();
                }
                else
                {
                    DebugLabel.Text = "Vann jag???? Vad fick jag för resultat???? Hallå.... svara då!!!!!";
                    // TODO: Hantera när frågorna är slut
                    // EXAMPLE: if (numberOfQuestions <= 0) results.Save() och sedan skicka användaren tillbaka till en resultat sida
                    // med typ: await Navigation.PushAsync(new ResaultPage(results)); eller liknande
                }
            }
        }

        private bool CheckAnswer(string answer)
        {
            return answer == quiz.Results[currentIndex].CorrectAnswer;
        }
    }
}


