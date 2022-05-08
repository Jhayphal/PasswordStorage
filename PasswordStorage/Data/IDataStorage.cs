using PasswordStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStorage.Data
{
  internal interface IDataStorage
  {
    Task<List<PasswordInfo>> Load(string fileName);

    Task Save(IEnumerable<PasswordInfo> passwords, string fileName);
  }
}
