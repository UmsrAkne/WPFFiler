using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFFiler.models {
    class FileList : BindableBase{

        private ObservableCollection<ExFile> files = new ObservableCollection<ExFile>();
        public ObservableCollection<ExFile> Files {
            private get => files;
            set => SetProperty(ref files, value);
        }

        private string currentDirectoryPath = "";
        public string CurrentDirectoryPath {
            get => currentDirectoryPath;
            set => SetProperty(ref currentDirectoryPath, value);
        }

        public FileList(string baseDirectoryPath) {
            currentDirectoryPath = baseDirectoryPath;
            reload();
        }

        public void reload() {
            string[] paths = Directory.GetFiles(CurrentDirectoryPath);
            string[] directoryPaths = Directory.GetDirectories(CurrentDirectoryPath);

            Files.Clear();
            List<ExFile> tempFiles = new List<ExFile>();
            foreach(string p in paths) {
                tempFiles.Add(new ExFile(p));
            }

            List<ExFile> tempDirectories = new List<ExFile>();
            foreach(string dp in directoryPaths) {
                tempDirectories.Add(new ExFile(dp));
            }

            Files.AddRange(tempFiles);
            files.AddRange(tempDirectories);
        }
    }
}
