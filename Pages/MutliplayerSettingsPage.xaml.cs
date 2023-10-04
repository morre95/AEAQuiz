using AEAQuiz.Classes;
using System.Diagnostics;
using Newtonsoft.Json;

namespace AEAQuiz.Pages;

public partial class MutliplayerSettingsPage : ContentPage
{
    private List<Player> players = new();
    private int playersCount = 1;
    public MutliplayerSettingsPage()
	{
		InitializeComponent();
    }

    private void NextPlayerClick(object sender, EventArgs e)
    {
        var player = sender as Button;
        if (player != null)
        {
            playersCount++;
            Color color = MyColors.GetColorBy(playersCount);
            players.Add(new Player(playerName.Text, color));
            playerName.Text = "Player " + playersCount;
        }
    }

    private async void FinnishPlayerClick(object sender, EventArgs e)
    {
        var player = sender as Button;
        if (player != null)
        {
            players.Add(new Player(playerName.Text, MyColors.GetColorBy(playersCount)));

            if (players.Count < 2)
            {
                DebugLabel.Text = $"Player count is only {players.Count}\nPlay single mode instead";
            }
            else
            {
                await Navigation.PushAsync(new GameSettingsPage(JsonConvert.SerializeObject(players)));
            }
        }
    }
}