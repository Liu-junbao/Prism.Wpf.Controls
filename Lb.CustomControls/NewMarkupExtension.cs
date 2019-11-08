using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace Lb.CustomControls
{
    public class NewMarkupExtension<TValue> : MarkupExtension where TValue : new()
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new TValue();
        }
    }
}
