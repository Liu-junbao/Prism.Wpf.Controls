using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace System.Windows.Controls
{
    public class FadeWipe :TransitionWipeBase
    {
        private readonly SineEase _sineEase = new SineEase();

        /// <summary>
        /// Duration of the animation
        /// </summary>
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(3);

        public override void Wipe(FrameworkElement oldPresenter, FrameworkElement newPresenter, Point origin, ITransitionContainer zIndexController)
        {
            if (oldPresenter == null) throw new ArgumentNullException(nameof(oldPresenter));
            if (newPresenter == null) throw new ArgumentNullException(nameof(newPresenter));
            if (zIndexController == null) throw new ArgumentNullException(nameof(zIndexController));

            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget

            // Set up time points
            var zeroKeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
            var endKeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(Duration.TotalSeconds/2));

            // From
            var fromAnimation = new DoubleAnimationUsingKeyFrames();
            fromAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(1, zeroKeyTime));
            fromAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, endKeyTime, _sineEase));

            // To
            var toAnimation = new DoubleAnimationUsingKeyFrames();
            toAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0, zeroKeyTime));
            toAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, endKeyTime, _sineEase));

            // Preset
            oldPresenter.Opacity = 1;
            newPresenter.Opacity = 0;

            // Set up events
            this.CompletedWithEnd(toAnimation,zIndexController,()=>
            {
                newPresenter.Opacity = 1;
                newPresenter.BeginAnimation(UIElement.OpacityProperty, null);
                oldPresenter.Opacity = 1;
                oldPresenter.Visibility = Visibility.Hidden;
            });

            this.CompletedWithEnd(fromAnimation,zIndexController,()=>
            {
                oldPresenter.Visibility = Visibility.Hidden;
                oldPresenter.Opacity = 1;
                oldPresenter.BeginAnimation(UIElement.OpacityProperty, null);
                newPresenter.BeginAnimation(UIElement.OpacityProperty, toAnimation);
            });

            // Animate
            oldPresenter.BeginAnimation(UIElement.OpacityProperty, fromAnimation);
            zIndexController.SetZIndexOrderBy(newPresenter, oldPresenter);
        }
    }
}