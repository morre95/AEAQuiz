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
                // TODO: Fakturera detta så det blir somma som i OnAnswerButtonClicked(). Det borde fungerar enligt Erik. 
                // Men Ari glöm inte att timer?.Dispose(); raderna om du kodare det så vi slipper minnesproblem, lol
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
                }
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
                    // TODO: Hantera rätt svar
                    // EXEMPEL: results.CorrectAnswer(userId, questionId);
                }
                else
                {
                    selectedButton.BackgroundColor = Colors.Red;
                    await Task.Delay(500);
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
                    string answerText;
                    if ((double)numberOfRightAswer / numberOfQuestions > 0.5) { answerText = "Well done!"; }
                    else { answerText = "Not so good...."; }
                    DebugLabel.Text = $"Quiz result: {numberOfRightAswer} right answers of {numberOfQuestions} questions.  {answerText}";
                    //DebugLabel.Text = "Vann jag???? Vad fick jag för resultat???? Hallå.... svara då!!!!!";
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


