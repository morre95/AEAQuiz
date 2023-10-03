using System.Diagnostics;

namespace AEAQuiz.Pages;

public partial class MutliplayerSettingsPage : ContentPage
{
	public MutliplayerSettingsPage()
	{
		InitializeComponent();
        DisplayPromptAsync();

    }

    async void DisplayPromptAsync()
    {
        string result = await DisplayPromptAsync("Multiplayer", "Name", initialValue: "Player 1");
        if (result == "Cancel")
        {
            Debug.WriteLine("Detta blir bra");
        }

        Debug.WriteLine(result);
    }
}