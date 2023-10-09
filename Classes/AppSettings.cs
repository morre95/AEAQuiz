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

        public static bool UseTimerToThink
        {
            get { return Preferences.Default.Get(nameof(UseTimerToThink), true); }
            set { Preferences.Default.Set(nameof(UseTimerToThink), value); }
        }

        public static int TimeToThinkSeconds
        {
            get { return Preferences.Default.Get(nameof(TimeToThinkSeconds), 60); }
            set { Preferences.Default.Set(nameof(TimeToThinkSeconds), value); }
        }

        public static string SelectedQuestionDB
        {
            get { return Preferences.Default.Get(nameof(SelectedQuestionDB), "local EN"); }
            set { Preferences.Default.Set(nameof(SelectedQuestionDB), value); }
        }
    }
}

