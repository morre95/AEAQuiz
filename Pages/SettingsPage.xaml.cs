namespace AEAQuiz
{
    public class Settings { }
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            //Avrunda värdet från slidern
            var roundedValue = Math.Round(e.NewValue);

            integerSlider.Value = roundedValue;

            displayLabel.Text = roundedValue.ToString();
            //TODO: 
        }
    }
}

