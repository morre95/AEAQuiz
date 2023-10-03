using AEAQuiz.Classes;
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
        List<Player> players = new();
        for (int i = 1; i < int.MaxValue; i++) 
        {
            string result = await DisplayPromptAsync("Multiplayer", "Name?", accept: "Next", cancel: "Finish", initialValue: $"Player {i}");
            if (result == null)
            {
                break;
            }
            players.Add(new Player(result));
        }

        players.ForEach(p => { Debug.WriteLine(p.Name); });
    }
}