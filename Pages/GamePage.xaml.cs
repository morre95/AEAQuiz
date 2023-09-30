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
            Button btn;
            Random r = new Random();
            foreach (string answer in answers.OrderBy(x => r.Next()))
            {
                btn = new Button();
                btn.Text = answer;
                btn.Clicked += OnAnswerButtonClicked;
                StackLayoutQ.Add(btn);
            }

            //Svar i samma labels /Amir
            //Random r = new Random();
            //answers = answers.OrderBy(x => r.Next()).ToList();

            // Uppdatera knapparnas texter istället för att skapa knappar

            //answerButton1.Text = answers[0];
            //answerButton2.Text = answers[1];
            //answerButton3.Text = answers[2];
            //answerButton4.Text = answers[3];
            //answerButton1.Clicked += OnAnswerButtonClicked;
            //answerButton2.Clicked += OnAnswerButtonClicked;
            //answerButton3.Clicked += OnAnswerButtonClicked;
            //answerButton4.Clicked += OnAnswerButtonClicked;

            //BUG: Vid val av type true/false så laddas inte frågorna alls! VIKTIGT
            //Såhär har jag tänkt att man ska lösa knapparna beroende på vilken sorts fråga det är
            ///////////////////////////////////////////////////////////////////////////////////////
            /*if (quiz.Results[numberOfQuestions - 1].Type == "boolean")
            {
                //answers.Add("True");
                //answers.Add("False");

                answerButton1.Text = answers[0];
                answerButton2.Text = answers[1];
                answerButton1.Clicked += OnAnswerButtonClicked;
                answerButton2.Clicked += OnAnswerButtonClicked;

                // Dölj de andra knapparna
                answerButton3.IsVisible = false;
                answerButton4.IsVisible = false;
            }
            else // multiple choice
            {
              //  answers.Add(quiz.Results[numberOfQuestions - 1].CorrectAnswer);
              //  answers.AddRange(quiz.Results[numberOfQuestions - 1].IncorrectAnswers);

                Random r = new Random();
                answers = answers.OrderBy(x => r.Next()).ToList();

                answerButton1.Text = answers[0];
                answerButton2.Text = answers[1];
                answerButton3.Text = answers[2];
                answerButton4.Text = answers[3];

                answerButton1.Clicked += OnAnswerButtonClicked;
                answerButton2.Clicked += OnAnswerButtonClicked;
                answerButton3.Clicked += OnAnswerButtonClicked;
                answerButton4.Clicked += OnAnswerButtonClicked;

                // Visa alla knappar
                answerButton3.IsVisible = true;
                answerButton4.IsVisible = true;
            }*/
            ///////////////////////////////////////////////////////////////////////////////////////
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
                    
                    for(int i = 0; i < StackLayoutQ.Count - 1; i++)
                        StackLayoutQ.RemoveAt(i);

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


