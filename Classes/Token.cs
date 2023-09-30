using Newtonsoft.Json;

namespace AEAQuiz.Classes
{
    public class Token
    {
        private string? _token = null;

        private enum TokenOption
        {
            Request,
            Reset
        }

        private static readonly Lazy<Token> lazy = new Lazy<Token>(() => new Token());

        public static Token Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private Token() { }

        private async Task<string> _Get()
        {
            if (_token != null) return _token;
            var res = await _getToken(TokenOption.Request);
            _token = res.Token;
            return _token;
        }

        public static async Task<string> Get()
        {
            return await Instance._Get();
        }

        public static async void Reset(string? token = null)
        {
            if (token != null) Instance._token = token;
            await Instance._getToken(TokenOption.Reset);
        }

        public string GetResponceMSG(int code)
        {
            return new ResponseCode(code).Message;
        }

        private async Task<SessionToken> _getToken(TokenOption option)
        {
            string url = "https://opentdb.com/api_token.php";
            if (option == TokenOption.Request) url += "?command=request";
            else if (option == TokenOption.Reset && _token != null) { url += $"?command=reset&token={_token}"; _token = null; }
            else throw new ArgumentNullException("Token is null");


            using HttpClient httpClient = new HttpClient();
            try
            {
                var json = await httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<SessionToken>(json)!;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred: " + e.Message);
                return null;
            }
        }
    }
}

