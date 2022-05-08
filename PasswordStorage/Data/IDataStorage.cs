using PasswordStorage.Models;
using System.Collections.Generic;

namespace PasswordStorage.Data
{
  internal interface IDataStorage
  {
    List<PasswordInfo> Load(string fileName);

    void Save(IEnumerable<PasswordInfo> passwords, string fileName);
  }
}
