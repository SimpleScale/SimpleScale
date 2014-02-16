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
    public class MandelbrotCalculator : IMapJob<MandelbrotCalculationInput, MandelbrotCalculationResult>
    {
        public MandelbrotCalculationResult DoWork(Job<MandelbrotCalculationInput> job)
        {
            return GenerateRectangle(job.Data);
        }

        public MandelbrotCalculationResult GenerateRectangle(MandelbrotCalculationInput input)
        {
            var height = input.EndY - input.StartY;
            var bitmap = new Bitmap(input.Width, height);
            for (int y = 0; y < height; y++)
            {
                var realY = y + input.StartY;
                double scaledPoint = (input.Height / 2 - realY) * input.Scale;
                var madelbrotRowCalculator = new MadelbrotRowCalculator { ScaledPoint = scaledPoint, Input = input };
                madelbrotRowCalculator.CalculateRow(bitmap, y);
            }
            var jpeg = ConvertBitmapToJpeg(bitmap);
            return new MandelbrotCalculationResult { Y = input.StartY, JpgImage = jpeg };
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
