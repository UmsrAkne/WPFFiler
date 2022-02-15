namespace WPFFiler.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Xaml.Behaviors;

    public class ScrollBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += ScrollView;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.SelectionChanged -= ScrollView;
        }

        private void ScrollView(object sender, EventArgs e)
        {
            var lb = (ListBox)sender;
            lb.ScrollIntoView(lb.SelectedItem);
        }
    }
}
