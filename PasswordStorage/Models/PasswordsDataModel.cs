using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace PasswordStorage.Models
{
  public class PasswordsDataModel
  {
    private BindingList<PasswordInfo> passwords;

    public BindingList<PasswordInfo> Passwords => passwords;

    public PasswordsDataModel()
    {
      passwords = new BindingList<PasswordInfo>
      {
        // Allow new parts to be added, but not removed once committed.        
        AllowNew = true,
        AllowRemove = true,

        // Raise ListChanged events when new parts are added.
        RaiseListChangedEvents = true,

        // Do not allow parts to be edited.
        AllowEdit = true
      };
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

    public async Task SaveAsync(string fileName)
    {
      using (var stream = new FileStream(fileName, FileMode.Create))
      using (var writer = new StreamWriter(stream))
      {
        foreach (var passwordInfo in passwords)
          await writer.WriteLineAsync(passwordInfo.ToString());
      }
    }
  }
}
