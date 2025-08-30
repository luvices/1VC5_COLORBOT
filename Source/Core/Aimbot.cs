using System;
using System.Drawing;
using Core;
using Utils;

namespace Core
{
    public class Aimbot
    {
        private readonly Configuration _config;
        private readonly ColorDetector _colorDetector;
        private readonly MouseController _mouseController;
        private readonly Random _random;

        private bool _isAiming = false;

        public Aimbot(Configuration config, ColorDetector colorDetector, MouseController mouseController)
        {
            _config = config;
            _colorDetector = colorDetector;
            _mouseController = mouseController;
            _random = new Random();
        }

        public void TryAim()
        {
            if (!_config.Aimbot.Enabled || !_isAiming) return;

            if (!_colorDetector.TryFindTargetColor(out var centerPoint, out var area)) return;

            // Get screen center
            var screenWidth = Screen.PrimaryScreen.Bounds.Width;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;
            var screenCenter = new Point(screenWidth / 2, screenHeight / 2);

            // Calculate relative position
            var deltaX = centerPoint.X - screenCenter.X;
            var deltaY = centerPoint.Y - screenCenter.Y;

            // Apply aim speed & smooth
            var smoothSpeed = _config.Aimbot.AimSmooth;
            var speed = _config.Aimbot.AimSpeed;

            var aimX = (deltaX / smoothSpeed) * speed;
            var aimY = (deltaY / smoothSpeed) * speed;

            // Apply flick aim if enabled and far enough
            if (_config.Aimbot.FlickAimEnabled && area > _config.Aimbot.ScanRange)
            {
                aimX = deltaX > 0 ? _config.Aimbot.FlickDistance : -_config.Aimbot.FlickDistance;
                aimY = deltaY > 0 ? _config.Aimbot.FlickDistance : -_config.Aimbot.FlickDistance;
            }

            _mouseController.MoveMouse(aimX, aimY, _config.Aimbot.AimSpeed);
        }

        public void ToggleAim(bool enabled)
        {
            _isAiming = enabled;
            Logger.Info($"Aimbot {(enabled ? "ON" : "OFF")} - F1");
        }
    }
}
