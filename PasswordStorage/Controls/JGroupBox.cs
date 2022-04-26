using System.Windows.Forms;

namespace PasswordStorage.Controls
{
  public class JGroupBox : GroupBox
  {
    public JGroupBox() : base()
    {
      Dock = DockStyle.Fill;
    }

    public JGroupBox(string text) : this()
    {
      Text = text;
    }
  }
}
