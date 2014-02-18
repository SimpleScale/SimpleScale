using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TestApp.Mandelbrot
{
    public class MadelbrotRowCalculator
    {
        public double ScaledPoint;
        public MandelbrotCalculationInput Input;

        public void CalculateRow(Bitmap bitmap, int y)
        {
            for (int x = 0; x < Input.Width; x++)
            {
                var colour = CalculatePixelColour(x, y);
                bitmap.SetPixel(x, y, colour);
            }
        }

        private Color CalculatePixelColour(int x, int y)
        {
            var s = (x - Input.Width / 2) * Input.Scale;
            var colour = CalcMandelbrotPixelColor(new ComplexNumber(s, ScaledPoint));
            return ConvertDoubleToColour(colour);
        }

        private double CalcMandelbrotPixelColor(ComplexNumber c)
        {
            // from http://en.wikipedia.org/w/index.php?title=Mandelbrot_set
            var MaxNorm = Common.MaxValueExtent * Common.MaxValueExtent;

            int iteration = 0;
            var z = new ComplexNumber();
            do
            {
                z = z * z + c;
                iteration++;
            } while (z.Norm() < MaxNorm && iteration < Common.MaxIterations);
            if (iteration < Common.MaxIterations)
                return (double)iteration / Common.MaxIterations;
            else
                return 0; // black
        }

        private Color ConvertDoubleToColour(double value)
        {
            const double MaxColor = 256;
            const double ContrastValue = 0.2;
            var colourValue = (int)(MaxColor * Math.Pow(value, ContrastValue));
            return Color.FromArgb(0, colourValue, colourValue);
        }
    }
}
