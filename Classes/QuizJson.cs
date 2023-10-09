using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace AEAQuiz.Classes
{
    class QuizJson : QuizBase
    {
        [JsonProperty("response_code")]
        public override int ResponseCode { get; set; }
        public override List<Result> Results { get; set; } = new List<Result>();
        public override string Token { get; set; }

        public static async Task<QuizJson> Fetch(int? category, QType type, Difficulty difficulty, int amount)
        {
            QuizJson quiz = await LoadJsonDB();

            QuizJson qResult = new();
            string _type = type.ToString().ToLower();
            string _difficulty = difficulty.ToString().ToLower();

            foreach (var result in quiz.Results)
            {
                if ((Categories.GetCategoryIdByName(result.Category) == category || category == null) &&
                    (result.Type.ToLower() == _type || _type == "any") &&
                    (result.Difficulty.ToLower() == _difficulty || _difficulty == "any"))
                {
                    qResult.Results.Add(result);
                }
            }

            if (qResult.Results.Count < amount) 
            { 
                // TODO: Detta kan returnera ett antal frågor som skulle kunna användas.
                // Exempel: Kontrollera i GamePage om det finns >= 1 frågor och ladda om sidan om det är ok för användaren med en knapp
                qResult.ResponseCode = 1;
                return qResult; 
            }

            return GetRandomAmount(amount, qResult);
        }

        private static QuizJson GetRandomAmount(int amount, QuizJson qResult)
        {
            var r = new Random();
            QuizJson ret = new();
            HashSet<int> set = new HashSet<int>(amount);
            for (int i = 0; i < amount; i++)
            {
                int index;

                do
                {
                    index = r.Next(qResult.Results.Count);
                } while (!set.Add(index));

                ret.Results.Add(qResult.Results[index]);
            }

            return ret;
        }

        private static async Task<QuizJson> LoadJsonDB()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("quizDB.json");
            using var reader = new StreamReader(stream);

            var contents = reader.ReadToEnd();

            return JsonConvert.DeserializeObject<QuizJson>(contents);
        }

    }
}
