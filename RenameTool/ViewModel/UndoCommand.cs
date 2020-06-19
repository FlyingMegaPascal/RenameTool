using System;
using System.Windows.Input;

namespace RenameTool.ViewModel
{
    class UndoCommand : ICommand
    {
        private readonly ViewModelBase viewModel;

        public UndoCommand(ViewModelBase viewModel)
        {
            this.viewModel = viewModel;
            viewModel.PropertyChanged += OnCanExecuteChanged;
        }

        public bool CanExecute(object parameter)
        {
            foreach (var file in viewModel.FileList)
            {
                if (file.IsSelected && file.HasBackUp())
                    return true;
            }

            return false;
        }

        public void Execute(object parameter)
        {
            foreach (var file in viewModel.FileList)
            {
                file.Undo();
            }

            viewModel.OnPropertyChanged();
        }

        public event EventHandler CanExecuteChanged;

        private void OnCanExecuteChanged(object sender, System.ComponentModel.PropertyChangedEventArgs eventArg)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}