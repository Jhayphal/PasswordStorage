﻿using ReactiveUI;
using System;

namespace PasswordStorage.Models
{
  public class PasswordInfo : ReactiveObject, ICloneable
  {
    private string url;
    private string userName;
    private string password;
    private string comment;
    private string tags;

    public string Url
    { 
      get => url;
      set => this.RaiseAndSetIfChanged(ref url, value);
    }

    public string UserName
    {
      get => userName;
      set => this.RaiseAndSetIfChanged(ref userName, value);
    }

    public string Password
    {
      get => password;
      set => this.RaiseAndSetIfChanged(ref password, value);
    }

    public string Comment
    {
      get => comment;
      set => this.RaiseAndSetIfChanged(ref comment, value);
    }

    public string Tags
    {
      get => tags;
      set => this.RaiseAndSetIfChanged(ref tags, value);
    }

    public override string ToString() => $"{Url},{UserName},{Password},{Comment},{Tags}";

    public static bool TryParse(string text, out PasswordInfo password)
    {
      var parts = text.Split(',');

      if (parts.Length != 5)
      {
        password = null;
        return false;
      }

      password = new PasswordInfo
      {
        Url = parts[0],
        UserName = parts[1],
        Password = parts[2],
        Comment = parts[3],
        Tags = parts[4]
      };

      return true;
    }

    public virtual object Clone() => new PasswordInfo
    {
      Url = Url,
      UserName = UserName,
      Password = Password,
      Comment = Comment,
      Tags = Tags
    };
  }
}
