using System.Windows;

namespace Lb.CustomControls
{
    public interface ITransitionWipe
    {
        void Wipe(FrameworkElement oldPresenter, FrameworkElement newPresenter, Point origin, ITransitionContainer container);
    }
}