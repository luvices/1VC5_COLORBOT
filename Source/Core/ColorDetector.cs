using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Utils;

namespace Core
{
    public class ColorDetector
    {
        private readonly Configuration _config;
        private readonly ScreenCapture _screenCapture;

        public ColorDetector(Configuration config)
        {
            _config = config;
            _screenCapture = new ScreenCapture();
        }

        public bool TryFindTargetColor(out Point centerPoint, out int area)
        {
            centerPoint = Point.Empty;
            area = 0;

            var bitmap = _screenCapture.CaptureScreen();
            if (bitmap == null) return false;

            var targetColor = Color.FromArgb(_config.ColorDetection.TargetColor[0], 
                                             _config.ColorDetection.TargetColor[1], 
                                             _config.ColorDetection.TargetColor[2]);

            var bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                var ptr = data.Scan0;
                var bytesPerPixel = 4;
                var stride = data.Stride;
                var height = data.Height;
                var width = data.Width;

                var foundPoints = new System.Collections.Generic.List<Point>();

                for (int y = 0; y < height; y++)
                {
                    var row = (byte*)ptr + (y * stride);
                    for (int x = 0; x < width; x++)
                    {
                        var b = row[x * bytesPerPixel];
                        var g = row[x * bytesPerPixel + 1];
                        var r = row[x * bytesPerPixel + 2];

                        var diffR = Math.Abs(r - targetColor.R);
                        var diffG = Math.Abs(g - targetColor.G);
                        var diffB = Math.Abs(b - targetColor.B);

                        if (diffR <= _config.ColorDetection.Tolerance &&
                            diffG <= _config.ColorDetection.Tolerance &&
                            diffB <= _config.ColorDetection.Tolerance)
                        {
                            foundPoints.Add(new Point(x, y));
                        }
                    }
                }

                if (foundPoints.Count < _config.ColorDetection.MinArea) return false;

                var centerX = foundPoints.Average(p => p.X);
                var centerY = foundPoints.Average(p => p.Y);
                centerPoint = new Point((int)centerX, (int)centerY);
                area = foundPoints.Count;

                return true;
            }
            finally
            {
                bitmap.UnlockBits(data);
                bitmap.Dispose();
            }
        }
    }
}
