namespace AEAQuiz.Pages;

public partial class GameSettingsPage : ContentPage
{
	public GameSettingsPage()
	{
		InitializeComponent();
        DificultyPicker.SelectedIndex = 0;
        TypePicker.SelectedIndex = 0;
        NumQuestionsSlider.Value = 15;

    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        //Avrunda värdet från slidern
        var roundedValue = Math.Round(e.NewValue);

        NumQuestionsSlider.Value = roundedValue;

        displayLabel.Text = roundedValue.ToString();
        //TODO: 
    }

    private async void GameBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GamePage());
    }
}