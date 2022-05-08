using PasswordStorage.Data;
using PasswordStorage.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordStorage.ViewModels
{
  public class PasswordsViewModel : ReactiveObject
  {
    private ObservableAsPropertyHelper<string> fileName;
    private string searchText;
    
    private PasswordInfo selected;
    private readonly Interaction<string, string> browseFile = new Interaction<string, string>();
    private readonly PasswordsDataModel data = new PasswordsDataModel(new CsvDataStorage());
    private readonly List<PasswordInfo> hidenByFilter = new List<PasswordInfo>();

    private bool isOperable = true;

    public Interaction<string, string> BrowseFile 
      => this.browseFile;

    public string FileName => fileName?.Value ?? string.Empty;

    public string SearchText
    {
      get => searchText;
      set => this.RaiseAndSetIfChanged(ref searchText, value);
    }

    public BindingList<PasswordInfo> Data
      => data.Passwords;

    public PasswordInfo Selected 
    {
      get => selected;
      set => this.RaiseAndSetIfChanged(ref selected, value);
    }

    public bool IsOperable
    {
      get => isOperable;
      set => this.RaiseAndSetIfChanged(ref isOperable, value);
    }

    public ReactiveCommand<Unit, Unit> BrowseCommand { get; }

    public ReactiveCommand<Unit, Unit> LoadCommand { get; }

    public ReactiveCommand<Unit, Unit> SaveCommand { get; }

    public ReactiveCommand<Unit, Unit> CopyLoginCommand { get; }

    public ReactiveCommand<Unit, Unit> CopyPasswordCommand { get; }

    public ReactiveCommand<Unit, Unit> FilterCommand { get; }

    public PasswordsViewModel()
    {
      BrowseCommand = ReactiveCommand.Create(Browse);

      var canLoad = this.WhenAnyValue(
        x => x.IsOperable,
        x => x.FileName,
        (operable, file) => operable && !string.IsNullOrWhiteSpace(file) && File.Exists(file))
        .Throttle(TimeSpan.FromSeconds(.25), RxApp.MainThreadScheduler)
        .DistinctUntilChanged();

      LoadCommand = ReactiveCommand.CreateFromTask(Load, canLoad);

      var canSave = this.WhenAnyValue(
        x => x.IsOperable,
        x => x.FileName,
        (operable, file) => operable && !string.IsNullOrWhiteSpace(file) && Directory.Exists(Path.GetDirectoryName(file)))
        .DistinctUntilChanged();

      SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync, canSave);

      var canCopy = this.WhenAnyValue(
        x => x.Data,
        x => x.Selected,
        (data, selected) => data.Count > 0 && selected != null)
        .DistinctUntilChanged();

      CopyLoginCommand = ReactiveCommand.Create(CopyLogin, canCopy);
      CopyPasswordCommand = ReactiveCommand.Create(CopyPassword, canCopy);

      var canFilter = this.WhenAnyValue(
        x => x.IsOperable, 
        x => x.SearchText, 
        x => x.hidenByFilter, 
        (operable, search, hiden) 
          => operable && (!string.IsNullOrEmpty(search) || hiden.Count > 0))
        .DistinctUntilChanged();

      FilterCommand = ReactiveCommand.Create(Filter, canFilter);
    }

    private void Browse()
      => fileName = browseFile.Handle(FileName).ToProperty(this, nameof(FileName));

    private Task Load() => data.LoadAsync(FileName);

    private async Task SaveAsync() 
      => await data.SaveAsync(FileName);

    private void Filter()
    {
      if (hidenByFilter.Count == 0)
      {
        hidenByFilter.AddRange(Data.Where(IsNotSuitable));

        foreach (var password in hidenByFilter)
          Data.Remove(password);
      }
      else if (string.IsNullOrEmpty(SearchText))
      {
        foreach(var password in hidenByFilter)
          Data.Add(password);

        hidenByFilter.Clear();
      }
      else
      {
        var cache = new List<PasswordInfo>(hidenByFilter);
        cache.AddRange(Data);

        hidenByFilter.Clear();
        hidenByFilter.AddRange(cache.Where(IsNotSuitable));

        foreach (var password in hidenByFilter)
          Data.Remove(password);

        foreach (var password in cache.Where(IsSuitable))
          if (!Data.Any(p => ReferenceEquals(p, password)))
            Data.Add(password);
      }
    }

    private bool IsSuitable(PasswordInfo password)
      => password.ToString().IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) > -1;

    private bool IsNotSuitable(PasswordInfo password)
      => !IsSuitable(password);

    private void CopyLogin() 
      => Clipboard.SetText(Selected.UserName);

    private void CopyPassword() 
      => Clipboard.SetText(Selected.Password);
  }
}
