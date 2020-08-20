using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public Boolean Exists { get => (File.Exists(CurrentPath) || Directory.Exists(CurrentPath)); }

        /// <summary>
        /// 対象がディレクトリであるかを取得します。対象がファイルであるか、存在しない場合は false を返します。
        /// </summary>
        public Boolean IsDirectory { get => (Directory.Exists(CurrentPath)); }

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
            Content.Delete();
        }

    }
}
