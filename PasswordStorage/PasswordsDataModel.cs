using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStorage
{
  public class PasswordsDataModel
  {
    private ObservableCollection<PasswordInfo> passwords;

    public ObservableCollection<PasswordInfo> Passwords => passwords;

    public PasswordsDataModel()
    {
      passwords = new ObservableCollection<PasswordInfo>();
    }

    public async Task LoadAsync(string fileName)
    {
      passwords.Clear();

      using (var stream = File.OpenRead(fileName))
      using (var reader = new StreamReader(stream))
      {
        string line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
          if (PasswordInfo.TryParse(line, out PasswordInfo passwordInfo))
          {
            passwords.Add(passwordInfo);
          }
        }
      }
    }
  }
}
