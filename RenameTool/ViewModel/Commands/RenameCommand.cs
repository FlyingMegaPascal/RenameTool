using System;
using System.Windows.Input;

namespace RenameTool.ViewModel.Commands
{
    internal class RenameCommand : ICommand
    {
        private readonly ViewModelBase viewModel;

        public RenameCommand(ViewModelBase viewModel)
        {
            this.viewModel = viewModel;
            viewModel.PropertyChanged += OnCanExecuteChanged;
        }


        public bool CanExecute(object parameter)
        {
            return viewModel.IsAnySelected() && !string.IsNullOrEmpty(viewModel.Prefix);
        }

        public void Execute(object parameter)
        {
            foreach (var file in viewModel.FileList)
            {
                file.ChangeFileName();
            }

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