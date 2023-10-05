using AEAQuiz.Classes;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace AEAQuiz.Pages
{
    public partial class GamePage : ContentPage
    {
        private QuizBase quiz;

        private int numberOfQuestions, numberOfRightAswer = 0, numberOfPoints = 0;

        private int currentIndex = 0;

        private List<Button> buttonsToDelete;

        private List<Player> players = new List<Player>();

        private int playerCountIndex = 0;

        public GamePage(string playersJson = null)
        {
            InitializeComponent();

            numberOfQuestions = AppSettings.NumQuestionsSelected;

            if (playersJson != null)
            {
                players = JsonConvert.DeserializeObject<List<Player>>(playersJson);
                nextPlayer = false;
            }

            _ = LoadTriviaQuestion();
        }


        private async Task LoadTriviaQuestion()
        {
            int? catId = null;
            if (AppSettings.CategorySelected != 0) catId = Categories.GetCategoryId(AppSettings.CategorySelected);

            ImageSource old = questionImage.Source;
            var oldWidth = questionImage.WidthRequest;
            var oldHeight = questionImage.HeightRequest;

            // Ändra storlek och sätt spinner
            questionImage.WidthRequest = 70; // eller 50 eller vilken storlek du vill ha för spinnern
            questionImage.HeightRequest = 70;
            questionImage.Source = ImageSource.FromFile("loading_spinner.gif");

            quiz = await QuizFactory.Create(
                AppSettings.SelectedQuestionDB.ToLower(),
                catId,
                (QType)AppSettings.TypeSelected,
                (Difficulty)AppSettings.DifficultySelected,
                numberOfQuestions,
                true);

            questionImage.Source = old;
            questionImage.WidthRequest = oldWidth;
            questionImage.HeightRequest = oldHeight;

            if (quiz == null)
            {
                DebugLabel.Text = "Error occurd: Check your internet";
                return;
            }

            if (quiz.ResponseCode == 4 || quiz.ResponseCode == 1)
            {
                DebugLabel.Text = $"Error occurd: the database didn´t have {numberOfQuestions} questions of your selected choices";
                return;
            }

            NextQuastion();
        }

        private Timer timer;
        private Timer displayTimer;
        private ImageService imageService = new ImageService();

        private bool nextPlayer = true;

        private void NextQuastion()
        {
            if (quiz.Results.Count > 0 && currentIndex < quiz.Results.Count)
            {
                timerProgress.IsVisible = false;
                TimerLable.IsVisible = false;
                if (!nextPlayer)
                {
                    timer?.Dispose();
                    displayTimer?.Dispose();

                    questionImage.IsVisible = false;
                    questonLabel.Text = "";
                    PlayerName.Text = players[playerCountIndex].Name;
                    PlayerName.TextColor = players[playerCountIndex].Color;
                    NextPlayerBtn.IsVisible = true;
                    return;
                }

                if (players.Count > 1)
                {
                    questionImage.IsVisible = true;
                    nextPlayer = false;
                    NextPlayerBtn.IsVisible = false;
                    PlayerName.Text = players[playerCountIndex].Name;
                }

                if (AppSettings.UseTimerToThink)
                {
                    int sec = AppSettings.TimeToThinkSeconds;

                    timer?.Dispose();
                    timer = new Timer(TimerCallback, null, TimeSpan.FromSeconds(sec), Timeout.InfiniteTimeSpan);

                    TimerLable.IsVisible = true;
                    timerProgress.IsVisible = true;
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
                questonLabel.Text = WebUtility.HtmlDecode(quiz.Results[currentIndex].Question) + " (" + PointCount() + " p)";

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


        private void OnNextPlayerClick(object sender, EventArgs e)
        {
            nextPlayer = true;
            NextQuastion();
        }

        void TimerCallback(object state)
        {
            Dispatcher.DispatchAsync(() =>
            {
                if (players.Count > 1)
                {
                    playerCountIndex++;
                }
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
                    if (players.Count > 1)
                    {
                        players[playerCountIndex].NumberOfRightAswer++;
                        players[playerCountIndex].NumberOfPoints += PointCount();
                    }
                    else
                    {
                        numberOfRightAswer++;
                        numberOfPoints += PointCount();
                    }

                    selectedButton.BackgroundColor = Colors.Green;
                    questionImage.Source = ImageSource.FromFile("correct.png");
                    await Task.Delay(500);
                }
                else
                {
                    selectedButton.BackgroundColor = Colors.Red;
                    questionImage.Source = ImageSource.FromFile("fail.png");
                    await Task.Delay(500);
                }

                await ResultOrNextCheck();
            }
        }

        private async Task ResultOrNextCheck()
        {
            if (currentIndex < quiz.Results.Count - 1)
            {
                if (players.Count > 1)
                {
                    playerCountIndex++;
                    if (playerCountIndex >= players.Count)
                    {
                        currentIndex++;
                        playerCountIndex = 0;
                        nextPlayer = false;
                    }
                } 
                else
                {
                    currentIndex++;
                }
                
                buttonsToDelete.ForEach(x => { StackLayoutQ.Remove(x); });
                NextQuastion();
            }
            else
            {
                timer?.Dispose();
                displayTimer?.Dispose();
                string answerText;

                if (players.Count > 1)
                {
                    List<string> answerTexts = new List<string>();
                    foreach (Player player in players)
                    {
                        if ((double)player.NumberOfRightAswer / numberOfQuestions > 0.5) { answerText = "Well done!"; }
                        else { answerText = "Not so good...."; }
                        answerTexts.Add($"Quiz result for {player.Name}: {player.NumberOfRightAswer} right answers of {numberOfQuestions} questions.  Points = {player.NumberOfPoints}");
                    }
                    await Navigation.PushAsync(new ResultPage(string.Join("\n", answerTexts.ToArray())));
                }
                else
                {
                    if ((double)numberOfRightAswer / numberOfQuestions > 0.5) { answerText = "Well done!"; }
                    else { answerText = "Not so good...."; }
                    await Navigation.PushAsync(new ResultPage($"Quiz result: {numberOfRightAswer} right answers of {numberOfQuestions} questions. Points = {numberOfPoints} {answerText}"));
                }    
            }
        }

        private int PointCount()
        {
            //Easy type of question gives 1 point, medium 2 points and hard 3 points    
            int points = 1;
            if (quiz.Results[currentIndex].Difficulty == "medium") points = 2;
            else if (quiz.Results[currentIndex].Difficulty == "hard") points = 3;

            return points;
        }
        private bool CheckAnswer(string answer)
        {
            return answer == quiz.Results[currentIndex].CorrectAnswer;
        }
    }
}


