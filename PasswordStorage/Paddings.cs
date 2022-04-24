using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordStorage
{
  internal static class Paddings
  {
    public static readonly int DefaultPaddingSize = 4;

    public static readonly Padding DefaultPaddings = new Padding
    (
      DefaultPaddingSize,
      DefaultPaddingSize,
      DefaultPaddingSize,
      DefaultPaddingSize
    );
  }
}
