using System.Windows.Forms;

namespace PasswordStorage.Controls
{
  public class JButton : Button
  {
    public JButton() : base()
    {
      AutoSize = true;
      AutoSizeMode = AutoSizeMode.GrowAndShrink;
    }

    public JButton(string text) : this()
    {
      Text = text;
    }
  }
}
