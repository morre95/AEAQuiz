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
            var winnerLabel = new Label
            {
                Text = winnerText,
                FontSize = 18,
                Margin = new Thickness(16, 5, 16, 10),
                LineBreakMode = LineBreakMode.WordWrap
            };
            ResultStack.Children.Add(winnerLabel);
            var root = new TableRoot() 
            { 
                TextColor = Colors.White
            };

            string answerDetail;
            foreach (var item in message)
            {
                if ((double)item.NumberOfRightAswer / numberOfQuestions > 0.5) { answerDetail = "Well done!"; }
                else { answerDetail = "Not so good...."; }

                var tableSection = new TableSection(item.Name);
                tableSection.TextColor = item.Color;
                tableSection.Add(new TextCell
                {
                    Text = $"{item.NumberOfRightAswer} right answers of {numberOfQuestions} questions. Points = {item.NumberOfPoints}",
                    Detail = answerDetail,
                    DetailColor = Colors.White,
                    TextColor = Colors.White
                });


                root.Add(tableSection);
            }
            var tabel = new TableView()
            {
                Intent = TableIntent.Data,
                Root = root
            };

            ResultStack.Children.Add(tabel);
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