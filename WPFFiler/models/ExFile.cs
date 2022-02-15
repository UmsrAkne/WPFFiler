namespace WPFFiler.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using Microsoft.VisualBasic.FileIO;
    using Prism.Mvvm;

    public class ExFile : BindableBase
    {
        private FileSystemInfo content;
        private string currentPath;
        private bool isMarked = false;

        /// <summary>
        /// ExFileインスタンスを生成します。
        /// 存在しないパスを指定した場合、対象がファイルかディレクトリか確定できないため、Content プロパティに値が入力されません。
        /// Content を使用する場合には、createFile() or createDirectory() を使用して実体を作成してから使用します。
        /// </summary>
        /// <param name="path"></param>
        public ExFile(string path)
        {
            CurrentPath = path;
            if (!Exists)
            {
                return;
            }

            Content = Directory.Exists(path) ? (FileSystemInfo)new DirectoryInfo(path) : (FileSystemInfo)new FileInfo(path);

            if (IsImageFile)
            {
                FileStream stream = File.OpenRead(Content.FullName);
                Thumbnail.BeginInit();
                Thumbnail.CacheOption = BitmapCacheOption.OnLoad;
                Thumbnail.StreamSource = stream;
                Thumbnail.DecodePixelWidth = 80;
                Thumbnail.EndInit();
                stream.Close();
            }
        }

        public FileSystemInfo Content
        {
            get => content;
            set
            {
                content = value;
            }
        }

        public string CurrentPath
        {
            get => currentPath;
            set
            {
                currentPath = value;
            }
        }

        public bool IsMarked
        {
            get => isMarked;
            set => SetProperty(ref isMarked, value);
        }

        public bool Exists { get => File.Exists(CurrentPath) || Directory.Exists(CurrentPath); }

        /// <summary>
        /// 対象がディレクトリであるかを取得します。対象がファイルであるか、存在しない場合は false を返します。
        /// </summary>
        public bool IsDirectory { get => Directory.Exists(CurrentPath); }

        public string Type { get => IsDirectory ? "[DIR]" : Content.Extension; }

        public BitmapImage Thumbnail { get; private set; } = new BitmapImage();

        public bool IsImageFile
        {
            get => new string[] { ".jpg", ".png", ".bmp" }.Contains(Content.Extension);
        }

        /// <summary>
        /// CurrentPath の値を使用してファイルを新規作成します。
        /// このメソッドを呼び出すと、Content に FileInfo がセットされます。
        /// </summary>
        public void CreateFile()
        {
            var f = new FileInfo(CurrentPath);
            File.Create(f.FullName).Close();
            Content = f;
        }

        /// <summary>
        /// CurrentPath の値を使用してディレクトリを作成します。
        /// このメソッドを呼び出すと、Content に DirectoryInfo がセットされます。
        /// </summary>
        public void CreateDirectory()
        {
            var d = new DirectoryInfo(CurrentPath);
            Directory.CreateDirectory(CurrentPath);
            Content = d;
        }

        public void Delete()
        {
            if (IsDirectory)
            {
                FileSystem.DeleteDirectory(
                    Content.FullName,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin);
            }
            else
            {
                FileSystem.DeleteFile(
                    Content.FullName,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin);
            }
        }

        public void CopyTo(string destinationPath)
        {
            if (IsDirectory)
            {
                FileSystem.CopyDirectory(
                    Content.FullName,
                    destinationPath + "\\" + Content.Name,
                    UIOption.AllDialogs,
                    UICancelOption.DoNothing);
            }
            else
            {
                FileSystem.CopyFile(
                    Content.FullName,
                    destinationPath + "\\" + Content.Name,
                    UIOption.AllDialogs,
                    UICancelOption.DoNothing);
            }
        }

        public void MoveTo(string destinationDirectoryPath)
        {
            if (IsDirectory)
            {
                FileSystem.MoveDirectory(
                    Content.FullName,
                    destinationDirectoryPath + "\\" + Content.Name,
                    UIOption.AllDialogs,
                    UICancelOption.DoNothing);
            }
            else
            {
                FileSystem.MoveFile(
                    Content.FullName,
                    destinationDirectoryPath + "\\" + Content.Name,
                    UIOption.AllDialogs,
                    UICancelOption.DoNothing);
            }
        }
    }
}
