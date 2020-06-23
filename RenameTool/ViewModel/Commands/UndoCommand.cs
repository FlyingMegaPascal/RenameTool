using System;
using System.Linq;
using System.Windows.Input;

namespace RenameTool.ViewModel.Commands
{
    public class UndoCommand : ICommand
    {
        private readonly ViewModelBase viewModel;

        public UndoCommand(ViewModelBase viewModel)
        {
            this.viewModel = viewModel;
            viewModel.PropertyChanged += OnCanExecuteChanged;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.FileList.Any(file => file.IsSelected && file.HasBackUp());
        }

        public void Execute(object parameter)
        {
            Undo();
        }

        public event EventHandler CanExecuteChanged;

        public void Undo()
        {
            foreach (var file in viewModel.FileList)
            {
                file.Undo();
            }
            viewModel.OnPropertyChanged();
        }

        private void OnCanExecuteChanged(object sender, System.ComponentModel.PropertyChangedEventArgs eventArg)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}