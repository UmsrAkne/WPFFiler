using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFFiler.models;

namespace WPFFiler.viewModels {
    class MainWindowViewModel : BindableBase {
        public FileList FileList {
            get;
            private set;
        } = new FileList(@"C:\");

        public FileListControlCommands FileListControlCommands {
            get;
            private set;
        }

        public MainWindowViewModel() {
            FileListControlCommands = new FileListControlCommands(FileList);
        }

    }
}
