using System.Windows.Forms;

namespace PasswordStorage.Extensions
{
  internal static class ControlExtensions
  {
    public static void Add(this Control control, params Control[] controls)
    {
      if (controls == null || controls.Length == 0)
        return;

      control.SuspendLayout();

      foreach (Control currentControl in controls)
        control.Controls.Add(currentControl);

      control.ResumeLayout();
    }
  }
}
