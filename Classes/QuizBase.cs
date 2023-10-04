namespace AEAQuiz.Classes
{
    public abstract class QuizBase
    {
        public abstract int ResponseCode { get; set; }
        public abstract List<Result> Results { get; set; }
        public abstract string Token { get; set; }
    }
}