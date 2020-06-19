using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using RenameTool.Annotations;

namespace RenameTool.ViewModel
{
    public class File : INotifyPropertyChanged
    {
        private string fullPath;

        private readonly List<string> backUpFileNames = new List<string>();

        public string OriginalFileName
            => Path.GetFileName(fullPath);

        private static string Prefix { get; set; }
        
        private string previewFileName;

        public string PreviewFileName { 
            get => previewFileName;
            set
            {
                previewFileName = value;
                OnPropertyChanged(nameof(PreviewFileName));
            }
        }



        public File(string fullPath)
        {
            this.fullPath = fullPath;
            PreviewFileName = OriginalFileName;

        }


        private bool selected;

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
            }
        }

        public void AddPrefix(string changedPrefix)
        {
            Prefix = changedPrefix;
            if (IsSelected)
            {
                PreviewFileName = Prefix + OriginalFileName;
            }
            
        }


        public event PropertyChangedEventHandler PropertyChanged;


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
            OnPropertyChanged(nameof(OriginalFileName));

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
            OnPropertyChanged(nameof(OriginalFileName));
        }
    }
}