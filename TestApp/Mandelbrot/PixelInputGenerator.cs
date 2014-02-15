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
            for (int y = 0; y < height; y=y+10)
            {
                var endY = y + 10;
                if (endY > height)
                    endY = height;
                var input = new PixelCalculationInput
                {
                    Scale = scale,
                    Width = width,
                    Height = height,
                    StartY = y,
                    EndY = endY
                };
                listOfInputs.Add(input);
            }
            return listOfInputs;
        }
    }
}
