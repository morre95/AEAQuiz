namespace AEAQuiz.Classes
{
    internal class MyColors
    {
        private static readonly List<Color> _colors2 = new List<Color> 
        {
            Colors.Green,
            Color.FromUint(0x808080),
            Colors.MediumBlue,
            Color.FromUint(0x997300),
            Color.FromUint(0x669900),
            Color.FromUint(0x99005c),
            Color.FromUint(0xff0000),
            Color.FromUint(0x000000),
            Colors.Blue,
            Color.FromUint(0x4cb8ff)
        };

        public static Color GetColorBy(int index)
        {
            string str = index.ToString();
            char lastCharacter = str[str.Length - 1];
            return _colors2[int.Parse(lastCharacter.ToString())];
        }

    }
}
