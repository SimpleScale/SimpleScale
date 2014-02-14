using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp.Mandelbrot
{
    public class PixelInputGenerator
    {
        const double MaxValueExtent = 2.0;

        public static List<PixelCalculationInput> GenerateListOfInputs(int width, int height)
        {
            var listOfInputs = new List<PixelCalculationInput>();
            double scale = 2 * MaxValueExtent / Math.Min(width, height);
            for (int y = 0; y < height; y++)
            {
                double scaledPoint = (height / 2 - y) * scale;

                var input = new PixelCalculationInput
                {
                    Y = y,
                    ScaledPoint = scaledPoint,
                    Scale = scale,
                    Width = width
                };
                listOfInputs.Add(input);
            }
            return listOfInputs;
        }
    }
}
