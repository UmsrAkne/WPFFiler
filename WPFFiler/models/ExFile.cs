﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Media.Imaging;

namespace WPFFiler.models {
    public class ExFile : BindableBase{

        private FileSystemInfo content;
        public FileSystemInfo Content {
            get => content;
            set {
                content = value;
            }
        }

        private string currentPath;
        public string CurrentPath {
            get => currentPath;
            set {
                currentPath = value;
            }
        }

        private bool isMarked = false;
        public bool IsMarked {
            get => isMarked;
            set => SetProperty(ref isMarked, value);
        }

        /// <summary>
        /// ExFileインスタンスを生成します。
        /// 存在しないパスを指定した場合、対象がファイルかディレクトリか確定できないため、Content プロパティに値が入力されません。
        /// Content を使用する場合には、createFile() or createDirectory() を使用して実体を作成してから使用します。
        /// </summary>
        /// <param name="path"></param>
        public ExFile(string path) {
            CurrentPath = path;
            if (!Exists) {
                return;
            }

            Content = (Directory.Exists(path)) ? (FileSystemInfo)new DirectoryInfo(path) : (FileSystemInfo)new FileInfo(path);

            if (IsImageFile) {
                FileStream stream = File.OpenRead(Content.FullName);
                Thumbnail.BeginInit();
                Thumbnail.CacheOption = BitmapCacheOption.OnLoad;
                Thumbnail.StreamSource = stream;
                Thumbnail.DecodePixelWidth = 80;
                Thumbnail.EndInit();
                stream.Close();
            }
        }

        public Boolean Exists { get => (File.Exists(CurrentPath) || Directory.Exists(CurrentPath)); }

        /// <summary>
        /// 対象がディレクトリであるかを取得します。対象がファイルであるか、存在しない場合は false を返します。
        /// </summary>
        public Boolean IsDirectory { get => (Directory.Exists(CurrentPath)); }

        public string Type { get => (IsDirectory) ? "[DIR]" : Content.Extension; }

        public BitmapImage Thumbnail {
            get; private set;
        } = new BitmapImage();

        public Boolean IsImageFile {
            get => new String[] { ".jpg", ".png", ".bmp" }.Contains(Content.Extension);
        }

        /// <summary>
        /// CurrentPath の値を使用してファイルを新規作成します。
        /// このメソッドを呼び出すと、Content に FileInfo がセットされます。
        /// </summary>
        public void createFile() {
            var f = new FileInfo(CurrentPath);
            File.Create(f.FullName).Close();
            Content = f;
        }

        /// <summary>
        /// CurrentPath の値を使用してディレクトリを作成します。
        /// このメソッドを呼び出すと、Content に DirectoryInfo がセットされます。
        /// </summary>
        public void createDirectory() {
            var d = new DirectoryInfo(CurrentPath);
            Directory.CreateDirectory(CurrentPath);
            Content = d;
        }

        public void delete() {
            if (IsDirectory) {
                FileSystem.DeleteDirectory(
                    Content.FullName,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin
                );
            }
            else {
                FileSystem.DeleteFile(
                    Content.FullName,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin
                );
            }
        }

        public void copyTo(string destinationPath) {
            if (IsDirectory) {
                FileSystem.CopyDirectory(
                    Content.FullName,
                    destinationPath + "\\" + Content.Name,
                    UIOption.AllDialogs,
                    UICancelOption.DoNothing
                    );
            }
            else {
                FileSystem.CopyFile(
                    Content.FullName,
                    destinationPath + "\\" + Content.Name,
                    UIOption.AllDialogs,
                    UICancelOption.DoNothing
                    );
            }
        }

        public void moveTo(string destinationDirectoryPath) {
            if (IsDirectory) {
                FileSystem.MoveDirectory(
                    Content.FullName,
                    destinationDirectoryPath + "\\" + Content.Name,
                    UIOption.AllDialogs,
                    UICancelOption.DoNothing);
            }
            else {
                FileSystem.MoveFile(
                    Content.FullName,
                    destinationDirectoryPath + "\\" + Content.Name,
                    UIOption.AllDialogs,
                    UICancelOption.DoNothing);
            }
        }
    }
}
