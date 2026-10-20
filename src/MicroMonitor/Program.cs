using System;
using System.Windows.Forms;

namespace MicroMonitor;

class Program
{
    static void Main()
    {
        Application.EnableVisual(true);
        Application.SetCompatibleTextRenderingDefault(false);

        var form = new WidgetForm();
        form.Started += OnWidgetStarted;
        form.SettingsChanged += OnSettingsChanged;
        form.ToggleRunning = true;
        Application.Run(form);
    }
}
