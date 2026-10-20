using System.Runtime.InteropServices;

namespace MicroMonitor;

public static class GpuMonitor
{
    /// <summary>
    /// Tries to read the NVIDIA GTX 1070 GPU temperature and usage.
    /// Returns null if no compatible driver is present (no nvidia.nvml.dll).
    /// </summary>
    public static int? GetNvidiaTemperature()
    {
        try
        using var nvml = CoCreateInstance(GetNVMLCLSID(), IntPtr.Zero);
        if (IntPtr.Zero == nvml)
            return null;

        // With nvidia.nvml.dll in the path, real NVML methods would be:
        //   int? currentTemp = GetNVLMSetTemperatures(nvml);
        // But this library is not pre-installed on Windows 11.
        return null;
    }

    private static Guid GetNVMLCLSID()
    {
        return new Guid("6DD8C5CC-4CE0-11E7-84AC-00D1CD86F2EA");
    }
}
