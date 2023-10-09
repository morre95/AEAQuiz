using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEAQuiz.Classes
{
    class QuizFactory
    {

        public static async Task<QuizBase> Create(string what, params object[] args)
        {
            if (what == "api" && args.Length == 5) 
            {
                string token = await Token.Get();
                return await Quiz.Fetch(
                (int?)args[0],
                (QType)args[1],
                (Difficulty)args[2],
                (int)args[3],
                token);
            } 
            else if (what.StartsWith("local") && args.Length == 5)
            {
                string[] parts = what.Split(' ');
                return await QuizJson.Fetch(
                (int?)args[0],
                (QType)args[1],
                (Difficulty)args[2],
                (int)args[3],
                parts[1].ToLower());
            }
            else
            {
                throw new ArgumentException("Unknown class");
            }
        }
    }
}
