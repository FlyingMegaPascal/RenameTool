using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using RenameTool.Properties;

namespace RenameTool.ViewModel
{
    public class File : INotifyPropertyChanged
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

        private static string Prefix { get; set; }

        public string PreviewFileName
        {
            get => previewFileName;
            set
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
                if (value == selected)
                {
                    return;
                }

                selected = value;
                OnPropertyChanged(nameof(IsSelected));
                UpdateViewItems();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateViewItems()
        {
            PreviewPrefix();
            OnPropertyChanged(nameof(OriginalFileName));
            viewModel.OnPropertyChanged();
        }

        public void AddPrefix(string changedPrefix)
        {
            Prefix = changedPrefix;
            PreviewPrefix();
            OnPropertyChanged(nameof(OriginalFileName));
        }

        private void PreviewPrefix()
        {
            if (IsSelected)
            {
                PreviewFileName = Prefix + OriginalFileName;
            }
            else
            {
                PreviewFileName = OriginalFileName;
            }
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChangeFileName()
        {
            if (!IsSelected) return;
            var path = Path.GetDirectoryName(fullPath) + "\\";
            backUpFileNames.Add(OriginalFileName);
            System.IO.File.Move(path + OriginalFileName, path + PreviewFileName);
            fullPath = path + PreviewFileName;
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
            backUpFileNames.RemoveAt(backUpFileNames.Count - 1);
            System.IO.File.Move(path + OriginalFileName, path + oldFileName);
            fullPath = path + oldFileName;
            UpdateViewItems();
        }
    }
}