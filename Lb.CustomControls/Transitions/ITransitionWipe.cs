using System.Windows;

namespace Lb.CustomControls.Transitions
{
    public interface ITransitionWipe
    {
        void Wipe(FrameworkElement oldPresenter, FrameworkElement newPresenter, Point origin, ITransitionContainer container);
    }
}