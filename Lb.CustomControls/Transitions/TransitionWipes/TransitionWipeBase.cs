using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace System.Windows.Controls
{
    public abstract class TransitionWipeBase : ITransitionWipe
    {
       
        public abstract void Wipe(FrameworkElement oldPresenter, FrameworkElement newPresenter, Point origin, ITransitionContainer zIndexController);
        protected void CompletedWithEnd(Storyboard storyboard,ITransitionContainer container,Action completedAction)
        {
            EventHandler endHandler = null;
            endHandler = (s, e) =>
            {
                completedAction?.Invoke();
                storyboard.Completed -= endHandler;
                container.TransitionChanged -= endHandler;
            };
            storyboard.Completed += endHandler;
            container.TransitionChanged += endHandler;        
        }
    }
}
