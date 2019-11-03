﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lb.CustomControls
{
    public interface ITransitionWipeSelector
    {
        ITransitionWipe ProviderTransitionWipeFrom(FrameworkElement oldPresenter,FrameworkElement newPresenter,ITransitionContainer container);
    }
}
