using AEAQuiz.Classes;

namespace AEAQuiz.Pages
{
    public partial class GamePage : ContentPage
    {
        private Quiz quiz;

        private int numberOfQuestions;

        public GamePage()
        {
            InitializeComponent();

            numberOfQuestions = AppSettings.NumQuestionsSelected;

            LoadTriviaQuestion();
        }


        private async Task LoadTriviaQuestion()
        {
            int? catId = null;
            if (AppSettings.CategorySelected != 0) catId = Categories.GetCategoryId(AppSettings.CategorySelected);
            quiz = await Quiz.Create(
                catId,
                (QType)AppSettings.TypeSelected,
                (Difficulty)AppSettings.DifficultySelected,
                numberOfQuestions);

            NextQuastion();
        }

        private void NextQuastion()
        {
            questonLabel.Text = quiz.Results[numberOfQuestions - 1].Question;

            var answers = new List<string>
            {
                quiz.Results[numberOfQuestions - 1].CorrectAnswer
            };
            answers.AddRange(quiz.Results[numberOfQuestions - 1].IncorrectAnswers);
            //Erik
            //Button btn;
            //Random r = new Random();
            //foreach (string answer in answers.OrderBy(x => r.Next()))
            //{
            //    btn = new Button();
            //    btn.Text = answer;
            //    btn.Clicked += OnAnswerButtonClicked;
            //    StackLayout.Add(btn);
            //}

            ////Svar i samma labels /Amir
            Random r = new Random();
            answers = answers.OrderBy(x => r.Next()).ToList();

            // Uppdatera knapparnas texter istället för att skapa knappar
            answerButton1.Text = answers[0];
            answerButton2.Text = answers[1];
            answerButton3.Text = answers[2];
            answerButton4.Text = answers[3];
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
                bool isCorrect = CheckAnswer(selectedButton.Text);
                if (isCorrect)
                {
                    numberOfQuestions--;
                    // TODO: Hantera när frågorna är slut
                    // TODO: Hantera när nästa fråga ska laddas så inte det bara laddas in fler knappar
                    NextQuastion();
                }
                else
                {
                    // TODO: Hantera fel svar
                }
            }
        }

        private bool CheckAnswer(string answer)
        {
            return answer == quiz.Results[numberOfQuestions - 1].CorrectAnswer;
        }
    }
}


