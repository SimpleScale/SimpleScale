using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SimpleScale.Common;
using System.IO;
using System.Drawing.Imaging;

namespace TestApp.Mandelbrot
{
    public class PixelCalculator : IMapJob<PixelCalculationInput, PixelCalculationResult>
    {
        public int MaxIterations = 20000;
        const double MaxValueExtent = 2.0;

        public PixelCalculationResult DoWork(Job<PixelCalculationInput> job)
        {
            return GenerateRectangleOfPixels(job.Data);
        }

        public PixelCalculationResult GenerateRectangleOfPixels(PixelCalculationInput input)
        {
            var height = input.EndY - input.StartY;
            var bitmap = new Bitmap(input.Width, height);
            for (int y = 0; y < height; y++)
            {
                var yOffset = y + input.StartY;
                double scaledPoint = (input.Height / 2 - yOffset) * input.Scale;
                for (int x = 0; x < input.Width; x++)
                {
                    var colour = CalculatePixelColour(input, x, y, scaledPoint);
                    bitmap.SetPixel(x, y, colour);
                }
            }
            var jpeg = ConvertBitmapToJpeg(bitmap);
            return new PixelCalculationResult { Y = input.StartY, JpgImage = jpeg };
        }

        private Color CalculatePixelColour(PixelCalculationInput input, int x, int y, double scaledPoint)
        {
            var s = (x - input.Width / 2) * input.Scale;
            var colour = CalcMandelbrotSetColor(new ComplexNumber(s, scaledPoint));
            return ConvertDoubleToColour(colour);
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

        private Color ConvertDoubleToColour(double value)
        {
            const double MaxColor = 256;
            const double ContrastValue = 0.2;
            return Color.FromArgb(0, 0,
                (int)(MaxColor * Math.Pow(value, ContrastValue)));
        }

        private static byte[] ConvertBitmapToJpeg(Bitmap bitmap)
        {
            var qualityEncoder = System.Drawing.Imaging.Encoder.Quality;

            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(qualityEncoder, 50L);

            var image = Image.FromHbitmap(bitmap.GetHbitmap());
            var memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;
            return memoryStream.GetBuffer();
        }
    }
}
