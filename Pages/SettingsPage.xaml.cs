using AEAQuiz.Classes;

namespace AEAQuiz
{
    public class Settings { }
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            TimeToThinkSlider.Value = AppSettings.TimeToThinkSeconds;
            UseTimerToThink.IsChecked = AppSettings.UseTimerToThink;
            TimeToThinkSlider.IsEnabled = AppSettings.UseTimerToThink;
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
    }
}

