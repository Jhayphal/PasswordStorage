using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordStorage
{
  internal static class FormExtensions
  {
    public static void BeginSetup(this Form form, 
      int width = 800,
      int height = 600,
      Padding? padding = null)
    {
      form.ClientSize = new Size(width, height);
      form.Padding = padding ?? Paddings.DefaultPaddings;
    }

    public static void EndSetup(this Form form, 
      float scaleX = 6f, 
      float scaleY = 13f, 
      AutoScaleMode autoScale = AutoScaleMode.Font)
    {
      form.AutoScaleDimensions = new SizeF(scaleX, scaleY);
      form.AutoScaleMode = autoScale;
    }
  }
}
