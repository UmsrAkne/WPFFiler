using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows.Controls;

namespace WPFFiler.models {
    public class FileListControlCommands {

        private int repeatCount = 0;
        private FileList mainFileList;

        public FileListControlCommands(FileList main) {
            mainFileList = main;
        }

        private DelegateCommand<ListBox> moveCursorToEndCommand;
        public DelegateCommand<ListBox> MoveCursorToEndCommand {
            get => moveCursorToEndCommand ?? (moveCursorToEndCommand = new DelegateCommand<ListBox>(
                (listBox) => {
                    mainFileList.SelectedIndex = mainFileList.Files.Count - 1;
                    listBox.ScrollIntoView(listBox.SelectedItem);
                }
            ));
        }

        private DelegateCommand<ListBox> moveCursorToHeadCommand;
        public DelegateCommand<ListBox> MoveCursorToHeadCommand {
            get => moveCursorToHeadCommand ?? (moveCursorToHeadCommand = new DelegateCommand<ListBox>(
                (listBox) => {
                    if(repeatCount == 0) {
                        mainFileList.SelectedIndex = 0;
                        listBox.ScrollIntoView(listBox.SelectedItem);
                        
                    }
                    else {
                        mainFileList.SelectedIndex = repeatCount;
                        listBox.ScrollIntoView(listBox.SelectedItem);
                        repeatCount = 0;
                    }
                }
            ));
        }

        private DelegateCommand<ListBox> downCursorCommand;
        public DelegateCommand<ListBox> DownCursorCommand {
            get => downCursorCommand ?? (downCursorCommand = new DelegateCommand<ListBox>(
                (listBox) => {

                    var action = new Action(() => {
                        mainFileList.SelectedIndex++;
                        listBox.ScrollIntoView(listBox.SelectedItem);
                    });

                    if(repeatCount == 0) {
                        action();
                    }
                    else {
                        repeatCommand(action);
                    }
                }
            ));
        }

        private DelegateCommand<ListBox> upCursorCommand;
        public DelegateCommand<ListBox> UpCursorCommand {
            get => upCursorCommand ?? (upCursorCommand = new DelegateCommand<ListBox>(
                (listBox) => {

                    var action = new Action(() => {
                        mainFileList.SelectedIndex--;
                        listBox.ScrollIntoView(listBox.SelectedItem);
                    });

                    if(repeatCount == 0) {
                        action();
                    }
                    else {
                        repeatCommand(action);
                    }
                }
            ));
        }

        private DelegateCommand<ListBox> pageUpCommand;
        public DelegateCommand<ListBox> PageUpCommand {
            get => pageUpCommand ?? (pageUpCommand = new DelegateCommand<ListBox>(
                (listBox) => {
                    Action action = new Action(() => {
                        if(mainFileList.Files.Count > 0) {
                            listBox.UpdateLayout();
                            var lbItem = listBox.ItemContainerGenerator.ContainerFromItem(listBox.Items.GetItemAt(mainFileList.SelectedIndex)) as ListBoxItem;

                            // -1 ではなく -2 なのでは、listBox.ActualHeight に header も含まれていると思われるためその分 -1
                            int itemDisplayCapacity = (int)Math.Floor(listBox.ActualHeight / lbItem.ActualHeight) - 2;
                            mainFileList.SelectedIndex -= itemDisplayCapacity;
                            listBox.ScrollIntoView(listBox.SelectedItem);
                        }
                    });

                    if(repeatCount == 0) {
                        action();
                    }
                    else {
                        repeatCommand(action);
                    }
                }
            ));
        }

        private DelegateCommand<ListBox> pageDownCommand;
        public DelegateCommand<ListBox> PageDownCommand {
            get => pageDownCommand ?? (pageDownCommand = new DelegateCommand<ListBox>(
                (listBox) => {
                    Action action = new Action(() => {
                        if(mainFileList.Files.Count > 0) {
                            listBox.UpdateLayout();
                            var lbItem = listBox.ItemContainerGenerator.ContainerFromItem(listBox.Items.GetItemAt(mainFileList.SelectedIndex)) as ListBoxItem;

                            // -1 ではなく -2 なのでは、listBox.ActualHeight に header も含まれていると思われるためその分 -1
                            int itemDisplayCapacity = (int)Math.Floor(listBox.ActualHeight / lbItem.ActualHeight) - 2;
                            mainFileList.SelectedIndex += itemDisplayCapacity;
                            listBox.ScrollIntoView(listBox.SelectedItem);
                        }
                    });

                    if(repeatCount == 0) {
                        action();
                    }
                    else {
                        repeatCommand(action);
                    }
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

        private DelegateCommand<ListBox> openDirectoryCommand;
        public DelegateCommand<ListBox> OpenDirectoryCommand {
            get => openDirectoryCommand ?? (openDirectoryCommand = new DelegateCommand<ListBox>(
                (listBox) => {
                    mainFileList.CurrentDirectoryPath = mainFileList.Files[mainFileList.SelectedIndex].Content.FullName;

                    if(mainFileList.Files.Count > 0) {
                        listBox.ScrollIntoView(listBox.Items.GetItemAt(0));
                    }
                },
                (listBox) => mainFileList.Files[mainFileList.SelectedIndex].IsDirectory
            ));
        }

        private DelegateCommand moveToParentDirectory;
        public DelegateCommand MoveToParentDirectory {
            get => moveToParentDirectory ?? (moveToParentDirectory = new DelegateCommand(
                () => {
                    if(repeatCount == 0) {
                        mainFileList.CurrentDirectoryPath = new DirectoryInfo(mainFileList.CurrentDirectoryPath).Parent.FullName;
                    }
                    else {
                        repeatCommand(() => {
                            var parentDirectory = new DirectoryInfo(mainFileList.CurrentDirectoryPath).Parent;
                            if(parentDirectory != null) {
                                mainFileList.CurrentDirectoryPath = parentDirectory.FullName;
                            }
                        });
                    }
                },
                () => new DirectoryInfo(mainFileList.CurrentDirectoryPath).Parent != null
            ));
        }

        private DelegateCommand<string> moveToDirectory;
        public DelegateCommand<string> MoveToDirectory {
            get => moveToDirectory ?? (moveToDirectory = new DelegateCommand<string>(
                path => mainFileList.CurrentDirectoryPath = path,
                path => new DirectoryInfo(path).Exists
            ));
        }

        private DelegateCommand createDirectoryCommand;
        public DelegateCommand CreateDirectoryCommand {
            get => createDirectoryCommand ?? (createDirectoryCommand = new DelegateCommand(
                () => {
                    ExFile directory = new ExFile(mainFileList.CurrentDirectoryPath + @"\testDirectory");
                    directory.createDirectory();
                    mainFileList.reload();
                }
            ));
        }

        private DelegateCommand<object> focusCommand;
        public DelegateCommand<object> FocusCommand { 
            get => focusCommand ?? (focusCommand = new DelegateCommand<object>(
                (object param) => {
                    var view = (System.Windows.Controls.Control)param;
                    view.Focus();
                }
            ));
        }

        private DelegateCommand<object> focusToURLBarCommandCommand;
        public DelegateCommand<object> FocusToURLBarCommandCommand { 
            get => focusToURLBarCommandCommand ?? (focusToURLBarCommandCommand = new DelegateCommand<object>(
                (object param) => {
                    var textBox = (TextBox)param;
                    textBox.Focus();
                    textBox.SelectAll();
                }
            ));
        }

        private DelegateCommand<object> setRepeatCountCommand;
        public DelegateCommand<object> SetRepeatCountCommand {
            get => setRepeatCountCommand ?? (setRepeatCountCommand = new DelegateCommand<object>(
                (object param) => {

                    string numberString = param.ToString().Substring(1);
                    // パラメーターに入ってくる文字列は "d0" から "d9" までの10種類。
                    // dxの数字部分が、数字キーと対応している。

                    if(repeatCount == 0) {
                        repeatCount = int.Parse(numberString);
                    }
                    else {
                        string stringNumbers = repeatCount.ToString() + numberString;
                        repeatCount = int.Parse(stringNumbers);
                    }

                    if(repeatCount > 100) {
                        repeatCount = 100;
                    }

                }
            ));
        }

        /// <summary>
        /// repeatCount の回数だけ action を実行し、実行後に repeatCount を 0 にセットします
        /// </summary>
        /// <param name="action"></param>
        private void repeatCommand(Action action) {
            for(int i = 0; i < repeatCount; i++) {
                action();
            }

            repeatCount = 0;
        }

    }
}
