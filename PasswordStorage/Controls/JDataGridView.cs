using System.Windows.Forms;

namespace PasswordStorage.Controls
{
  public class JDataGridView : DataGridView
  {
    public JDataGridView() : base()
    {
      Dock = DockStyle.Fill;
      AutoGenerateColumns = false;
      AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }
  }
}
