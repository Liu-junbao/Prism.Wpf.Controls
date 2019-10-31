using System;
using System.Windows;

namespace System.Windows.Controls
{
    public interface ITransitionContainer
    {
        event EventHandler TransitionChanged;
        void SetZIndexOrderBy(params FrameworkElement[] presenters);
        int Indexof(FrameworkElement presenter);
        int Indexof(object item);
        FrameworkElement FindPresenter(object item);
    }
}