using PasswordStorage.Data;
using System.ComponentModel;

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

    public void Load(string fileName)
    {
      passwords.Clear();

      var items = storage.Load(fileName);

      foreach (var item in items)
        passwords.Add(item);
    }

    public void Save(string fileName)
      => storage.Save(passwords, fileName);
  }
}
