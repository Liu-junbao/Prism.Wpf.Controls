using System.Windows;
using System.Windows.Media.Animation;

namespace System.Windows.Controls
{
    public interface ITransitionEffect
    {
        Timeline Build<TSubject>(TSubject effectSubject) where TSubject : FrameworkElement, ITransitionEffectSubject;
    }
}