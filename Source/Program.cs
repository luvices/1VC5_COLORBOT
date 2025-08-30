using System;
using System.Diagnostics;
using System.Threading;
using Core;
using Utils;

namespace Source
{
    class Program
    {
        private static Configuration _config;
        private static ColorDetector _colorDetector;
        private static Aimbot _aimbot;
        private static Triggerbot _triggerbot;
        private static MouseController _mouseController;

        private static bool _isAimActive = false;
        private static bool _isTriggerActive = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing 1VC5 COLORBOT...");

            try
            {
                _config = Configuration.LoadFromJson("config.json");
                _mouseController = new MouseController();
                _colorDetector = new ColorDetector(_config);
                _aimbot = new Aimbot(_config, _colorDetector, _mouseController);
                _triggerbot = new Triggerbot(_config, _colorDetector, _mouseController);

                Logger.Info("1VC5 COLORBOT initialized successfully.");

                // Start hotkey listener thread
                new Thread(WatchHotkeys).Start();

                // Main loop
                while (true)
                {
                    try
                    {
                        if (_isAimActive)
                            _aimbot.TryAim();

                        if (_isTriggerActive)
                            _triggerbot.TryTrigger();

                        // Anti-detection delay
                        var delay = _config.AntiDetection.DelayBetweenFrames;

                        if (_config.AntiDetection.RandomDelay)
                        {
                            var randomDelay = _config.AntiDetection.MinRandomDelay +
                                             new Random().Next(0, _config.AntiDetection.MaxRandomDelay + 1);
                            delay += randomDelay;
                        }

                        Thread.Sleep(delay);
                    }
                    catch (Exception e)
                    {
                        Logger.Error($"Main loop error: {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Fatal error: {e.Message}");
                Console.WriteLine($"FATAL: {e.Message}");
                Console.ReadKey();
            }
        }

        private static void WatchHotkeys()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.F1:
                            _isAimActive = !_isAimActive;
                            _aimbot.ToggleAim(_isAimActive);
                            break;

                        case ConsoleKey.F2:
                            _isTriggerActive = !_isTriggerActive;
                            _triggerbot.ToggleTrigger(_isTriggerActive);
                            break;

                        default:
                            continue;
                    }
                }

                Thread.Sleep(50);
            }
        }
    }
}
