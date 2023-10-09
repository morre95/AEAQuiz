using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

using AEAQuiz.Classes;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AEAQuiz.Pages
{

    public partial class GameSettingsPage : ContentPage
    {
        private List<Player> players = new();

        public GameSettingsPage(string playersJson = null)
        {
            InitializeComponent();
            if (playersJson != null) 
            {
                players = JsonConvert.DeserializeObject<List<Player>>(playersJson);

                HorizontalStackLayout stack = new HorizontalStackLayout();
                for (int index = 0; index < players.Count; index++)
                {
                    Label playerLabel = new Label()
                    {
                        Text = players[index].Name,
                        TextColor = players[index].Color,
                        Margin = new Thickness(0, 0, 10, 20)
                    };
                    stack.Add(playerLabel);
                    if (index % 3 == 2)
                    {
                        MultiplayerNameStack.Add(stack);
                        stack = new HorizontalStackLayout();
                    }
                    else if (index >= players.Count - 1)
                    {
                        MultiplayerNameStack.Add(stack);
                    }
                }

            }
            
            DificultyPicker.SelectedIndex = AppSettings.DifficultySelected;
            TypePicker.SelectedIndex = AppSettings.TypeSelected;
            NumQuestionsSlider.Value = AppSettings.NumQuestionsSelected;

            BindingContext = new Categories();

            CategoryPicker.SelectedIndex = AppSettings.CategorySelected;

            TimeToThinkSlider.Value = AppSettings.TimeToThinkSeconds;
            UseTimerToThink.IsChecked = AppSettings.UseTimerToThink;
            TimeToThinkSlider.IsEnabled = AppSettings.UseTimerToThink;

            int i = 0;
            foreach(var item in DBPicker.ItemsSource)
            {
                if (item.ToString().ToLower() == AppSettings.SelectedQuestionDB) DBPicker.SelectedIndex = i;
                i++;
            }
        }

        private void OnTimeToThinkValueChanged(object sender, ValueChangedEventArgs e)
        {
            //Avrunda värdet från slidern
            var roundedValue = Math.Round(e.NewValue);

            TimeToThinkSlider.Value = roundedValue;

            var ts = TimeSpan.FromSeconds(roundedValue);
            TimeToThinkValue.Text = ts.ToString("mm':'ss");
            AppSettings.TimeToThinkSeconds = (int)roundedValue;
        }

        private void OnTimerCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox != null)
            {
                AppSettings.UseTimerToThink = UseTimerToThink.IsChecked;
            }

            if (UseTimerToThink.IsChecked == false)
            {
                TimeToThinkSlider.IsEnabled = false;
            }
            else
            {
                TimeToThinkSlider.IsEnabled = true;
            }
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            //Avrunda värdet från slidern
            var roundedValue = Math.Round(e.NewValue);

            NumQuestionsSlider.Value = roundedValue;

            displayLabel.Text = roundedValue.ToString();

            AppSettings.NumQuestionsSelected = (int)NumQuestionsSlider.Value;
        }

        private void Category_IndexChanged(object sender, EventArgs e)
        {
            AppSettings.CategorySelected = CategoryPicker.SelectedIndex;
        }
        
        private void Dificulty_IndexChanged(object sender, EventArgs e)
        {
            AppSettings.DifficultySelected = DificultyPicker.SelectedIndex;
        }
        
        private void Type_IndexChanged(object sender, EventArgs e)
        {
            AppSettings.TypeSelected = TypePicker.SelectedIndex;
        }

        private async void GameBtn_Clicked(object sender, EventArgs e)
        {
            if (players.Count <= 1)
            {
                await Navigation.PushAsync(new GamePage());
            }
            else
            {
                await Navigation.PushAsync(new GamePage(JsonConvert.SerializeObject(players)));
            }
        }

        private void DB_IndexChanged(object sender, EventArgs e)
        {
            AppSettings.SelectedQuestionDB = DBPicker.ItemsSource[DBPicker.SelectedIndex].ToString().ToLower();
        }
    }
}

