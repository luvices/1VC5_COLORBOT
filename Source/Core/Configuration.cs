using System;
using System.IO;
using System.Text.Json;

namespace Core
{
    public class Configuration
    {
        public AimbotConfig Aimbot { get; set; } = new();
        public ColorDetectionConfig ColorDetection { get; set; } = new();
        public HotkeyConfig Hotkey { get; set; } = new();
        public LoggingConfig Logging { get; set; } = new();
        public AntiDetectionConfig AntiDetection { get; set; } = new();

        public static Configuration LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Config file not found: {filePath}");

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Configuration>(json, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }

    public class AimbotConfig
    {
        public bool Enabled { get; set; } = true;
        public double AimSpeed { get; set; } = 2.0;
        public double AimSmooth { get; set; } = 1.5;
        public bool FlickAimEnabled { get; set; } = true;
        public int FlickDistance { get; set; } = 100;
        public bool TriggerbotEnabled { get; set; } = true;
        public int TriggerDelay { get; set; } = 50;
        public bool HeadshotOnly { get; set; } = false;
        public int ScanRange { get; set; } = 500;
    }

    public class ColorDetectionConfig
    {
        public int[] TargetColor { get; set; } = { 255, 0, 255 }; // Purple
        public int Tolerance { get; set; } = 15;
        public int MinArea { get; set; } = 100;
        public int ProcessInterval { get; set; } = 50;
        public double Sensitivity { get; set; } = 0.8;
    }

    public class HotkeyConfig
    {
        public string ToggleKey { get; set; } = "F1";
        public string AutoAimKey { get; set; } = "F2";
    }

    public class LoggingConfig
    {
        public bool Enabled { get; set; } = true;
        public string LogLevel { get; set; } = "INFO";
        public int MaxFileSizeMB { get; set; } = 10;
    }

    public class AntiDetectionConfig
    {
        public int DelayBetweenFrames { get; set; } = 8;
        public bool RandomDelay { get; set; } = true;
        public int MinRandomDelay { get; set; } = 2;
        public int MaxRandomDelay { get; set; } = 7;
    }
}
