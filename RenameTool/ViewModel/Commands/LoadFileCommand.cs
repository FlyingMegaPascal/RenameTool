using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace RenameTool.ViewModel.Commands
{
    internal sealed class LoadFileCommand : ICommand
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
            OpenFile();
        }

        public event EventHandler CanExecuteChanged;


        private void OpenFile()
        {
            var directory = SelectPath();
            if (directory == null)
                return;
            AddFiles(directory);
            viewModel.OnPropertyChanged();
        }

        private void AddFiles(string directory)
        {
            var filePaths = Directory.GetFiles(directory).ToList();
            viewModel.FileList.Clear();
            viewModel.SelectAll = false;
            foreach (var filePath in filePaths)
            {
                
                viewModel.FileList.Add(new File(filePath, viewModel));
            }

            viewModel.SelectAll = true;
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