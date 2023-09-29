﻿using Newtonsoft.Json;

namespace AEAQuiz.Classes
{
    public class Quiz
    {
        private int? _category;
        private string _type;
        private string _difficulty;
        private int _amount = 50;

        private string? _token = null;

        [JsonIgnore]
        public string? Token { get { return _token; } set { _token = value; } }

        [JsonIgnore]
        public int Amount { get { return _amount; } set { _amount = value; } }

        public Quiz() { }

        public Quiz(Category category, Type type, Difficulty difficulty)
        {
            _category = (int)category;
            _type = type.ToString().ToLower();
            _difficulty = difficulty.ToString().ToLower();
        }

        public static Quiz Create()
        {
            return new Quiz()._getQuestions().GetAwaiter().GetResult();
        }

        public static Quiz CreateWithToken(string token)
        {
            Quiz quiz = new Quiz();
            quiz.Token = token;
            return quiz._getQuestions().GetAwaiter().GetResult();
        }

        public static Quiz Create(Category category, Type type, Difficulty difficulty)
        {
            return new Quiz(category, type, difficulty)._getQuestions().GetAwaiter().GetResult();
        }

        public static void DownloadAllTo(string fileName, string token)
        {
            var quiz = CreateWithToken(token);

            List<List<Result>> results = new();

            while (quiz.ResponseCode != 4)
            {
                results.Add(quiz.Results);
                quiz = CreateWithToken(token);
            }

            foreach (var result in results)
            {
                quiz.Results.AddRange(result);
            }
            quiz.ResponseCode = 0;
            File.WriteAllText(fileName, JsonConvert.SerializeObject(quiz));
        }

        private async Task<Quiz> _getQuestions()
        {
            string url = GetUrl();

            using HttpClient httpClient = new HttpClient();
            try
            {
                var json = await httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Quiz>(json)!;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred: " + e.Message);
                return null;
            }
        }

        private string GetUrl()
        {
            string url = "https://opentdb.com/api.php?amount=" + _amount;

            if (_category != null)
            {
                int catId = (int)_category;
                if (catId == 0) url += "&category=any";
                else url += "&category=" + catId;
            }
            if (_type != null && _type != "any") url += "&typte=" + _type;
            if (_difficulty != null && _difficulty != "any") url += "&difficulty=" + _difficulty;
            if (_token != null) url += "&token=" + _token;
            return url;
        }

        [JsonProperty("response_code")]
        public int ResponseCode { get; set; }

        public List<Result> Results { get; set; }
    }
}

