﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFFiler.models {
    class FileList : BindableBase{

        private ObservableCollection<FileSystemInfo> files = new ObservableCollection<FileSystemInfo>();
        public ObservableCollection<FileSystemInfo> Files {
            private get => files;
            set => SetProperty(ref files, value);
        }

        public string currentDirectoryPath = "";

        public FileList(string baseDirectoryPath) {
            currentDirectoryPath = baseDirectoryPath;
            reload();
        }

        public void reload() {
            string[] paths = Directory.GetFiles(currentDirectoryPath);
            string[] directoryPaths = Directory.GetDirectories(currentDirectoryPath);

            Files.Clear();
            List<FileSystemInfo> tempFiles = new List<FileSystemInfo>();
            foreach(string p in paths) {
                tempFiles.Add(new FileInfo(p));
            }

            List<FileSystemInfo> tempDirectories = new List<FileSystemInfo>();
            foreach(string dp in directoryPaths) {
                tempDirectories.Add(new DirectoryInfo(dp));
            }

            Files.AddRange(tempFiles);
            files.AddRange(tempDirectories);
        }
    }
}
