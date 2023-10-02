namespace AEAQuiz.Classes
{
    public class ImageService
    {
        private Random _random = new Random();

        public ImageSource GetRandomImageForCategory(int category)
        {

            int imageNumber = _random.Next(1, 4);

            string imagePath = $"CategoryImg/{category}/{category}-{imageNumber}.png";

            return ImageSource.FromFile(imagePath);
        }
    }

}
