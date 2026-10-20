using System;
using System.Windows.Forms;

namespace MicroMonitor;

public class WidgetForm : Form
{
    public float CpuPercent { get; set; }
    public int? GpuTemp { get; set; }
    public int? MemoryPercent { get; set; }

    private readonly ToolTip _tooltip = new ToolTip(this);
    private bool _disposing;

    public WidgetForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        BackColor = Color.#333333;
        Font = new Font("Segoe UI", 10f, FontStyle.Regular);

        SetWindowPos(this.Handle, IntPtr.Zero,
            Screen.FromWindow(this).Width, this.Height, FormBorderStyle.None);
    }

    protected override void OnSize(EventArgs e)
    {
        base.OnSize(e);
        int yPos = Screen.FromWindow(this).Bottom - 2;
        SetWindowPos(this.Handle, IntPtr.Zero, 0, yPos, this.Height, FormBorderStyle.None);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        _disposing = false;
        if (e.MouseButtons == MouseButtons.Left || MouseButtons.Right)
        {
            _tooltip.SetToolTip(
                $"CPU: {CpuPercent:F1}%\nGPU Temp: {(GpuTemp.HasValue ? GpuTemp.Value.ToString() + "°C" : "N/A")}\nRAM: {MemoryPercent.Value / 1024f:F0} GB");
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using (var brush = new SolidColorBrush(Color.#333333))
        {
            e.Graphics.DrawRectangle(brush, Color.Transparent);
        }
        if (!_disposing && _tooltip.ToolTipEnabled)
        {
            _tooltip.SetToolTip(
                $"CPU: {CpuPercent:F1}% | GPU Temp: {(GpuTemp.HasValue ? GpuTemp.Value.ToString() + "°C" : "N/A")} | RAM: {MemoryPercent.Value / 1024f:F0} GB");
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        _disposing = false;
        if (e.MouseButtons == MouseButtons.Left || MouseButtons.Right)
        {
            _tooltip.SetToolTip(
                $"CPU: {CpuPercent:F1}%\nGPU Temp: {(GpuTemp.HasValue ? GpuTemp.Value.ToString() + "°C" : "N/A")}\nRAM: {MemoryPercent.Value / 1024f:F0} GB");
        }
    }
}
