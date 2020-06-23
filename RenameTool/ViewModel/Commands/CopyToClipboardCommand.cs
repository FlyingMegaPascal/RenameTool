using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RenameTool.ViewModel.Commands
{
    public class CopyToClipboardCommand : ICommand
    {
        private const string NoSelectionMessage = "nothing selected";
        private const string CopiedMessage = "copied to clipboard: \n";
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
            Clipboard.SetText(ClipboardText());
            MessageBox.Show(MessageTxt());
            viewModel.OnPropertyChanged();
        }

        public event EventHandler CanExecuteChanged;

        public string MessageTxt()
        {
            var messageTxt = CopiedMessage;
            if (SelectedFileNames().Count > 10)
            {
                messageTxt += string.Join("\n", SelectedFileNames().GetRange(0, 9));
                messageTxt += "\n...";
            }
            else if (!SelectedFileNames().Any())
            {
                messageTxt = NoSelectionMessage;
            }
            else
            {
                messageTxt += ClipboardText();
            }

            return messageTxt;
        }

        public string ClipboardText()
        {
            var txtClipboard = string.Join("\n", SelectedFileNames());
            return txtClipboard;
        }

        private List<String> SelectedFileNames()
        {
            var selectedFiles = viewModel.FileList.Where(file => file.IsSelected).ToList();
            return selectedFiles.Select(file => file.OriginalFileName).ToList();
        }

        private void OnCanExecuteChanged(object sender, System.ComponentModel.PropertyChangedEventArgs eventArg)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}