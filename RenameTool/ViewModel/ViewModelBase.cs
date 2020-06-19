using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using System.Windows.Input;
using Microsoft.Win32;

namespace RenameTool.ViewModel
{
    class ViewModelBase : INotifyPropertyChanged
    {

        public ICommand LoadFileCommand { get; }
        public ICommand RenameCommand { get; }
        public ICommand UndoCommand { get; }

        private string prefix;
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

        private void UpdatePreview()
        {
            foreach (var file in FileList)
            {
                file.AddPrefix(Prefix);
            }
        }

        public ViewModelBase()
        {
            LoadFileCommand = new LoadFileCommand(this);
            RenameCommand = new RenameCommand(this);
            UndoCommand = new UndoCommand(this);
            FileList = new ObservableCollection<File>();
        }

        public ObservableCollection<File> FileList { get; }


        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
