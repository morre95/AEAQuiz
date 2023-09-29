using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

using AEAQuiz.Classes;

namespace AEAQuiz.Pages
{

    public partial class GameSettingsPage : ContentPage
    {


        public GameSettingsPage()
        {
            InitializeComponent();
            DificultyPicker.SelectedIndex = AppSettings.DifficultySelected;
            TypePicker.SelectedIndex = AppSettings.TypeSelected;
            NumQuestionsSlider.Value = AppSettings.NumQuestionsSelected;

            BindingContext = new Categories();

            CategoryPicker.SelectedIndex = AppSettings.CategorySelected;
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

        private async void GameBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GamePage());
        }
    }
}

