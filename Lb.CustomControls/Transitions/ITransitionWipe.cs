using System.Windows;

namespace System.Windows.Controls
{
    public interface ITransitionWipe
    {
        void Wipe(FrameworkElement oldPresenter, FrameworkElement newPresenter, Point origin, ITransitionContainer container);
    }
}