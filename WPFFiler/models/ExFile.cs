using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFFiler.models {
    public class ExFile {

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

    }
}
