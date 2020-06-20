using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RenameTool.ViewModel.Commands
{
    internal class CopyToClipboardCommand : ICommand
    {
        private readonly ViewModelBase viewModel;

        public CopyToClipboardCommand(ViewModelBase viewModel)
        {
            this.viewModel = viewModel;
            viewModel.PropertyChanged += OnCanExecuteChanged;
        }


        public bool CanExecute(object parameter)
        {
            return viewModel.IsAnySelected();
        }

        public void Execute(object parameter)
        {
            var selectedFiles = viewModel.FileList.Where(file => file.IsSelected).ToList();
            var fileNames = selectedFiles.Select(file => file.OriginalFileName);
            var txtClipboard = string.Join("\n", fileNames);
            Clipboard.SetText(txtClipboard);
            MessageBox.Show(txtClipboard);
            //Update Commands
            viewModel.OnPropertyChanged();
        }

        public event EventHandler CanExecuteChanged;

        private void OnCanExecuteChanged(object sender, System.ComponentModel.PropertyChangedEventArgs eventArg)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}