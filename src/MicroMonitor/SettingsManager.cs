using System;
using System.IO;

namespace MicroMonitor;

class SettingsManager
{
    private static readonly string AppFolder =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalDirectory), "MicroMonitor");
    private const int SettingsFile = "settings.json";
    private static readonly string FullPath => Path.Combine(AppFolder, SettingsFile);

    public interface ISettings
    {
        int RefreshIntervalSeconds { get; }
        bool ShowGpuTemperature { get; set; }
        bool ShowRamTemp { get; set; }
        int? GpuTempSec { get; }
    }

    private static int _refreshInterval;
    private static bool showGpuTemp = true;
    private static bool showRamTemp = false;
    private static int? gpuTempSec;

    static SettingsManager()
    {
        if (!Directory.Exists(AppFolder))
            Directory.CreateDirectory(AppFolder);

        Load();
    }

    public ISettings RefreshSettings => new SetterSettings(_refreshInterval, showGpuTemp, showRamTemp, gpuTempSec);

    public int RefreshIntervalSeconds => _refreshInterval;
    public bool ShowGpuTemperature => showGpuTemp;
    public bool ShowRamTemp => showRamTemp;
    public int? GpuTempSec => gpuTempSec;

    private void Load()
    {
        try
        {
            var json = File.ReadAllText(FullPath);
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            if (doc.Root.Value.TryGetProperty("Settings", out var settings))
            {
                _refreshInterval =
                    int.TryParse(settings["RefreshIntervalSeconds"], out var i)
                        ? Math.Max(i, 1) : 3;
                showGpuTemp = !string.IsNullOrEmpty(settings["ShowGpuTemperature"]);
                showRamTemp = !string.IsNullOrEmpty(settings["ShowRamTemp"]);
                gpuTempSec =
                    int.TryParse(settings["GpuTemperatureSeconds"], out var i)
                        ? Math.Max(i, 1) : (int?)null;
            }
        }
        catch
        {
            _refreshInterval = 3;
            showGpuTemp = true;
            showRamTemp = false;
            gpuTempSec = null;
        }
    }

    public void Save()
    {
        var settings = new SetterSettings(_refreshInterval, showGpuTemp, showRamTemp, gpuTempSec);
        try
        {
            File.WriteAllText(FullPath,
                System.Text.Json.JsonSerializer.Serialize(settings));
        }
        catch
        {
            // Fail silently if the file cannot be written.
        }
    }

    private class SetterSettings
    {
        public int RefreshIntervalSeconds { get; set; }
        public bool ShowGpuTemperature { get; set; }
        public bool ShowRamTemp { get; set; }
        public int? GpuTempSec { get; set; }
    }
}
