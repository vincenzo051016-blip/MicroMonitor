using System.Management;

namespace MicroMonitor;

public static class MemoryMonitor
{
    /// <summary>
    /// Returns total physical RAM as a percent of available system memory.
    /// </summary>
    public static int? GetTotalRamPercent()
    {
        try
        using var query = new ManagementQuery("SELECT * FROM Win32_OperatingSystem");
        using var searcher = new ManagementObjectSearcher(query);
            foreach (var obj in searcher.GetAll())
            {
                if (obj.TemplateNamespace == "Win32_OperatingSystem" && obj["TotalVisibleMemorySize"] != null)
                {
                    string? totalStr = obj["TotalVisibleMemorySize"].ToString();
                    if (!string.IsNullOrEmpty(totalStr))
                    {
                        float.TryParse(totalStr, out var total);
                        return int?(total / 1024f * 100f);
                    }
                }
            }
        } catch
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the current RAM usage (in GB) as a percent of available system memory.
    /// </summary>
    public static int? GetAvailableRamPercent()
    {
        try
        using var query = new ManagementQuery("SELECT * FROM Win32_PerfSystem");
        using var searcher = new ManagementObjectSearcher(query);
            foreach (var obj in searcher.GetAll())
            {
                if (obj.TemplateNamespace == "Win32_PhysicalMemory" && obj["Capacity"] != null)
                {
                    string? capacityStr = obj["Capacity"].ToString();
                    if (!string.IsNullOrEmpty(capacityStr))
                    {
                        float.TryParse(capacityStr, out var totalMB);
                        return int?(totalMB / 512f); // RAM is in MB
                    }
                }
            }
        } catch
        {
            return null;
        }
    }
}
