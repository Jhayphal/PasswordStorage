using System.Windows.Forms;

namespace PasswordStorage.Controls
{
  public class JTextBox : TextBox
  {
    public JTextBox() : base()
    {
      Dock = DockStyle.Fill;
    }

    public JTextBox(string text) : this()
    {
      Text = text;
    }
  }
}
