using System;
using System.Threading;
using Core;
using Utils;

namespace Core
{
    public class Triggerbot
    {
        private readonly Configuration _config;
        private readonly ColorDetector _colorDetector;
        private readonly MouseController _mouseController;

        private bool _isTriggering = false;

        public Triggerbot(Configuration config, ColorDetector colorDetector, MouseController mouseController)
        {
            _config = config;
            _colorDetector = colorDetector;
            _mouseController = mouseController;
        }

        public void TryTrigger()
        {
            if (!_config.Aimbot.TriggerbotEnabled || !_isTriggering) return;

            if (!_colorDetector.TryFindTargetColor(out var centerPoint, out var area)) return;

            var center = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
            var distance = Math.Sqrt(Math.Pow(centerPoint.X - center.X, 2) + Math.Pow(centerPoint.Y - center.Y, 2));

            if (distance < _config.Aimbot.ScanRange)
            {
                Thread.Sleep(_config.Aimbot.TriggerDelay);
                _mouseController.LeftClick();
            }
        }

        public void ToggleTrigger(bool enabled)
        {
            _isTriggering = enabled;
            Logger.Info($"Triggerbot {(enabled ? "ON" : "OFF")} - F2");
        }
    }
}
