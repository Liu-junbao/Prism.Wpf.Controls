using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lb.CustomControls
{
    [StyleTypedProperty(Property = nameof(ItemContainerStyle), StyleTargetType = typeof(BrowserBoxItem))]
    public class BrowserBox : ListBox
    {
        static BrowserBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BrowserBox), new FrameworkPropertyMetadata(typeof(BrowserBox)));
        }
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is BrowserBoxItem;
        }
    }
    public class BrowserBoxItem : ListBoxItem
    {
        public static readonly DependencyProperty CanCloseProperty =
          DependencyProperty.Register(nameof(CanClose), typeof(bool), typeof(BrowserBoxItem), new PropertyMetadata(true));
        public static readonly DependencyProperty CloseCommandProperty =
          DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(BrowserBoxItem), new PropertyMetadata(null));
        public bool CanClose
        {
            get { return (bool)GetValue(CanCloseProperty); }
            set { SetValue(CanCloseProperty, value); }
        }
        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
    }
}
