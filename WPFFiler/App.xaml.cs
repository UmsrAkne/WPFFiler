using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using WPFFiler.ViewModels;
using WPFFiler.Views;

namespace WPFFiler {
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App {
        protected override Window CreateShell() {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) {
            // ビューとビューモデルを登録
            containerRegistry.RegisterDialog<InputDialog, InputDialogViewModel>();
        }
    }
}
