using AEAQuiz.Classes;

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
            // TODO: Vinnare presenteras inte på Android
            var root = new TableRoot(winnerText);

            string answerDetail;
            foreach (var item in message)
            {
                if ((double)item.NumberOfRightAswer / numberOfQuestions > 0.5) { answerDetail = "Well done!"; }
                else { answerDetail = "Not so good...."; }
                
                root.Add(new TableSection(item.Name) {
                        new TextCell {
                            Text = $"{item.NumberOfRightAswer} right answers of {numberOfQuestions} questions. Points = {item.NumberOfPoints}",
                            Detail = answerDetail
                        }
                });
            }
            var tabel = new TableView()
            {
                Intent = TableIntent.Data,
                Root = root
            };

            ResultStack.Add(tabel);
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