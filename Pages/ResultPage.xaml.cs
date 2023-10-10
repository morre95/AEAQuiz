namespace AEAQuiz.Pages
{

    public partial class ResultPage : ContentPage
    {
        public ResultPage(string message)
        {
            InitializeComponent();
            result.Text = message;
        }

        public ResultPage(string winnerText, List<dynamic> message, int numberOfQuestions)
        {
            InitializeComponent();

            var sortedResults = message.OrderByDescending(item => item.NumberOfPoints).ToList();


            var winnerLabel = new Label
            {
                Text = $"Winner is: {sortedResults.First().Name} with {sortedResults.First().NumberOfPoints} points!",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center
            };
            //ResultStack.Children.Add(winnerLabel);

            var root = new TableRoot(winnerText);

            foreach (var item in sortedResults)
            {
                string answerDetail = (double)item.NumberOfRightAswer / numberOfQuestions > 0.5 ? "Well done!" : "Not so good....";

                root.Add(new TableSection(item.Name) {
                        new TextCell {
                            Text = $"{item.NumberOfRightAswer} right answers of {numberOfQuestions} questions. Points = {item.NumberOfPoints}",
                            Detail = answerDetail
                        }
                });
            }

            var table = new TableView()
            {
                Intent = TableIntent.Data,
                Root = root
            };
            ResultStack.Children.Add(table);
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
}