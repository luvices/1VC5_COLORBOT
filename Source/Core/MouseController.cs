using System;
using System.Runtime.InteropServices;
using Utils;

namespace Core
{
    public class MouseController
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const uint MOUSEEVENTF_MOVE = 0x0001;
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;

        public void MoveMouse(double deltaX, double deltaY, double speed = 1.0)
        {
            var scaledDx = (uint)(deltaX * speed);
            var scaledDy = (uint)(deltaY * speed);

            mouse_event(MOUSEEVENTF_MOVE, scaledDx, scaledDy, 0, 0);
        }

        public void LeftClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(1);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
    }
}
