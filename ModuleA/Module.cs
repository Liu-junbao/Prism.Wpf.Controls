using System;
using System.Threading.Tasks;
using ModuleA.ViewModels;
using ModuleA.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Wpf;

namespace ModuleA
{
    [Module(ModuleName = nameof(ModuleA))]
    public class Module : IModule
    {
        public  void OnInitialized(IContainerProvider containerProvider)
        {
        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterView<ViewA>("ContentRegion");
        }
    }
}