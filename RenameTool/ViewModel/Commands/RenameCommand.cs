using System;
using System.Linq;
using System.Windows.Input;

namespace RenameTool.ViewModel.Commands
{
    public class RenameCommand : ICommand
    {
        private readonly ViewModelBase viewModel;

        public RenameCommand(ViewModelBase viewModel)
        {
            this.viewModel = viewModel;
            viewModel.PropertyChanged += OnCanExecuteChanged;
        }


        public bool CanExecute(object parameter)
        {
            return viewModel.FileList.Any(file => file.IsSelected && file.OriginalFileName != file.PreviewFileName);
        }

        public void Execute(object parameter)
        {
            RenameFiles();
        }


        public event EventHandler CanExecuteChanged;

        private void OnCanExecuteChanged(object sender, System.ComponentModel.PropertyChangedEventArgs eventArg)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RenameFiles()
        {
            foreach (var file in viewModel.FileList)
            {
                file.ChangeFileName();
            }

            viewModel.OnPropertyChanged();
        }
    }
}