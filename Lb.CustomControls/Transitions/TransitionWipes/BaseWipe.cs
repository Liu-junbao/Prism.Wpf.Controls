using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace System.Windows.Controls
{
    public abstract class BaseWipe : ITransitionWipe
    {
        public abstract void Wipe(FrameworkElement oldPresenter, FrameworkElement newPresenter, Point origin, ITransitionContainer zIndexController);

        protected void CompletedWithEnd(Timeline timeline,ITransitionContainer zIndexController,Action completedAction)
        {
            EventHandler endHandler = null;
            endHandler = (s, e) =>
            {
                completedAction?.Invoke();
                timeline.Completed -= endHandler;
                zIndexController.TransitionChanged -= endHandler;
            };
            timeline.Completed += endHandler;
            zIndexController.TransitionChanged += endHandler;        
        }
    }
}
