using PasswordStorage.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordStorage
{
  internal class PasswordsReaderForm : Form, IViewFor<PasswordsViewModel>
  {
    private const string PasswordStorageKey = @"C:\Users\OlegN\OneDrive\pswds.csv";

    public DataGridView View;

    public TextBox TextBoxFile;
    public Button ButtonFile;
    public Button ButtonLoad;

    public Button ButtonCopyLogin;
    public Button ButtonCopyPassword;
    public Button ButtonSave;

    public TextBox TextBoxFilter;
    public Button ButtonFilter;

    public PasswordsViewModel ViewModel { get; set; } = new PasswordsViewModel();

    object IViewFor.ViewModel
    {
      get => ViewModel;
      set => ViewModel = (PasswordsViewModel)value;
    }

    public PasswordsReaderForm()
    {
      this.BeginSetup();

      var panelMain = CreateMainPanel();
      var groupBoxFile = CreateFileGroupBox();
      var groupBoxPassword = CreatePasswordsGroupBox();
      var groupBoxFilter = CreateFilterGroupBox();

      panelMain.Build(groupBoxFile, groupBoxPassword, groupBoxFilter);

      SetBindings();

      this.Add(panelMain);

      this.EndSetup();

      //view.DataSource = new PasswordsDataModel(PasswordStorageKey).Passwords;
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
        Text = "File with passwords",
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
        Text = PasswordStorageKey,
        Dock = DockStyle.Fill,
      };

      ButtonFile = new Button
      {
        Name = nameof(ButtonFile),
        Text = "&Browse...",
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };

      ButtonLoad = new Button
      {
        Name = nameof(ButtonLoad),
        Text = "L&oad",
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };

      panelFile.Build((TextBoxFile, ButtonFile, ButtonLoad));
      
      groupBoxFile.Add(panelFile);

      return groupBoxFile;
    }

    private GroupBox CreatePasswordsGroupBox()
    {
      GroupBox groupBoxPasswords = new GroupBox
      {
        Name = nameof(groupBoxPasswords),
        Text = "Passwords",
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
        Text = "Copy &Login",
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };

      ButtonCopyPassword = new Button
      {
        Name = nameof(ButtonCopyPassword),
        Text = "Copy &Password",
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };

      ButtonSave = new Button
      {
        Name = nameof(ButtonSave),
        Text = "&Save",
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
        Text = "Filter",
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

      ButtonFilter = new Button
      {
        Name = nameof(ButtonFilter),
        Text = "&Apply",
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };

      panelControls.Build((TextBoxFilter, ButtonFilter));
      
      groupBoxFilter.Add(panelControls);

      return groupBoxFilter;
    }

    private void SetBindings()
    {
      this.WhenActivated(disposables =>
      {
        this.Bind(ViewModel, vm => vm.FileName, f => f.TextBoxFile.Text)
          .DisposeWith(disposables);
        this.BindCommand(ViewModel, vm => vm.BrowseCommand, f => f.ButtonFile)
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
        this.BindCommand(ViewModel, vm => vm.FilterCommand, f => f.ButtonFilter)
          .DisposeWith(disposables);
      });
    }
  }
}
