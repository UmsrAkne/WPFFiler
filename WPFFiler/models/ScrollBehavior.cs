using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace WPFFiler.models
{
    class ScrollBehavior : Behavior<ListBox > {

        protected override void OnAttached() {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += scrollView;
        }

        protected override void OnDetaching() {
            base.OnDetaching();
            this.AssociatedObject.SelectionChanged -= scrollView;
        }

        private void scrollView(object sender, EventArgs e) {
            var lb = (ListBox)sender;
            lb.ScrollIntoView(lb.SelectedItem);
        }
    }
}
