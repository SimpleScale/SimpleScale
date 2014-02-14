using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SimpleScale.Common;

namespace TestApp.Mandelbrot
{
    public class PixelCalculator : IMapJob<PixelCalculationInput, PixelCalculationResult>
    {
        public int MaxIterations = 20000;
        const double MaxValueExtent = 2.0;

        public PixelCalculationResult DoWork(Job<PixelCalculationInput> job)
        {
            return CalculatePixelColour(job.Data);
        }

        public PixelCalculationResult CalculatePixelColour(PixelCalculationInput input)
        {
            var colours = new List<double>();
            for (int x = 0; x < input.Width; x++)
            {
                double s = (x - input.Width / 2) * input.Scale;
                double colour = CalcMandelbrotSetColor(new ComplexNumber(s, input.ScaledPoint));
                colours.Add(colour);
            }
            return new PixelCalculationResult {Y = input.Y, Colours = colours.ToArray() };
        }

        private double CalcMandelbrotSetColor(ComplexNumber c)
        {
            // from http://en.wikipedia.org/w/index.php?title=Mandelbrot_set
            const double MaxNorm = MaxValueExtent * MaxValueExtent;

            int iteration = 0;
            ComplexNumber z = new ComplexNumber();
            do
            {
                z = z * z + c;
                iteration++;
            } while (z.Norm() < MaxNorm && iteration < MaxIterations);
            if (iteration < MaxIterations)
                return (double)iteration / MaxIterations;
            else
                return 0; // black
        }

    }
}
