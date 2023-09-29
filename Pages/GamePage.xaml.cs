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

            Button btn;
            Random r = new Random();
            foreach (string answer in answers.OrderBy(x => r.Next()))
            {
                btn = new Button();
                btn.Text = answer;
                btn.Clicked += OnAnswerButtonClicked;
                StackLayout.Add(btn);
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


