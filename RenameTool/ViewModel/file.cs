using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using RenameTool.Properties;

namespace RenameTool.ViewModel
{
    public sealed class File : INotifyPropertyChanged
    {
        private readonly List<string> backUpFileNames = new List<string>();

        private readonly ViewModelBase viewModel;
        private string fullPath;
        private string previewFileName;
        private bool selected;


        public File(string fullPath, ViewModelBase viewModel)
        {
            this.fullPath = fullPath;
            this.viewModel = viewModel;
            PreviewFileName = OriginalFileName;
        }

        public string OriginalFileName
            => Path.GetFileName(fullPath);


        public string PreviewFileName
        {
            get => previewFileName;
            private set
            {
                previewFileName = value;
                OnPropertyChanged(nameof(PreviewFileName));
            }
        }

        public bool IsSelected
        {
            get => selected;
            set
            {
                if (selected == value)
                {
                    return;
                }

                selected = value;
                UpdateViewItems();
                viewModel.OnPropertyChanged();
                OnPropertyChanged(nameof(IsSelected));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateViewItems()
        {
            OnPropertyChanged(nameof(OriginalFileName));
            PreviewFileName = !IsSelected ? OriginalFileName : CreateNewString();
        }


        private string CreateNewString()
        {
            if (string.IsNullOrEmpty(viewModel.OldTextValue))
            {
                return viewModel.Prefix + OriginalFileName;
            }

            var replacement = OriginalFileName.Replace(viewModel.OldTextValue, viewModel.NewTextValue);
            return viewModel.Prefix + replacement;
        }


        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChangeFileName()
        {
            if (!IsSelected)
            {
                return;
            }

            var path = Path.GetDirectoryName(fullPath) + "\\";
            backUpFileNames.Add(OriginalFileName);
            try
            {
                System.IO.File.Move(path + OriginalFileName, path + PreviewFileName);
                fullPath = path + PreviewFileName;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            UpdateViewItems();
        }

        public bool HasBackUp()
        {
            return backUpFileNames.Any();
        }

        public void Undo()
        {
            if (!IsSelected || !HasBackUp())
            {
                return;
            }

            var path = Path.GetDirectoryName(fullPath) + "\\";
            var oldFileName = backUpFileNames.Last();
            try
            {
                System.IO.File.Move(path + OriginalFileName, path + oldFileName);
                backUpFileNames.RemoveAt(backUpFileNames.Count - 1);
                fullPath = path + oldFileName;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            UpdateViewItems();
        }
    }
}