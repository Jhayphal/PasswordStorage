using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordStorage
{
  internal static class TableLayoutExtensions
  {
    public static void Build(this TableLayoutPanel pane, params Control[] controls)
      => Build(pane, controls.Select(t => new Control[] { t }).ToArray());

    public static void Build(this TableLayoutPanel pane, params (Control, Control)[] controls)
      => Build(pane, controls.Select(t => new Control[] { t.Item1, t.Item2 }).ToArray());

    public static void Build(this TableLayoutPanel pane, params (Control, Control, Control)[] controls)
      => Build(pane, controls.Select(t => new Control[] { t.Item1, t.Item2, t.Item3 }).ToArray());

    public static void Build(this TableLayoutPanel pane, params (Control, Control, Control, Control)[] controls)
      => Build(pane, controls.Select(t => new Control[] { t.Item1, t.Item2, t.Item3, t.Item4 }).ToArray());

    public static void Build(this TableLayoutPanel pane, params (Control, Control, Control, Control, Control)[] controls)
      => Build(pane, controls.Select(t => new Control[] { t.Item1, t.Item2, t.Item3, t.Item4, t.Item5 }).ToArray());

    public static void Build(this TableLayoutPanel pane, params (Control, Control, Control, Control, Control, Control)[] controls)
      => Build(pane, controls.Select(t => new Control[] { t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6 }).ToArray());

    public static void Build(this TableLayoutPanel pane, params (Control, Control, Control, Control, Control, Control, Control)[] controls)
      => Build(pane, controls.Select(t => new Control[] { t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7 }).ToArray());

    public static void Build(this TableLayoutPanel pane, params (Control, Control, Control, Control, Control, Control, Control, Control)[] controls)
      => Build(pane, controls.Select(t => new Control[] { t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Item8 }).ToArray());

    public static void Build(this TableLayoutPanel pane, params Control[][] controls)
    {
      if (controls == null || controls.Length == 0)
      {
        return;
      }

      pane.SuspendLayout();

      try
      {
        for (int i = 0; i < controls.Length; i++)
        {
          var controlsLine = controls[i];

          if (controlsLine != null)
          {
            for (int j = 0; j < controlsLine.Length; j++)
            {
              var control = controlsLine[j];

              if (control != null)
              {
                pane.Controls.Add(controlsLine[j], j, i);
              }
            }
          }
        }
      }
      finally
      {
        pane.ResumeLayout();
      }
    }
  }
}
