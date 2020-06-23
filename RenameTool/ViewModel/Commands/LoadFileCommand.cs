using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace RenameTool.ViewModel.Commands
{
    public class LoadFileCommand : ICommand
    {
        private readonly ViewModelBase viewModel;


        public LoadFileCommand(ViewModelBase viewModelBase)
        {
            viewModel = viewModelBase;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var directory = SelectPath();
            if (directory == null)
                return;
            AddFiles(directory);
        }

        public event EventHandler CanExecuteChanged;


        public void AddFiles(string directory)
        {
            var filePaths = Directory.GetFiles(directory).ToList();
            viewModel.FileList.Clear();
            viewModel.SelectAll = false;
            foreach (var filePath in filePaths)
            {
                viewModel.FileList.Add(new File(filePath, viewModel));
            }

            viewModel.SelectAll = true;
            viewModel.OnPropertyChanged();
        }

        private static string SelectPath()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            return dialog.ShowDialog() == CommonFileDialogResult.Ok ? dialog.FileName : null;
        }

        // ReSharper disable once UnusedMember.Local
        private void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}