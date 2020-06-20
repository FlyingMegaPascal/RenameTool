using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RenameTool.ViewModel.Commands;

namespace RenameTool.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private string prefix;


        private bool selectAll;

        public ViewModelBase()
        {
            LoadFileCommand = new LoadFileCommand(this);
            RenameCommand = new RenameCommand(this);
            UndoCommand = new UndoCommand(this);
            CopyToClipboard = new CopyToClipboardCommand(this);
            FileList = new ObservableCollection<File>();
        }

        public ICommand LoadFileCommand { get; }
        public ICommand RenameCommand { get; }
        public ICommand UndoCommand { get; }

        public ICommand CopyToClipboard { get; }

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

        public ObservableCollection<File> FileList { get; }

        public bool SelectAll
        {
            get => selectAll;
            set
            {
                if (selectAll == value)
                    return;
                selectAll = value;
                UnOrSelectAllFiles(value);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdatePreview()
        {
            foreach (var file in FileList)
            {
                file.AddPrefix(Prefix);
            }
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UnOrSelectAllFiles(bool value)
        {
            foreach (var file in FileList)
            {
                file.IsSelected = value;
            }
        }

        public bool IsAnySelected()
        {
            return FileList.Any(file => file.IsSelected);
        }
    }
}