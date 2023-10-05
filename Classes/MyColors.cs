namespace AEAQuiz.Classes
{
    internal class MyColors
    {
        private static readonly List<Color> _colors = new List<Color> 
        {
            Colors.Black,
            Colors.Blue,
            Colors.Green,
            Colors.Silver,
            Colors.Olive,
            Colors.Navy,
            Colors.Maroon,
            Colors.Gray,
            Colors.Teal,
            Colors.Purple
            //Color.FromUint(0x4cb8ff)
        };

        public static Color GetColorBy(int index)
        {
            string str = index.ToString();
            char lastCharacter = str[str.Length - 1];
            return _colors[int.Parse(lastCharacter.ToString())];
        }

    }
}
