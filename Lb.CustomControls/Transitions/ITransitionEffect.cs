using System.Windows;
using System.Windows.Media.Animation;

namespace Lb.CustomControls.Transitions
{
    public interface ITransitionEffect
    {
        Timeline Build<TSubject>(TSubject effectSubject) where TSubject : FrameworkElement, ITransitionEffectSubject;
    }
}