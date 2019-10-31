using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System.Windows.Controls
{
    public class RollWipe:CustomWipe
    {
        public override void Wipe(FrameworkElement oldPresenter, FrameworkElement newPresenter, Point origin, ITransitionContainer zIndexController)
        {
            if (oldPresenter == null) throw new ArgumentNullException(nameof(oldPresenter));
            if (newPresenter == null) throw new ArgumentNullException(nameof(newPresenter));
            if (zIndexController == null) throw new ArgumentNullException(nameof(zIndexController));

            zIndexController.SetZIndexOrderBy(oldPresenter, newPresenter);

            if (FromStoryboard != null)
                BeginStoryboard(FromStoryboard, FromStyle, oldPresenter, zIndexController, () =>
                {
                    oldPresenter.Visibility = Visibility.Hidden;
                    zIndexController.SetZIndexOrderBy(newPresenter, oldPresenter);
                });
            if (ToStoryboard != null)
                BeginStoryboard(ToStoryboard, ToStyle, newPresenter, zIndexController, () =>
                {
                    oldPresenter.Visibility = Visibility.Hidden;
                    zIndexController.SetZIndexOrderBy(newPresenter, oldPresenter);
                });
        }
    }
}
