using PasswordStorage.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PasswordStorage.Data
{
  class CsvDataStorage : IDataStorage
  {
    public Task<List<PasswordInfo>> Load(string fileName)
    {
      return Task<List<PasswordInfo>>.Run(async () =>
      {
        var passwords = new List<PasswordInfo>();

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

        return passwords;
      });
    }

    public Task Save(IEnumerable<PasswordInfo> passwords, string fileName)
    {
      return Task.Run(async () =>
      {
        using (var stream = new FileStream(fileName, FileMode.Create))
        using (var writer = new StreamWriter(stream))
        {
          foreach (var passwordInfo in passwords)
            await writer.WriteLineAsync(passwordInfo.ToString());
        }
      });
    }
  }
}
