using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AEAQuiz.Pages
{


    public class AppSettings
    {
        public static int CategorySelected
        {
            get { return Preferences.Default.Get(nameof(CategorySelected), 0); }
            set { Preferences.Default.Set(nameof(CategorySelected), value); }
        }

        public static int NumQuestionsSelected
        { 
            get{ return Preferences.Default.Get(nameof(NumQuestionsSelected), 15); }
            set { Preferences.Default.Set(nameof(NumQuestionsSelected), value); }
        }

        public static int DifficultySelected
        {
            get { return Preferences.Default.Get(nameof(DifficultySelected), 0); }
            set { Preferences.Default.Set(nameof(DifficultySelected), value); }
        }

        public static int TypeSelected
        {
            get { return Preferences.Default.Get(nameof(TypeSelected), 0); }
            set { Preferences.Default.Set(nameof(TypeSelected), value); }
        }
    }

    public class Categories
    {
        private Dictionary<string, string> _category = new Dictionary<string, string>()
        {
            { "any", "Any Category" },
            { "9", "General Knowledge" },
            { "10", "Entertainment: Books" },
            { "11", "Entertainment: Film" },
            { "12", "Entertainment: Music" },
            { "13", "Entertainment: Musicals & Theatres" },
            { "14", "Entertainment: Television" },
            { "15", "Entertainment: Video Games" },
            { "16", "Entertainment: Board Games" },
            { "17", "Science & Nature" },
            { "18", "Science: Computers" },
            { "19", "Science: Mathematics" },
            { "20", "Mythology" },
            { "21", "Sports" },
            { "22", "Geography" },
            { "23", "History" },
            { "24", "Politics" },
            { "25", "Art" },
            { "26", "Celebrities" },
            { "27", "Animals" },
            { "28", "Vehicles" },
            { "29", "Entertainment: Comics" },
            { "30", "Science: Gadgets" },
            { "31", "Entertainment: Japanese Anime & Manga" },
            { "32", "Entertainment: Cartoon & Animations" },
        };

        public List<string> CategorysToList { get { return _category.Values.ToList(); } }

        public string GetCategoryId(int index)
        {
            return _category.Keys.ToList()[index];
        }
    }

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

