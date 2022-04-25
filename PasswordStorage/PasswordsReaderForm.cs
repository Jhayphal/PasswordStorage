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
      TableLayoutPanel panelMain = new TableLayoutPanel
      {
        Name = nameof(panelMain),
        RowCount = 3,
        ColumnCount = 1
      };

      panelMain.Dock = DockStyle.Fill;
      
      panelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      
      panelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
      panelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      panelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));

      return panelMain;
    }

    private GroupBox CreateFileGroupBox()
    {
      GroupBox groupBoxFile = new GroupBox
      {
        Name = nameof(groupBoxFile),
        Text = Resources.GroupBoxFileHeader,
        Dock = DockStyle.Fill
      };

      TableLayoutPanel panelFile = new TableLayoutPanel
      {
        Name = nameof(panelFile),
        RowCount = 1,
        ColumnCount = 3
      };

      panelFile.Dock = DockStyle.Fill;
      
      panelFile.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      panelFile.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
      panelFile.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

      TextBoxFile = new TextBox
      {
        Name = nameof(TextBoxFile),
        Dock = DockStyle.Fill
      };

      ButtonBrowse = new Button
      {
        Name = nameof(ButtonBrowse),
        Text = Resources.ButtonBrowseText,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };

      ButtonLoad = new Button
      {
        Name = nameof(ButtonLoad),
        Text = Resources.ButtonLoadText,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };

      panelFile.Build((TextBoxFile, ButtonBrowse, ButtonLoad));
      
      groupBoxFile.Add(panelFile);

      return groupBoxFile;
    }

    private GroupBox CreatePasswordsGroupBox()
    {
      GroupBox groupBoxPasswords = new GroupBox
      {
        Name = nameof(groupBoxPasswords),
        Text = Resources.GroupBoxPasswordsHeader,
        Dock = DockStyle.Fill
      };

      TableLayoutPanel panelControls = new TableLayoutPanel
      {
        Name = nameof(panelControls),
        ColumnCount = 4,
        RowCount = 2,
        Dock = DockStyle.Fill
      };

      panelControls.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
      panelControls.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
      panelControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      panelControls.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
      
      panelControls.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      panelControls.RowStyles.Add(new RowStyle(SizeType.AutoSize));

      View = new DataGridView
      {
        Name = nameof(View),
        Dock = DockStyle.Fill,
        AutoGenerateColumns = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
      };

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

      ButtonCopyLogin = new Button
      {
        Name = nameof(ButtonCopyLogin),
        Text = Resources.ButtonCopyLoginText,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };

      ButtonCopyPassword = new Button
      {
        Name = nameof(ButtonCopyPassword),
        Text = Resources.ButtonCopyPaswordText,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };

      ButtonSave = new Button
      {
        Name = nameof(ButtonSave),
        Text = Resources.ButtonSaveText,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };

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
      GroupBox groupBoxFilter = new GroupBox
      {
        Name = nameof(groupBoxFilter),
        Text = Resources.GroupBoxFilterHeader,
        Dock = DockStyle.Fill
      };

      TableLayoutPanel panelControls = new TableLayoutPanel
      {
        Name = nameof(panelControls),
        ColumnCount = 2,
        RowCount = 1,
        Dock = DockStyle.Fill
      };

      panelControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      panelControls.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
      
      panelControls.RowStyles.Add(new RowStyle(SizeType.AutoSize));

      TextBoxFilter = new TextBox
      {
        Name = nameof(TextBoxFilter),
        Dock = DockStyle.Fill
      };

      ButtonApplyFilter = new Button
      {
        Name = nameof(ButtonApplyFilter),
        Text = Resources.ButtonApplyFilterText,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };

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
