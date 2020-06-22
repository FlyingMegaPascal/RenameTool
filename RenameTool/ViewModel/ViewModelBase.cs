using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RenameTool.ViewModel.Commands;

namespace RenameTool.ViewModel
{
    public sealed class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            LoadFileCommand = new LoadFileCommand(this);
            RenameCommand = new RenameCommand(this);
            UndoCommand = new UndoCommand(this);
            CopyToClipboardCommand = new CopyToClipboardCommand(this);
            FileList = new ObservableCollection<File>();
        }


        public ObservableCollection<File> FileList { get; }

        public ICommand LoadFileCommand { get; }
        public ICommand RenameCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand CopyToClipboardCommand { get; }

        #region CommandFunctions

        public bool IsAnySelected()
        {
            return FileList.Any(file => file.IsSelected);
        }

        #endregion


        #region UiTextBoxes

        private string prefix = "";

        public string Prefix
        {
            get => prefix;
            set
            {
                prefix = value;
                UpdatePreview();
                OnPropertyChanged();
            }
        }

        private string oldTextValue = "";

        public string OldTextValue
        {
            get => oldTextValue;
            set
            {
                oldTextValue = value;
                UpdatePreview();
                OnPropertyChanged();
            }
        }

        private string newTextValue = "";

        public string NewTextValue
        {
            get => newTextValue;
            set
            {
                newTextValue = value;
                UpdatePreview();
                OnPropertyChanged();
            }
        }

        #endregion


        #region selectAllCheckBox

        private bool selectAll;

        public bool SelectAll
        {
            get => selectAll;
            set
            {
                if (selectAll == value)
                    return;
                selectAll = value;
                SelectAllFiles(value);
                OnPropertyChanged(nameof(SelectAll));
            }
        }

        private void SelectAllFiles(bool value)
        {
            foreach (var file in FileList)
            {
                file.IsSelected = value;
            }

            OnPropertyChanged();
        }

        #endregion

        #region notifier

        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdatePreview()
        {
            foreach (var file in FileList)
            {
                file.UpdateViewItems();
            }
        }

        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}