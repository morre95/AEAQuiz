using Newtonsoft.Json;

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

        public Quiz(int? category, QType type, Difficulty difficulty)
        {
            _category = category;
            _type = type.ToString().ToLower();
            _difficulty = difficulty.ToString().ToLower();
        }
        public Quiz(int? category, QType type, Difficulty difficulty, int amount)
        {
            _category = category;
            _type = type.ToString().ToLower();
            _difficulty = difficulty.ToString().ToLower();
            _amount = amount;
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

        public static async Task<Quiz> Create(int? category, QType type, Difficulty difficulty)
        {
            return await new Quiz(category, type, difficulty)._getQuestions();
        }

        public static async Task<Quiz> Create(int? category, QType type, Difficulty difficulty, int amount)
        {
            return await new Quiz(category, type, difficulty, amount)._getQuestions();
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
            if (_type != null && _type != "any") url += "&type=" + _type;
            if (_difficulty != null && _difficulty != "any") url += "&difficulty=" + _difficulty;
            if (_token != null) url += "&token=" + _token;
            return url;
        }

        [JsonProperty("response_code")]
        public int ResponseCode { get; set; }

        public List<Result> Results { get; set; }
    }
}

