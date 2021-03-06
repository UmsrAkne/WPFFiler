﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ViewStyle {
    ListView,
    ListBox
}

namespace WPFFiler.models {
    public class FileList : BindableBase{

        private ViewStyle leftViewStyle = ViewStyle.ListView;
        public ViewStyle LeftViewStyle {
            get => leftViewStyle;
            set => SetProperty(ref leftViewStyle, value);
        }

        private ViewStyle rightViewStyle = ViewStyle.ListView;
        public ViewStyle RightViewStyle {
            get => rightViewStyle;
            set => SetProperty(ref rightViewStyle, value);
        }

        public bool BothViewBinding { get; set; } = false;

        private List<ExFile> files = new List<ExFile>();
        public List<ExFile> Files {
            get => files;
            set => SetProperty(ref files, value);
        }

        private string currentDirectoryPath = "";
        public string CurrentDirectoryPath {
            get => currentDirectoryPath;
            set {
                if(Directory.Exists(value)) {
                    SetProperty(ref currentDirectoryPath, value);
                    reload();
                }
            }
        }

        private int selectedIndex = 0;
        public int SelectedIndex {
            get => selectedIndex;
            set {
                if(value >= Files.Count) {
                    SetProperty(ref selectedIndex, Files.Count - 1);
                    return;
                }

                if (value <= 0) {
                    SetProperty(ref selectedIndex, 0);
                    return;
                }

                SetProperty(ref selectedIndex, value);
            }
        }

        public FileList(string baseDirectoryPath) {
            currentDirectoryPath = baseDirectoryPath;
            reload();
        }

        public void reload() {
            string[] paths = Directory.GetFiles(CurrentDirectoryPath);
            string[] directoryPaths = Directory.GetDirectories(CurrentDirectoryPath);

            List<ExFile> allFiles = new List<ExFile>();
            foreach(string p in paths) {
                allFiles.Add(new ExFile(p));
            }

            foreach(string dp in directoryPaths) {
                allFiles.Add(new ExFile(dp));
            }

            Files = allFiles;
            SelectedIndex = 0;
        }

        public List<ExFile> MarkedFiles {
            get {
                return Files.Where((f) => f.IsMarked).ToList();
            }
        }

        public void raiseMakedFilesChanged() {
            RaisePropertyChanged(nameof(MarkedFiles));
        }
    }
}
