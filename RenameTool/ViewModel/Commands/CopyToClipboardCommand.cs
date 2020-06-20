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
            var fileNames = selectedFiles.Select(file => file.OriginalFileName).ToList();
            var txtClipboard = string.Join("\n", fileNames);
            Clipboard.SetText(txtClipboard);

            var messageTxt = "Copy to Clipboard: \n";
            if (selectedFiles.Count > 10)
            {
                messageTxt += string.Join("\n", fileNames.GetRange(0, 9));
                messageTxt += "\n...";
            }
            else
            {
                messageTxt += txtClipboard;
            }

            MessageBox.Show(messageTxt);
            viewModel.OnPropertyChanged();
        }

        public event EventHandler CanExecuteChanged;

        private void OnCanExecuteChanged(object sender, System.ComponentModel.PropertyChangedEventArgs eventArg)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}