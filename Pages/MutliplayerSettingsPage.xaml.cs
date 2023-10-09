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
            Color color = MyColors.GetColorBy(playersCount - 1);
            players.Add(new Player(playerName.Text, color));
            playersCount++;
            playerName.Text = "Player " + playersCount;
            playerName.Focus();

            //PlayersListLabel.Text = "Players added:\n" + string.Join(", ", players.Select(p => p.Name).ToArray());
            PlayersListLabel.Text = "Players added:\n";

            for (int i = 0; i < players.Count; i++)
            {
                PlayersListLabel.Text += players[i].Name;
                if (i % 3 == 2) 
                {
                    PlayersListLabel.Text += "\n";
                }
                else if (i < players.Count - 1)
                {
                    PlayersListLabel.Text += ", ";
                }
            }

        }
    }

    private async void FinnishPlayerClick(object sender, EventArgs e)
    {
        var player = sender as Button;
        if (player != null)
        {
            //players.Add(new Player(playerName.Text, MyColors.GetColorBy(playersCount - 1)));

            if (players.Count < 2)
            {
                DebugLabel.Text = $"Player count is only {players.Count}\nPlay single mode instead";
            }
            else
            {
                // TODO: Andriod stänger inte tangentbordet 
                await Navigation.PushAsync(new GameSettingsPage(JsonConvert.SerializeObject(players)));
            }
        }
    }

    private async void OnEntryFocus(object sender, FocusEventArgs e)
    {
        var entry = sender as Entry;

        entry.CursorPosition = 0;
        entry.SelectionLength = entry.Text == null ? 0 : entry.Text.Length;
    }
}