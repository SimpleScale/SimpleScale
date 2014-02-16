using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp.Mandelbrot
{
    public class InputGenerator
    {

        public static List<MandelbrotCalculationInput> GenerateListOfInputs(int width, int height)
        {
            var listOfInputs = new List<MandelbrotCalculationInput>();
            double scale = 2 * Common.MaxValueExtent / Math.Min(width, height);
            for (int y = 0; y < height; y=y+10)
            {
                var endY = y + 10;
                if (endY > height)
                    endY = height;
                var input = new MandelbrotCalculationInput
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
