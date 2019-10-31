using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace System.Windows.Controls
{
    public class TransitionerBox : ContentControl, ITransitionContainer
    {
        public static readonly DependencyProperty WipeProperty =
           DependencyProperty.Register(nameof(Wipe), typeof(ITransitionWipe), typeof(TransitionerBox), new PropertyMetadata(null));



        public event EventHandler TransitionChanged;
        public TransitionerBox()
        {
            WipeStrategy = new TransitionWipeStrategy();
        }
        public TransitionWipeStrategy WipeStrategy { get; }
        public ITransitionWipe Wipe
        {
            get { return (ITransitionWipe)GetValue(WipeProperty); }
            set { SetValue(WipeProperty, value); }
        }
        private void ActivateFrame(FrameworkElement oldPresenter, FrameworkElement newPresenter)
        {
            if (!IsLoaded) return;
            if (newPresenter != null) newPresenter.Visibility = Visibility.Visible;
            if (oldPresenter != null && newPresenter != null)
            {
                ITransitionWipe wipe = null;
                if (WipeStrategy.Match(this, oldPresenter, newPresenter, unselectedIndex, selectedIndex, count, out wipe) == false)
                    wipe = selectedIndex > unselectedIndex ? oldPresenter.ForwardWipe : oldPresenter.BackwardWipe;
                if (wipe != null)
                {
                    wipe.Wipe(oldPresenter, newPresenter, GetTransitionOrigin(newPresenter), this);
                }
                else
                {
                    DoStack(newPresenter, oldPresenter);
                    if (oldPresenter != null) oldPresenter.Visibility = Visibility.Hidden;
                }
            }
            else if (oldPresenter != null || newPresenter != null)
            {
                DoStack(oldPresenter ?? newPresenter);
                if (oldPresenter != null) oldPresenter.Visibility = Visibility.Hidden;
            }

            _nextTransitionOrigin = null;
        }

        void ITransitionContainer.Stack(params UIElement[] highestToLowest)
        {

            DoStack(highestToLowest);
        }
        private static void DoStack(params UIElement[] highestToLowest)
        {
            if (highestToLowest == null) return;

            var pos = highestToLowest.Length;
            foreach (var slide in highestToLowest.Where(s => s != null))
            {
                Panel.SetZIndex(slide, pos--);
            }
        }

    }
}
