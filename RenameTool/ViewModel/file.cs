﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace RenameTool.ViewModel
{
    public sealed class File : INotifyPropertyChanged
    {
        private readonly List<string> backUpFileNames = new List<string>();
        private readonly ViewModelBase viewModel;
        private string fullPath;

        public File(string filePath, ViewModelBase viewModel)
        {
            fullPath = filePath;
            this.viewModel = viewModel;
            PreviewFileName = OriginalFileName;
        }

        #region Properties

        public string OriginalFileName
            => Path.GetFileName(fullPath);

        private string previewFileName;

        public string PreviewFileName
        {
            get => previewFileName;
            private set
            {
                previewFileName = value;
                OnPropertyChanged(nameof(PreviewFileName));
            }
        }


        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value)
                {
                    return;
                }

                isSelected = value;
                UpdateViewItems(nameof(IsSelected));
            }
        }

        #endregion


        #region BusinessLogic

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


        private string CreateNewString()
        {
            if (string.IsNullOrEmpty(viewModel.OldTextValue))
            {
                return viewModel.Prefix + OriginalFileName;
            }

            var replacement = OriginalFileName.Replace(viewModel.OldTextValue, viewModel.NewTextValue);
            return viewModel.Prefix + replacement;
        }

        public bool HasBackUp()
        {
            return backUpFileNames.Any();
        }

        #endregion

        #region Notifier

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateViewItems(string property = "")
        {
            viewModel.OnPropertyChanged(property);
            OnPropertyChanged(property);
            PreviewFileName = !IsSelected ? OriginalFileName : CreateNewString();
        }

        #endregion
    }
}