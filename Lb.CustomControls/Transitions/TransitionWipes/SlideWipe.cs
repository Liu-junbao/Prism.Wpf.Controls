using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace System.Windows.Controls
{
    public class SlideWipe : TransitionWipeBase
    {        
        private readonly SineEase _sineEase = new SineEase();

        /// <summary>
        /// Direction of the slide wipe
        /// </summary>
        public SlideDirection Direction { get; set; }

        /// <summary>
        /// Duration of the animation
        /// </summary>
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0.7);

        public override void Wipe(FrameworkElement oldPresenter, FrameworkElement newPresenter, Point origin, ITransitionContainer zIndexController)
        {
            if (oldPresenter == null) throw new ArgumentNullException(nameof(oldPresenter));
            if (newPresenter == null) throw new ArgumentNullException(nameof(newPresenter));
            if (zIndexController == null) throw new ArgumentNullException(nameof(zIndexController));

            // Set up time points
            var zeroKeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
            var endKeyTime = KeyTime.FromTimeSpan(Duration);

            // Set up coordinates
            double fromStartX = 0, fromEndX = 0, toStartX = 0, toEndX = 0;
            double fromStartY = 0, fromEndY = 0, toStartY = 0, toEndY = 0;

            if (Direction == SlideDirection.Left)
            {
                fromEndX = -oldPresenter.ActualWidth;
                toStartX = newPresenter.ActualWidth;
            }
            else if (Direction == SlideDirection.Right)
            {
                fromEndX = oldPresenter.ActualWidth;
                toStartX = -newPresenter.ActualWidth;
            }
            else if (Direction == SlideDirection.Up)
            {
                fromEndY = -oldPresenter.ActualHeight;
                toStartY = newPresenter.ActualHeight;
            }
            else if (Direction == SlideDirection.Down)
            {
                fromEndY = oldPresenter.ActualHeight;
                toStartY = -newPresenter.ActualHeight;
            }

            // From
            var fromTransform = new TranslateTransform(fromStartX, fromStartY);
            oldPresenter.RenderTransform = fromTransform;
            var fromXAnimation = new DoubleAnimationUsingKeyFrames();
            fromXAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(fromStartX, zeroKeyTime));
            fromXAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(fromEndX, endKeyTime, _sineEase));
            var fromYAnimation = new DoubleAnimationUsingKeyFrames();
            fromYAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(fromStartY, zeroKeyTime));
            fromYAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(fromEndY, endKeyTime, _sineEase));

            // To
            var toTransform = new TranslateTransform(toStartX, toStartY);
            newPresenter.RenderTransform = toTransform;
            var toXAnimation = new DoubleAnimationUsingKeyFrames();
            toXAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(toStartX, zeroKeyTime));
            toXAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(toEndX, endKeyTime, _sineEase));
            var toYAnimation = new DoubleAnimationUsingKeyFrames();
            toYAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(toStartY, zeroKeyTime));
            toYAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(toEndY, endKeyTime, _sineEase));

            this.CompletedWithEnd(fromXAnimation,zIndexController,()=> {
                fromTransform.BeginAnimation(TranslateTransform.XProperty, null);
                fromTransform.X = fromEndX;
                oldPresenter.RenderTransform = null;
                oldPresenter.Visibility = Visibility.Hidden;
            });
            this.CompletedWithEnd(fromYAnimation,zIndexController,()=> {
                fromTransform.BeginAnimation(TranslateTransform.YProperty, null);
                fromTransform.Y = fromEndY;
                oldPresenter.RenderTransform = null;
                oldPresenter.Visibility = Visibility.Hidden;
            });
            this.CompletedWithEnd(toXAnimation,zIndexController,()=> {
                toTransform.BeginAnimation(TranslateTransform.XProperty, null);
                toTransform.X = toEndX;
                newPresenter.RenderTransform = null;
                oldPresenter.Visibility = Visibility.Collapsed;
            });
            this.CompletedWithEnd(toYAnimation,zIndexController,()=> {
                toTransform.BeginAnimation(TranslateTransform.YProperty, null);
                toTransform.Y = toEndY;
                newPresenter.RenderTransform = null;
                oldPresenter.Visibility = Visibility.Hidden;
            });

            // Animate
            fromTransform.BeginAnimation(TranslateTransform.XProperty, fromXAnimation);
            fromTransform.BeginAnimation(TranslateTransform.YProperty, fromYAnimation);
            toTransform.BeginAnimation(TranslateTransform.XProperty, toXAnimation);
            toTransform.BeginAnimation(TranslateTransform.YProperty, toYAnimation);

            zIndexController.SetZIndexOrderBy(newPresenter, oldPresenter);
        }
    }
}