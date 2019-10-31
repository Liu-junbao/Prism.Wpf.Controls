using ModuleC.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Wpf;

namespace ModuleC
{
    [Module(ModuleName = nameof(ModuleC))]
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterView<ViewA>("ContentRegion");
        }
    }
}