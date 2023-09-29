using Newtonsoft.Json;

namespace AEAQuiz.Classes
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
}

