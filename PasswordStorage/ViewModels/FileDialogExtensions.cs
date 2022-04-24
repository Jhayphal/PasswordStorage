using System.Text;
using System.Windows.Forms;

namespace PasswordStorage.ViewModels
{
  internal static class FileDialogExtensions
  {
    public static void SetFilter(this FileDialog dialog, string name, string extension)
      => SetFilter(dialog, (name, extension));

    public static void SetFilter(this FileDialog dialog, params (string name, string extension)[] files)
    {
      var filter = new StringBuilder();

      foreach (var fileType in files)
      {
        if (filter.Length > 0)
          filter.Append('|');

        filter.AppendFormat("{0} (*.{1})|*.{1}", fileType.name, fileType.extension);
      }

      dialog.Filter = filter.ToString();
    }
  }
}
