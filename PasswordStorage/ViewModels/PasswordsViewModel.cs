using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    private string fileName;
    private string searchText;
    
    private PasswordInfo selected;
    private readonly PasswordsDataModel data;

    private bool isOperable = true;

    public string FileName
    {
      get => fileName;
      set => this.RaiseAndSetIfChanged(ref fileName, value);
    }

    public string SearchText
    {
      get => searchText;
      set => this.RaiseAndSetIfChanged(ref searchText, value);
    }

    public ObservableCollection<PasswordInfo> Data => data.Passwords;

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
      data = new PasswordsDataModel();

      BrowseCommand = ReactiveCommand.Create(Browse);

      var canLoad = this.WhenAnyValue(
        x => x.IsOperable,
        x => x.FileName,
        (operable, file) => operable && !string.IsNullOrWhiteSpace(file) && File.Exists(file))
        .Throttle(TimeSpan.FromSeconds(.25))
        .DistinctUntilChanged();

      LoadCommand = ReactiveCommand.CreateFromTask(LoadAsync, canLoad);

      var canSave = this.WhenAnyValue(
        x => x.IsOperable,
        x => x.FileName,
        (operable, file) => operable && !string.IsNullOrWhiteSpace(file))
        .DistinctUntilChanged();

      SaveCommand = ReactiveCommand.Create(Save, canSave);

      var canCopy = this.WhenAnyValue(
        x => x.IsOperable,
        x => x.Selected,
        (operable, selected) => operable && selected != null)
        .DistinctUntilChanged();

      CopyLoginCommand = ReactiveCommand.Create(CopyLogin, canCopy);
      CopyPasswordCommand = ReactiveCommand.Create(CopyPassword, canCopy);

      var canFilter = this.WhenAnyValue(
        x => x.IsOperable,
        x => x.SearchText,
        (operable, search) => operable && !string.IsNullOrWhiteSpace(search))
        .DistinctUntilChanged();

      FilterCommand = ReactiveCommand.Create(Filter, canFilter);
    }

    private void Browse()
    {
      OpenFileDialog dialog = new OpenFileDialog
      {
        CheckFileExists = true,
        Multiselect = false,
        RestoreDirectory = true,
        Title = "Choose your file..."
      };

      dialog.SetFilter("Password files", "pwd");

      if (dialog.ShowDialog() == DialogResult.OK)
      {
        FileName = dialog.FileName;
      }
    }

    private async Task LoadAsync() => await data.LoadAsync(FileName);

    private void Save() { }

    private void Filter() => MessageBox.Show(SearchText);

    private void CopyLogin() => Clipboard.SetText(Selected.UserName);

    private void CopyPassword() => Clipboard.SetText(Selected.Password);
  }
}
