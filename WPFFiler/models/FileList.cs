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

        private ViewStyle viewStyle = ViewStyle.ListView;
        public ViewStyle ViewStyle {
            get => viewStyle;
            set => SetProperty(ref viewStyle, value);
        }

        private ObservableCollection<ExFile> files = new ObservableCollection<ExFile>();
        public ObservableCollection<ExFile> Files {
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

            SelectedIndex = 0;
        }
    }
}
