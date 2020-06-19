using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RenameTool.ViewModel
{
    class RenameCommand : ICommand
    {

        private readonly ViewModelBase viewModel;

        public RenameCommand(ViewModelBase viewModel)
        {
            this.viewModel = viewModel;
            viewModel.PropertyChanged += OnCanExecuteChanged;
        }


        public bool CanExecute(object parameter)
        {
            return viewModel.FileList.Count > 0;
        }

        public void Execute(object parameter)
        {
            foreach (var file in viewModel.FileList)
            {
                file.ChangeFileName();
            }

            viewModel.Prefix = "";
        }

        public event EventHandler CanExecuteChanged;
        
        private void OnCanExecuteChanged(object sender, System.ComponentModel.PropertyChangedEventArgs eventArg)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
