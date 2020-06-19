using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;

namespace RenameTool.ViewModel
{
    internal class LoadFileCommand : ICommand
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


        public void OpenFile()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            var directory = Path.GetDirectoryName(openFileDialog.FileName);

            if (directory == null)
            {
                throw new ArgumentNullException($"directory ist not defined");
            }
            var filePaths = Directory.GetFiles(directory).ToList();
            foreach (var filePath in filePaths)
            {
                viewModel.FileList.Add(new File(filePath));
            }

            viewModel.OnPropertyChanged();
        }

        public event EventHandler CanExecuteChanged;

        // ReSharper disable once UnusedMember.Global
        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}