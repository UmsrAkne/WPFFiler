using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFFiler.models {
    public class FileListControlCommands {

        private FileList mainFileList;

        public FileListControlCommands(FileList main) {
            mainFileList = main;
        }

        private DelegateCommand downCursorCommand;
        public DelegateCommand DownCursorCommand {
            get => downCursorCommand ?? (downCursorCommand = new DelegateCommand(
                () => {
                    mainFileList.SelectedIndex++;
                }
            ));
        }

        private DelegateCommand upCursorCommand;
        public DelegateCommand UpCursorCommand {
            get => upCursorCommand ?? (upCursorCommand = new DelegateCommand(
                () => {
                    mainFileList.SelectedIndex--;
                }
            ));
        }

        private DelegateCommand reloadCommand;
        public DelegateCommand ReloadCommand {
            get => reloadCommand ?? (reloadCommand = new DelegateCommand(
                () => {
                    mainFileList.reload();
                }
            ));
        }
    }
}
