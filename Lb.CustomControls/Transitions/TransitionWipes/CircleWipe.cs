using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace System.Windows.Controls
{
    public class CircleWipe : BaseWipe
    {
        public override void Wipe(FrameworkElement oldPresenter, FrameworkElement newPresenter, Point origin, ITransitionContainer zIndexController)
        {
            if (oldPresenter == null) throw new ArgumentNullException(nameof(oldPresenter));
            if (newPresenter == null) throw new ArgumentNullException(nameof(newPresenter));
            if (zIndexController == null) throw new ArgumentNullException(nameof(zIndexController));

            var horizontalProportion = Math.Max(1.0 - origin.X, 1.0 * origin.X);
            var verticalProportion = Math.Max(1.0 - origin.Y, 1.0 * origin.Y);
            var radius = Math.Sqrt(Math.Pow(newPresenter.ActualWidth * horizontalProportion, 2) + Math.Pow(newPresenter.ActualHeight * verticalProportion, 2));

            var scaleTransform = new ScaleTransform(0, 0);
            var translateTransform = new TranslateTransform(newPresenter.ActualWidth * origin.X, newPresenter.ActualHeight * origin.Y);
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(translateTransform);
            var ellipseGeomotry = new EllipseGeometry()
            {
                RadiusX = radius,
                RadiusY = radius,
                Transform = transformGroup
            };

            newPresenter.SetCurrentValue(UIElement.ClipProperty, ellipseGeomotry);
            zIndexController.SetZIndexOrderBy(newPresenter, oldPresenter);

            var zeroKeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
            var midKeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200));
            var endKeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(400));

            var opacityAnimation = new DoubleAnimationUsingKeyFrames();
            opacityAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, zeroKeyTime));
            opacityAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, midKeyTime));
            opacityAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, endKeyTime));

            var scaleAnimation = new DoubleAnimationUsingKeyFrames();
            scaleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, zeroKeyTime));
            scaleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, endKeyTime));

            this.CompletedWithEnd(opacityAnimation, zIndexController, () =>
              {
                  oldPresenter.BeginAnimation(UIElement.OpacityProperty, null);
                  oldPresenter.Opacity = 1;
                  oldPresenter.Visibility = Visibility.Hidden;
              });

            this.CompletedWithEnd(scaleAnimation, zIndexController, () =>
              {
                  newPresenter.SetCurrentValue(UIElement.ClipProperty, null);
                  oldPresenter.BeginAnimation(UIElement.OpacityProperty, null);
                  oldPresenter.RenderTransform = null;
                  newPresenter.RenderTransform = null;
                  oldPresenter.Opacity = 1;
                  oldPresenter.Visibility = Visibility.Hidden;
              });

            oldPresenter.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }
    }
}