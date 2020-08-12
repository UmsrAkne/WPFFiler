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
            FileSystemInfo f;
            CurrentPath = path;
            if (!Exists(path)) {
                return;
            }

            Content = (Directory.Exists(path)) ? (FileSystemInfo)new DirectoryInfo(path) : (FileSystemInfo)new FileInfo(path);
        }

        public Boolean Exists(string path) {
            return (File.Exists(path) || Directory.Exists(path));
        }

    }
}
