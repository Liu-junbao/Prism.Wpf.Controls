using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Controls
{
    public class MoveWipeSelector : ITransitionWipeSelector
    {
        public MoveWipeSelector()
        {
            ForwardWipe = new SlideWipe() { Direction = SlideDirection.Left };
            BackwardWipe = new SlideWipe() { Direction = SlideDirection.Right };
        }
        public ITransitionWipe ForwardWipe { get; set; }
        public ITransitionWipe BackwardWipe { get; set; }
        public ITransitionWipe ProviderTransitionWipeFrom(FrameworkElement oldPresenter, FrameworkElement newPresenter, ITransitionContainer container)
        {
            return container.Indexof(newPresenter) > container.Indexof(oldPresenter) ? ForwardWipe : BackwardWipe;
        }
    }
}
