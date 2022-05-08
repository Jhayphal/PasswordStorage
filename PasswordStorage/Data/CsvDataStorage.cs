using PasswordStorage.Models;
using System.Collections.Generic;
using System.IO;

namespace PasswordStorage.Data
{
  class CsvDataStorage : IDataStorage
  {
    public List<PasswordInfo> Load(string fileName)
    {
      var passwords = new List<PasswordInfo>();

      using (var stream = File.OpenRead(fileName))
      using (var reader = new StreamReader(stream))
      {
        string line;

        while ((line = reader.ReadLine()) != null)
        {
          if (PasswordInfo.TryParse(line, out PasswordInfo passwordInfo))
          {
            passwords.Add(passwordInfo);
          }
        }
      }

      return passwords;
    }

    public void Save(IEnumerable<PasswordInfo> passwords, string fileName)
    {
      using (var stream = new FileStream(fileName, FileMode.Create))
      using (var writer = new StreamWriter(stream))
      {
        foreach (var passwordInfo in passwords)
          writer.WriteLine(passwordInfo.ToString());
      }
    }
  }
}
