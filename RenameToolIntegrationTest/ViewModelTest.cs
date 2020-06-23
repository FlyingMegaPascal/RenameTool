using System.IO;
using RenameTool.ViewModel;
using Xunit;
using File = System.IO.File;

namespace RenameToolIntegrationTest
{
    public class ViewModelTest
    {
        public ViewModelTest()
        {
            cut = new ViewModelBase();
            path = "..\\..\\..\\TestFiles\\";
            NewFiles();
            cut.LoadFileCommand.AddFiles(path);
        }

        private readonly ViewModelBase cut;
        private readonly string path;
        private const string PrefixTestString = "MyPrefix";


        private static void CreateEmptyFile(string filename)
        {
            File.Create(filename).Dispose();
        }

        private void NewFiles()
        {
            var directoryInfo = new DirectoryInfo(path);
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            for (var i = 0; i < 5; i++)
            {
                CreateEmptyFile(path + "Test" + i + ".txt");
            }
        }

        [Fact]
        public void CopyToClipboard_AllFile_Success()
        {
            //Arrange and assert arrangement
            cut.SelectAll = true;

            //act
            var messageText = cut.CopyToClipboardCommand.MessageTxt();
            var clipBoardText = cut.CopyToClipboardCommand.ClipboardText();

            //Assert
            Assert.True(cut.CopyToClipboardCommand.CanExecute(null));
            Assert.NotNull(messageText);
            Assert.NotNull(clipBoardText);
        }

        [Fact]
        public void CopyToClipboard_NoFile_Empty()
        {
            //Arrange and assert arrangement
            cut.SelectAll = false;

            //act
            var messageText = cut.CopyToClipboardCommand.MessageTxt();
            var clipBoardText = cut.CopyToClipboardCommand.ClipboardText();

            //Assert
            Assert.False(cut.CopyToClipboardCommand.CanExecute(null));
            Assert.NotNull(messageText);
            Assert.Equal("", clipBoardText);
        }

        [Fact]
        public void LoadFileCanExecute_null_Success()
        {
            //Act and Assert
            Assert.True(cut.LoadFileCommand.CanExecute(null));
        }


        [Fact]
        public void RenameFiles_SelectedFile_Success()
        {
            //Arrange
            cut.SelectAll = false;
            cut.FileList[2].IsSelected = true;
            const string newValue = "NewValue";
            cut.Prefix = PrefixTestString;
            cut.NewTextValue = newValue;
            cut.OldTextValue = "Test2";

            //act
            cut.RenameCommand.RenameFiles();

            //Assert
            Assert.Equal(PrefixTestString + newValue + ".txt", cut.FileList[2].OriginalFileName);
            Assert.Equal("Test1.txt", cut.FileList[1].OriginalFileName);
        }

        [Fact]
        public void Undo_AllFiles_Success()
        {
            //Arrange

            cut.SelectAll = true;
            var originalName = cut.FileList[0].OriginalFileName;
            cut.Prefix = PrefixTestString;
            cut.RenameCommand.RenameFiles();
            Assert.Equal(PrefixTestString + originalName, cut.FileList[0].OriginalFileName);
            //act
            cut.UndoCommand.Undo();
            //Assert
            Assert.Equal(originalName, cut.FileList[0].OriginalFileName);
        }

        [Fact]
        public void Undo_SingleFile_Success()
        {
            //Arrange and assert arrangement
            cut.SelectAll = false;
            cut.FileList[0].IsSelected = true;
            var originalName = cut.FileList[0].OriginalFileName;
            cut.Prefix = PrefixTestString;
            cut.RenameCommand.RenameFiles();
            Assert.Equal(PrefixTestString + originalName, cut.FileList[0].OriginalFileName);
            //act
            cut.UndoCommand.Undo();
            //Assert
            Assert.Equal(originalName, cut.FileList[0].OriginalFileName);
        }
    }
}