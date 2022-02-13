namespace WPFFiler.models
{

    using Prism.Commands;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.IO;
    using System.Windows.Controls;
    using Prism.Services.Dialogs;
    using WPFFiler.Views;
    using WPFFiler.ViewModels;

    public class FileListControlCommands
    {

        private int repeatCount = 0;
        private FileList mainFileList;
        private FileList subFileList;
        private IDialogService dialogService;

        public FileListControlCommands(IDialogService ds, FileList main, FileList sub)
        {
            mainFileList = main;
            subFileList = sub;
            dialogService = ds;
        }

        private DelegateCommand moveCursorToEndCommand;
        public DelegateCommand MoveCursorToEndCommand
        {
            get => moveCursorToEndCommand ?? (moveCursorToEndCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        FileList currentFileList = getFileListFromListView(lv);
                        currentFileList.SelectedIndex = currentFileList.Files.Count - 1;
                    }
                }
            ));
        }

        private DelegateCommand moveCursorToHeadCommand;
        public DelegateCommand MoveCursorToHeadCommand
        {
            get => moveCursorToHeadCommand ?? (moveCursorToHeadCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        FileList fl = getFileListFromListView(lv);
                        if (repeatCount == 0)
                        {
                            fl.SelectedIndex = 0;

                        }
                        else
                        {
                            fl.SelectedIndex = repeatCount;
                            repeatCount = 0;
                        }
                    }
                }
            ));
        }

        private DelegateCommand downCursorCommand;
        public DelegateCommand DownCursorCommand
        {
            get => downCursorCommand ?? (downCursorCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);

                        var action = new Action(() =>
                        {
                            fl.SelectedIndex++;
                        });

                        if (repeatCount == 0)
                        {
                            action();
                        }
                        else
                        {
                            repeatCommand(action);
                        }
                    }
                }
            ));
        }

        private DelegateCommand upCursorCommand;
        public DelegateCommand UpCursorCommand
        {
            get => upCursorCommand ?? (upCursorCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);
                        var action = new Action(() =>
                        {
                            fl.SelectedIndex--;
                        });

                        if (repeatCount == 0)
                        {
                            action();
                        }
                        else
                        {
                            repeatCommand(action);
                        }
                    }
                }
            ));
        }

        private DelegateCommand pageUpCommand;
        public DelegateCommand PageUpCommand
        {
            get => pageUpCommand ?? (pageUpCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);

                        Action action = new Action(() =>
                        {
                            if (fl.Files.Count > 0)
                            {
                                lv.UpdateLayout();
                                var lbItem = lv.ItemContainerGenerator.ContainerFromItem(lv.Items.GetItemAt(fl.SelectedIndex)) as ListBoxItem;

                                // -1 ではなく -2 なのでは、listBox.ActualHeight に header も含まれていると思われるためその分 -1
                                int itemDisplayCapacity = (int)Math.Floor(lv.ActualHeight / lbItem.ActualHeight) - 2;
                                fl.SelectedIndex -= itemDisplayCapacity;
                            }
                        });

                        if (repeatCount == 0)
                        {
                            action();
                        }
                        else
                        {
                            repeatCommand(action);
                        }
                    }
                }
            ));
        }

        private DelegateCommand pageDownCommand;
        public DelegateCommand PageDownCommand
        {
            get => pageDownCommand ?? (pageDownCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);
                        Action action = new Action(() =>
                        {
                            if (fl.Files.Count > 0)
                            {
                                lv.UpdateLayout();
                                var lbItem = lv.ItemContainerGenerator.ContainerFromItem(lv.Items.GetItemAt(fl.SelectedIndex)) as ListBoxItem;

                                // -1 ではなく -2 なのでは、listBox.ActualHeight に header も含まれていると思われるためその分 -1
                                int itemDisplayCapacity = (int)Math.Floor(lv.ActualHeight / lbItem.ActualHeight) - 2;
                                fl.SelectedIndex += itemDisplayCapacity;
                            }
                        });

                        if (repeatCount == 0)
                        {
                            action();
                        }
                        else
                        {
                            repeatCommand(action);
                        }
                    }

                }
            ));
        }

        private DelegateCommand reloadCommand;
        public DelegateCommand ReloadCommand
        {
            get => reloadCommand ?? (reloadCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);
                        fl?.reload();
                    }
                }
            ));
        }

        private DelegateCommand syncCurrentDirectoryCommand;
        public DelegateCommand SyncCurrentDirectoryCommand
        {
            get => syncCurrentDirectoryCommand ?? (syncCurrentDirectoryCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);
                        var af = getAnotherFileList(fl);
                        af.CurrentDirectoryPath = fl.CurrentDirectoryPath;
                    }
                }
            ));
        }

        private DelegateCommand syncFromSubCurrentDirectoryCommand;
        public DelegateCommand SyncFromSubCurrentDirectoryCommand
        {
            get => syncFromSubCurrentDirectoryCommand ?? (syncFromSubCurrentDirectoryCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);
                        var af = getAnotherFileList(fl);
                        fl.CurrentDirectoryPath = af.CurrentDirectoryPath;
                    }
                }
            ));
        }

        private DelegateCommand openCommand;
        public DelegateCommand OpenCommand
        {
            get => openCommand ?? (openCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {

                        var fl = getFileListFromListView(lv);
                        ExFile currentFile = (ExFile)fl.Files[fl.SelectedIndex];
                        if (currentFile.IsDirectory)
                        {
                            fl.CurrentDirectoryPath = currentFile.Content.FullName;

                            // ディレクトリの中身が存在する場合はスクロール処理も行う
                            if (fl.Files.Count > 0)
                            {
                            }
                        }
                        else
                        {
                            System.Diagnostics.Process.Start(currentFile.Content.FullName);
                        }
                    }
                }
            ));
        }

        private DelegateCommand moveToParentDirectory;
        public DelegateCommand MoveToParentDirectory
        {
            get => moveToParentDirectory ?? (moveToParentDirectory = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);
                        if (repeatCount == 0)
                        {
                            fl.CurrentDirectoryPath = new DirectoryInfo(fl.CurrentDirectoryPath).Parent.FullName;
                        }
                        else
                        {
                            repeatCommand(() =>
                            {
                                var parentDirectory = new DirectoryInfo(fl.CurrentDirectoryPath).Parent;
                                if (parentDirectory != null)
                                {
                                    fl.CurrentDirectoryPath = parentDirectory.FullName;
                                }
                            });
                        }
                    }
                },
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv == null)
                    {
                        return false;
                    }

                    var fl = getFileListFromListView(lv);
                    return (new DirectoryInfo(fl.CurrentDirectoryPath).Parent != null);
                }
            ));
        }

        private DelegateCommand<string> moveToDirectory;
        public DelegateCommand<string> MoveToDirectory
        {
            get => moveToDirectory ?? (moveToDirectory = new DelegateCommand<string>(
                path => mainFileList.CurrentDirectoryPath = path,
                path => new DirectoryInfo(path).Exists
            ));
        }

        private DelegateCommand<string> moveToDirectoryForSubFileList;
        public DelegateCommand<string> MoveToDirectoryForSubFileList
        {
            get => moveToDirectoryForSubFileList ?? (moveToDirectoryForSubFileList = new DelegateCommand<string>(
                path => subFileList.CurrentDirectoryPath = path,
                path => new DirectoryInfo(path).Exists
            ));
        }

        private DelegateCommand createDirectoryCommand;
        public DelegateCommand CreateDirectoryCommand
        {
            get => createDirectoryCommand ?? (createDirectoryCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();

                    if (lv == null)
                    {
                        return;
                    }

                    var fl = getFileListFromListView(lv);

                    dialogService.ShowDialog(nameof(InputDialog), new DialogParameters(),
                        (IDialogResult result) =>
                        {
                            System.Diagnostics.Debug.WriteLine(result.Parameters.GetValue<string>("InputText"));
                            if (result != null)
                            {
                                string r = result.Parameters.GetValue<string>(nameof(InputDialogViewModel.InputText));
                                if (!string.IsNullOrEmpty(r))
                                {
                                    ExFile directory = new ExFile(fl.CurrentDirectoryPath + "\\" + r);
                                    directory.createDirectory();
                                    fl.reload();
                                }
                            }
                        });
                }
            ));
        }

        private DelegateCommand deleteMarkedFilesCommand;
        public DelegateCommand DeleteMarkedFilesCommand
        {
            get => deleteMarkedFilesCommand ?? (deleteMarkedFilesCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);
                        fl.MarkedFiles.ForEach((ExFile f) => { f.delete(); });
                        fl.reload();
                        fl.SelectedIndex = 0;
                    }
                }
            ));
        }

        private DelegateCommand toggleMarkCommand;
        public DelegateCommand ToggleMarkCommand
        {
            get => toggleMarkCommand ?? (toggleMarkCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);
                        var file = fl.Files[fl.SelectedIndex];
                        file.IsMarked = !file.IsMarked;
                        fl.raiseMakedFilesChanged();
                    }
                }
            ));
        }

        private DelegateCommand markCommand;
        public DelegateCommand MarkCommand
        {
            get => markCommand ?? (markCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);
                        Action action = () =>
                        {
                            var file = fl.Files[fl.SelectedIndex];
                            file.IsMarked = true;
                            fl.SelectedIndex++;
                            fl.raiseMakedFilesChanged();
                        };

                        if (repeatCount == 0)
                        {
                            action();
                        }
                        else
                        {
                            repeatCommand(action);
                        }
                    }
                }
            ));
        }

        private DelegateCommand unmarkCommand;
        public DelegateCommand UnmarkCommand
        {
            get => unmarkCommand ?? (unmarkCommand = new DelegateCommand(
                () =>
                {
                    var lv = getFocusingListView();
                    if (lv != null)
                    {
                        var fl = getFileListFromListView(lv);
                        Action action = () =>
                        {
                            var file = fl.Files[fl.SelectedIndex];
                            file.IsMarked = false;
                            fl.SelectedIndex++;
                            fl.raiseMakedFilesChanged();
                        };

                        if (repeatCount == 0)
                        {
                            action();
                        }
                        else
                        {
                            repeatCommand(action);
                        }
                    }
                }
            ));
        }

        private DelegateCommand copyFileCommand;
        public DelegateCommand CopyFileCommand
        {
            get => copyFileCommand ?? (copyFileCommand = new DelegateCommand(
                () =>
                {
                    var fileList = getFileListFromListView(getFocusingListView());
                    var anotherFileList = getAnotherFileList(fileList);

                    fileList.MarkedFiles.ForEach((f) =>
                    {
                        f.copyTo(anotherFileList.CurrentDirectoryPath);
                    });

                    fileList.reload();
                    anotherFileList.reload();
                },
                () => getFocusingListView() != null
            ));
        }

        private DelegateCommand moveFileCommand;
        public DelegateCommand MoveFileCommand
        {
            get => moveFileCommand ?? (moveFileCommand = new DelegateCommand(
                () =>
                {
                    var fileList = getFileListFromListView(getFocusingListView());
                    var anotherFileList = getAnotherFileList(fileList);

                    fileList.MarkedFiles.ForEach((f) =>
                    {
                        f.moveTo(anotherFileList.CurrentDirectoryPath);
                    });

                    fileList.reload();
                    anotherFileList.reload();
                },
                () =>
                {
                    return (getFocusingListView() != null &&
                            mainFileList.CurrentDirectoryPath != subFileList.CurrentDirectoryPath);
                }
            ));
        }

        private DelegateCommand<ListBox> changeLeftViewStyleToListViewStyleCommand;
        public DelegateCommand<ListBox> ChangeLeftViewStyleToListViewStyleCommand
        {
            get => changeLeftViewStyleToListViewStyleCommand ?? (changeLeftViewStyleToListViewStyleCommand = new DelegateCommand<ListBox>(
                (lb) => getFileListFromListView(lb).LeftViewStyle = ViewStyle.ListView
            ));
        }

        private DelegateCommand<ListBox> changeLeftViewStyleToListBoxStyleCommand;
        public DelegateCommand<ListBox> ChangeLeftViewStyleToListBoxStyleCommand
        {
            get => changeLeftViewStyleToListBoxStyleCommand ?? (changeLeftViewStyleToListBoxStyleCommand = new DelegateCommand<ListBox>(
                (lb) => getFileListFromListView(lb).LeftViewStyle = ViewStyle.ListBox
            ));
        }

        private DelegateCommand<ListBox> changeRightViewStyleToListViewStyleCommand;
        public DelegateCommand<ListBox> ChangeRightViewStyleToListViewStyleCommand
        {
            get => changeRightViewStyleToListViewStyleCommand ?? (changeRightViewStyleToListViewStyleCommand = new DelegateCommand<ListBox>(
                (lb) => getFileListFromListView(lb).RightViewStyle = ViewStyle.ListView
            ));
        }

        private DelegateCommand<ListBox> changeRightViewStyleToListBoxStyleCommand;
        public DelegateCommand<ListBox> ChangeRightViewStyleToListBoxStyleCommand
        {
            get => changeRightViewStyleToListBoxStyleCommand ?? (changeRightViewStyleToListBoxStyleCommand = new DelegateCommand<ListBox>(
                (lb) => getFileListFromListView(lb).RightViewStyle = ViewStyle.ListBox
            ));
        }

        private DelegateCommand<object> focusCommand;
        public DelegateCommand<object> FocusCommand
        {
            get => focusCommand ?? (focusCommand = new DelegateCommand<object>(
                (object param) =>
                {
                    var view = (System.Windows.Controls.Control)param;
                    view.Focus();
                }
            ));
        }

        private DelegateCommand<object> focusToURLBarCommandCommand;
        public DelegateCommand<object> FocusToURLBarCommandCommand
        {
            get => focusToURLBarCommandCommand ?? (focusToURLBarCommandCommand = new DelegateCommand<object>(
                (object param) =>
                {
                    var textBox = (TextBox)param;
                    textBox.Focus();
                    textBox.SelectAll();
                }
            ));
        }

        private DelegateCommand<object> setRepeatCountCommand;
        public DelegateCommand<object> SetRepeatCountCommand
        {
            get => setRepeatCountCommand ?? (setRepeatCountCommand = new DelegateCommand<object>(
                (object param) =>
                {

                    string numberString = param.ToString().Substring(1);
                    // パラメーターに入ってくる文字列は "d0" から "d9" までの10種類。
                    // dxの数字部分が、数字キーと対応している。

                    if (repeatCount == 0)
                    {
                        repeatCount = int.Parse(numberString);
                    }
                    else
                    {
                        string stringNumbers = repeatCount.ToString() + numberString;
                        repeatCount = int.Parse(stringNumbers);
                    }

                    if (repeatCount > 100)
                    {
                        repeatCount = 100;
                    }

                }
            ));
        }

        /// <summary>
        /// 現在キーボードフォーカスが当たっている ListView を取得します。
        /// </summary>
        /// <returns> ListView がフォーカスを持っていない場合は null を返します。</returns>
        private ListBox getFocusingListView()
        {
            if (Keyboard.FocusedElement == null)
            {
                return null;
            }

            var obj = (System.Windows.DependencyObject)Keyboard.FocusedElement;
            while (!(obj is ListBox))
            {
                obj = System.Windows.Media.VisualTreeHelper.GetParent(obj);

                if (obj == null)
                {
                    break;
                }
            }

            return (obj != null) ? (ListBox)obj : null;
        }


        /// <summary>
        /// ListView から、それに紐付いている FileList モデルを取得します。
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        private FileList getFileListFromListView(ListBox lv)
        {
            if (ReferenceEquals(mainFileList.Files, lv.ItemsSource))
            {
                return mainFileList;
            }

            if (ReferenceEquals(subFileList.Files, lv.ItemsSource))
            {
                return subFileList;
            }

            return null;
        }

        /// <summary>
        /// mainFileList, subFileList のうち、引数に入力した方でない方の FileList を取得します。
        /// </summary>
        /// <param name="fl"></param>
        /// <returns></returns>
        private FileList getAnotherFileList(FileList fl)
        {
            if ((mainFileList == fl) == (subFileList == fl))
            {
                throw new ArgumentException("入力される FileList は、 mainFileList, subFileList のいずれか一方とのみ同一のインスタンスでなければなりません。");
            }

            return (mainFileList == fl) ? subFileList : mainFileList;
        }

        /// <summary>
        /// repeatCount の回数だけ action を実行し、実行後に repeatCount を 0 にセットします
        /// </summary>
        /// <param name="action"></param>
        private void repeatCommand(Action action)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                action();
            }

            repeatCount = 0;
        }

    }
}
