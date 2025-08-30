using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Utils
{
    public class ScreenCapture
    {
        public Bitmap CaptureScreen()
        {
            try
            {
                var bounds = Screen.PrimaryScreen.Bounds;
                var bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);

                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
                }

                return bitmap;
            }
            catch (Exception e)
            {
                Logger.Error($"Screen capture failed: {e.Message}");
                return null;
            }
        }
    }
}
