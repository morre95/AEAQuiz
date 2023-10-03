namespace AEAQuiz.Classes
{
    internal class Player
    {
        public string Name { get; private set; }

        public int NumberOfRightAswer { get; set; } = 0;

        public Player(string name) 
        { 
            Name = name; 
        }
    }
}
