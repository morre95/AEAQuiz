namespace AEAQuiz.Classes
{
    internal class Player
    {
        public string Name { get; private set; }

        public Color Color { get; private set; }

        public int NumberOfRightAswer { get; set; } = 0;
        public int NumberOfPoints { get; set; } = 0;


        public Player(string name, Color color) 
        { 
            Name = name; 
            Color = color;
        }


    }
}
