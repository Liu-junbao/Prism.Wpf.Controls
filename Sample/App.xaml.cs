using Prism.Ioc;
using Prism.Modularity;
using Sample.Views;
using System.Windows;

namespace Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = "." };
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
