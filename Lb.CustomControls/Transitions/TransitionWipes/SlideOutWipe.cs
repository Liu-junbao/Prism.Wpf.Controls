using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace System.Windows.Controls
{
    public class SlideOutWipe :TransitionWipeBase
    {
        private readonly SineEase _sineEase = new SineEase();

        public override void Wipe(FrameworkElement oldPresenter, FrameworkElement newPresenter, Point origin, ITransitionContainer zIndexController)
        {
            if (oldPresenter == null) throw new ArgumentNullException(nameof(oldPresenter));
            if (newPresenter == null) throw new ArgumentNullException(nameof(newPresenter));
            if (zIndexController == null) throw new ArgumentNullException(nameof(zIndexController));

            var zeroKeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
            var midishKeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200));
            var endKeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(400));

            //back out old slide setup
            var scaleTransform = new ScaleTransform(1, 1);
            oldPresenter.RenderTransform = scaleTransform;
            var scaleAnimation = new DoubleAnimationUsingKeyFrames();
            scaleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, zeroKeyTime));
            scaleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(.8, endKeyTime));

            var opacityAnimation = new DoubleAnimationUsingKeyFrames();
            opacityAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, zeroKeyTime));
            opacityAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, endKeyTime));


            //slide in new slide setup
            var translateTransform = new TranslateTransform(0, newPresenter.ActualHeight);
            newPresenter.RenderTransform = translateTransform;
            var slideAnimation = new DoubleAnimationUsingKeyFrames();
            slideAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(newPresenter.ActualHeight, zeroKeyTime));
            slideAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(newPresenter.ActualHeight, midishKeyTime) { EasingFunction = _sineEase });
            slideAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, endKeyTime) { EasingFunction = _sineEase });

            this.CompletedWithEnd(scaleAnimation, zIndexController, () =>
            {
                oldPresenter.RenderTransform = null;
                oldPresenter.Visibility = Visibility.Hidden;
            });

            this.CompletedWithEnd(opacityAnimation, zIndexController, () =>
            {
                oldPresenter.BeginAnimation(UIElement.OpacityProperty, null);
                oldPresenter.Opacity = 1;
                oldPresenter.Visibility = Visibility.Hidden;
            });

            this.CompletedWithEnd(slideAnimation, zIndexController, () =>
            {
                newPresenter.RenderTransform = null;
            });

            //kick off!
            translateTransform.BeginAnimation(TranslateTransform.YProperty, slideAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
            oldPresenter.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);

            zIndexController.SetZIndexOrderBy(newPresenter, oldPresenter);
        }
    }
}