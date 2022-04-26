using PasswordStorage.Controls;
using PasswordStorage.Extensions;
using PasswordStorage.Models;
using PasswordStorage.Properties;
using PasswordStorage.ViewModels;
using ReactiveUI;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Windows.Forms;

namespace PasswordStorage
{
  internal class PasswordsReaderForm : Form, IViewFor<PasswordsViewModel>
  {
    public DataGridView View;

    public TextBox TextBoxFile;
    public Button ButtonBrowse;
    public Button ButtonLoad;

    public Button ButtonCopyLogin;
    public Button ButtonCopyPassword;
    public Button ButtonSave;

    public TextBox TextBoxFilter;
    public Button ButtonApplyFilter;

    public PasswordsViewModel ViewModel { get; set; } = new PasswordsViewModel();

    object IViewFor.ViewModel
    {
      get => ViewModel;
      set => ViewModel = (PasswordsViewModel)value;
    }

    public PasswordsReaderForm()
    {
      this.BeginSetup();
      
      this.Iniatialize();

      this.EndSetup();
    }

    private void Iniatialize()
    {
      var panelMain = CreateMainPanel();
      var groupBoxFile = CreateFileGroupBox();
      var groupBoxPassword = CreatePasswordsGroupBox();
      var groupBoxFilter = CreateFilterGroupBox();

      panelMain.Build(groupBoxFile, groupBoxPassword, groupBoxFilter);

      SetBindings();

      this.Add(panelMain);
    }

    private TableLayoutPanel CreateMainPanel()
    {
      TableLayoutPanel panelMain = new JTableLayoutPanel(rows: 3, columns: 1) { Name = nameof(panelMain) };
      
      panelMain.AddColumnStyle(new ColumnStyle(SizeType.Percent, 100f));
      
      panelMain
        .AddRowStyle(new RowStyle(SizeType.Absolute, 60f))
        .AddRowStyle(new RowStyle(SizeType.Percent, 100f))
        .AddRowStyle(new RowStyle(SizeType.Absolute, 60f));

      return panelMain;
    }

    private GroupBox CreateFileGroupBox()
    {
      GroupBox groupBoxFile = new JGroupBox(Resources.GroupBoxFileHeader) { Name = nameof(groupBoxFile) };

      TableLayoutPanel panelFile = new JTableLayoutPanel(rows: 1, columns: 3) { Name = nameof(panelFile) };
      
      panelFile
        .AddColumnStyle(new ColumnStyle(SizeType.Percent, 100f))
        .AddColumnStyle(new ColumnStyle(SizeType.AutoSize), repeatTimes: 2);

      TextBoxFile = new JTextBox { Name = nameof(TextBoxFile) };

      ButtonBrowse = new JButton(Resources.ButtonBrowseText) { Name = nameof(ButtonBrowse) };
      ButtonLoad = new JButton(Resources.ButtonLoadText) { Name = nameof(ButtonLoad) };

      panelFile.Build((TextBoxFile, ButtonBrowse, ButtonLoad));
      
      groupBoxFile.Add(panelFile);

      return groupBoxFile;
    }

    private GroupBox CreatePasswordsGroupBox()
    {
      GroupBox groupBoxPasswords = new JGroupBox(Resources.GroupBoxPasswordsHeader) { Name = nameof(groupBoxPasswords) };

      TableLayoutPanel panelControls = new JTableLayoutPanel(rows: 2, columns: 4) { Name = nameof(panelControls) };

      panelControls
        .AddColumnStyle(new ColumnStyle(SizeType.AutoSize), repeatTimes: 2)
        .AddColumnStyle(new ColumnStyle(SizeType.Percent, 100f))
        .AddColumnStyle(new ColumnStyle(SizeType.AutoSize));
      
      panelControls
        .AddRowStyle(new RowStyle(SizeType.Percent, 100f))
        .AddRowStyle(new RowStyle(SizeType.AutoSize));

      View = new JDataGridView { Name = nameof(View) };

      var properties = typeof(PasswordInfo).GetProperties
      (
          BindingFlags.Public
        | BindingFlags.Instance
        | BindingFlags.DeclaredOnly
        | BindingFlags.GetProperty
      );

      View.Columns.AddRange
      (
        properties.Select(x => new DataGridViewTextBoxColumn
        {
          Name = x.Name,
          ValueType = x.PropertyType,
          DataPropertyName = x.Name,
          HeaderText = x.Name
        }).ToArray()
      );

      panelControls.SetColumnSpan(control: View, value: panelControls.ColumnCount);

      ButtonCopyLogin = new JButton(Resources.ButtonCopyLoginText) { Name = nameof(ButtonCopyLogin) };
      ButtonCopyPassword = new JButton(Resources.ButtonCopyPaswordText) { Name = nameof(ButtonCopyPassword) };
      ButtonSave = new JButton(Resources.ButtonSaveText) { Name = nameof(ButtonSave) };

      panelControls.Build
      (
        (View, null, null, null),
        (ButtonCopyLogin, ButtonCopyPassword, null, ButtonSave)
      );

      groupBoxPasswords.Add(panelControls);

      return groupBoxPasswords;
    }

    private GroupBox CreateFilterGroupBox()
    {
      GroupBox groupBoxFilter = new JGroupBox(Resources.GroupBoxFilterHeader) { Name = nameof(groupBoxFilter) };

      TableLayoutPanel panelControls = new JTableLayoutPanel(rows: 1, columns: 2) { Name = nameof(panelControls) };

      panelControls
        .AddColumnStyle(new ColumnStyle(SizeType.Percent, 100f))
        .AddColumnStyle(new ColumnStyle(SizeType.AutoSize));
      
      panelControls.AddRowStyle(new RowStyle(SizeType.AutoSize));

      TextBoxFilter = new JTextBox { Name = nameof(TextBoxFilter) };

      ButtonApplyFilter = new JButton(Resources.ButtonApplyFilterText) { Name = nameof(ButtonApplyFilter) };

      panelControls.Build((TextBoxFilter, ButtonApplyFilter));
      
      groupBoxFilter.Add(panelControls);

      return groupBoxFilter;
    }

    private void SetBindings()
    {
      this.WhenActivated(disposables =>
      {
        ViewModel.BrowseFile.RegisterHandler(interaction =>
        {
          OpenFileDialog dialog = new OpenFileDialog
          {
            CheckFileExists = true,
            Multiselect = false,
            RestoreDirectory = true,
            Title = Resources.BrowseDialogHeader
          };

          dialog.SetFilter(Resources.BrowseFilterName, Resources.BrowseFilterExtension);

          if (dialog.ShowDialog() == DialogResult.OK)
            interaction.SetOutput(dialog.FileName);
          else
            interaction.SetOutput(interaction.Input);
        }).DisposeWith(disposables);

        this.Bind(ViewModel, vm => vm.FileName, f => f.TextBoxFile.Text)
          .DisposeWith(disposables);
        this.BindCommand(ViewModel, vm => vm.BrowseCommand, f => f.ButtonBrowse)
          .DisposeWith(disposables);
        this.BindCommand(ViewModel, vm => vm.LoadCommand, f => f.ButtonLoad)
          .DisposeWith(disposables);

        this.Bind(ViewModel, vm => vm.Data, f => f.View.DataSource)
          .DisposeWith(disposables);

        this.BindCommand(ViewModel, vm => vm.CopyLoginCommand, f => f.ButtonCopyLogin)
          .DisposeWith(disposables);
        this.BindCommand(ViewModel, vm => vm.CopyPasswordCommand, f => f.ButtonCopyPassword)
          .DisposeWith(disposables);
        this.BindCommand(ViewModel, vm => vm.SaveCommand, f => f.ButtonSave)
          .DisposeWith(disposables);

        this.Bind(ViewModel, vm => vm.SearchText, f => f.TextBoxFilter.Text)
          .DisposeWith(disposables);
        this.BindCommand(ViewModel, vm => vm.FilterCommand, f => f.ButtonApplyFilter)
          .DisposeWith(disposables);
      });
    }
  }
}
