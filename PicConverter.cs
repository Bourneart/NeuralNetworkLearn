using System.Collections.Generic;
using System.Drawing;

namespace NeuralNetwork
{
    public class PicConverter
    {
        public int Threshold { get; set; } = 128;
        public List<double> Convert(string path)
        {
            var result = new List<double>();
            //Получаем изображение
            var image = new Bitmap(path);
            //Получаем список из пикселей изображения
            for (int y = 0; y < image.Height; y++)
            {
                for ( int x = 0; x < image.Width; x++)
                {
                    var pixel = image.GetPixel(x, y);
                    var value = Brightness(pixel);
                    result.Add(value);
                }
            }
            return result;
        }

        private double Brightness (Color pixel)
        {
            var result = 0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B;
            return result < Threshold ? 0 : 1;
        }
    }
}
