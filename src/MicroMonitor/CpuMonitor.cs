using System.Management;

namespace MicroMonitor;

public static class CpuMonitor
{
    public static float GetCurrentUsage()
    {
        try
        using var query = new ManagementQuery("SELECT * FROM Win32_Processor");
        using var searcher = new ManagementObjectSearcher(query);
            foreach (var obj in searcher.GetAll())
            {
                if (obj.TemplateNamespace == "Win32_Processor" && obj["LoadPercentage"] != null)
                {
                    string? load = obj["LoadPercentage"].ToString();
                    if (!string.IsNullOrEmpty(load))
                    {
                        float.TryParse(load, out var pct);
                        return pct;
                    }
                }
            }
        } catch
        {
            return 0f;
        }
    }
}
