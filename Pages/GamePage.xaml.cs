using AEAQuiz.Classes;
using System.Net;

namespace AEAQuiz.Pages
{
    public partial class GamePage : ContentPage
    {
        private Quiz quiz;

        private int numberOfQuestions, numberOfRightAswer = 0;

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
            var oldWidth = questionImage.WidthRequest;
            var oldHeight = questionImage.HeightRequest;

            // Ändra storlek och sätt spinner
            questionImage.WidthRequest = 70; // eller 50 eller vilken storlek du vill ha för spinnern
            questionImage.HeightRequest = 70;
            questionImage.Source = ImageSource.FromFile("loading_spinner.gif");
            quiz = await Quiz.Create(
                catId,
                (QType)AppSettings.TypeSelected,
                (Difficulty)AppSettings.DifficultySelected,
                numberOfQuestions,
                await Token.Get());
            questionImage.Source = old;
            questionImage.WidthRequest = oldWidth;
            questionImage.HeightRequest = oldHeight;
            NextQuastion();
        }

        private Timer timer;
        private Timer displayTimer;
        private ImageService imageService = new ImageService();

        private void NextQuastion()
        {

            if (quiz.Results.Count > 0 && currentIndex < quiz.Results.Count)
            {
                if (AppSettings.UseTimerToThink)
                {
                    int sec = AppSettings.TimeToThinkSeconds;

                    timer?.Dispose();
                    timer = new Timer(TimerCallback, null, TimeSpan.FromSeconds(sec), Timeout.InfiniteTimeSpan);

                    timerProgress.Progress = 0;
                    timerProgress.ProgressTo(1.0, Convert.ToUInt32(sec * 1000), Easing.CubicIn);

                    displayTimer?.Dispose();
                    displayTimer = new Timer((e) =>
                    {
                        Dispatcher.DispatchAsync(() =>
                        {
                            TimerLable.Text = TimeSpan.FromSeconds(sec).ToString("mm':'ss");
                            sec--;
                        });
                    }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
                }

                var currentCategoryName = quiz.Results[currentIndex].Category;
                var currentCategoryId = Categories.GetCategoryIdByName(currentCategoryName);
                questionImage.Source = imageService.GetRandomImageForCategory(currentCategoryId);

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

        void TimerCallback(object state)
        {
            Dispatcher.DispatchAsync(() =>
            {
                _ = ResultOrNextCheck();
            });
        }

        private async void OnAnswerButtonClicked(object sender, EventArgs e)
        {
            var selectedButton = sender as Button;
            if (selectedButton != null)
            {
                bool isCorrect = CheckAnswer(selectedButton.Text);
                if (isCorrect)
                {
                    numberOfRightAswer++;
                    selectedButton.BackgroundColor = Colors.Green;
                    await Task.Delay(500);
                }
                else
                {
                    selectedButton.BackgroundColor = Colors.Red;
                    await Task.Delay(500);
                }

                await ResultOrNextCheck();
            }
        }

        private async Task ResultOrNextCheck()
        {
            if (currentIndex < quiz.Results.Count - 1)
            {
                currentIndex++;
                buttonsToDelete.ForEach(x => { StackLayoutQ.Remove(x); });
                NextQuastion();
            }
            else
            {
                timer?.Dispose();
                displayTimer?.Dispose();
                string answerText;
                if ((double)numberOfRightAswer / numberOfQuestions > 0.5) { answerText = "Well done!"; }
                else { answerText = "Not so good...."; }
                await Navigation.PushAsync(new ResultPage($"Quiz result: {numberOfRightAswer} right answers of {numberOfQuestions} questions.  {answerText}"));
            }
        }

        private bool CheckAnswer(string answer)
        {
            return answer == quiz.Results[currentIndex].CorrectAnswer;
        }
    }
}


