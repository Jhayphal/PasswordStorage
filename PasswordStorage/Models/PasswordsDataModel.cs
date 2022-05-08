using PasswordStorage.Data;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace PasswordStorage.Models
{
  internal class PasswordsDataModel
  {
    private readonly IDataStorage storage;

    private BindingList<PasswordInfo> passwords;

    public BindingList<PasswordInfo> Passwords => passwords;

    public PasswordsDataModel(IDataStorage storage)
    {
      this.storage = storage;

      passwords = new BindingList<PasswordInfo>
      {
        AllowNew = true,
        AllowRemove = true,
        RaiseListChangedEvents = true,
        AllowEdit = true
      };
    }

    public async Task LoadAsync(string fileName)
    {
      passwords.Clear();

      //using (var stream = File.OpenRead(fileName))
      //using (var reader = new StreamReader(stream))
      //{
      //  string line;

      //  while ((line = await reader.ReadLineAsync()) != null)
      //  {
      //    if (PasswordInfo.TryParse(line, out PasswordInfo passwordInfo))
      //    {
      //      passwords.Add(passwordInfo);
      //    }
      //  }
      //}

      var curT = System.Threading.Thread.CurrentThread.ManagedThreadId;

      await storage.Load(fileName)
        .ContinueWith(t =>
        {
          var nT = System.Threading.Thread.CurrentThread.ManagedThreadId;

          foreach (var item in t.Result)
            passwords.AddNew(); //passwords.Add(item);

          ++nT;
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public async Task SaveAsync(string fileName)
      => await storage.Save(passwords, fileName);
  }
}
