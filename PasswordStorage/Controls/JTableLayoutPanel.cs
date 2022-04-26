using System.Windows.Forms;

namespace PasswordStorage.Controls
{
  public class JTableLayoutPanel : TableLayoutPanel
  {
    public JTableLayoutPanel() : base()
    {
      Dock = DockStyle.Fill;
    }

    public JTableLayoutPanel(int rows, int columns) : this()
    {
      RowCount = rows;
      ColumnCount = columns;
    }
  }
}
