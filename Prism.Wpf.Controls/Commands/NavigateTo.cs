using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;

namespace Prism.Wpf.Commands
{
    public class NavigateTo : ICommand
    {
        public NavigateTo(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }
        public IRegionManager RegionManager { get; }
        public string ViewName { get; set; }
        public string RegionName { get; set; }
        public bool CanExecute(object parameter) => string.IsNullOrEmpty(ViewName) == false;
        public void Execute(object parameter)
        {
            if (string.IsNullOrEmpty(RegionName))
                this.RegionManager.Regions.FirstOrDefault()?.RequestNavigate(ViewName, i => i.PublichNavigated());
            else
                this.RegionManager.RequestNavigate(RegionName, ViewName, i => i.PublichNavigated());
        }
        public event EventHandler CanExecuteChanged;
    }
    public class NavigateToExtension : InstanceExtenions<NavigateTo>
    {
        public NavigateToExtension(string viewName)
        {
            ViewName = viewName;
        }
        public NavigateToExtension(string regionName,string viewName)
        {
            RegionName = regionName;
            ViewName = viewName;
        }
        [ConstructorArgument("viewName")]
        public string ViewName { get; set; }
        [ConstructorArgument("regionName")]
        public string RegionName { get; set; }
        protected override void OnInitialize(NavigateTo value)
        {
            value.ViewName = ViewName;
            value.RegionName = RegionName;
            base.OnInitialize(value);
        }
    }
}
