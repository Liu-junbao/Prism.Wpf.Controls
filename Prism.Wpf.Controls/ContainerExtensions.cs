using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Wpf
{
    public static class ContainerExtensions
    {
        public static TInstance GetInstance<TInstance>()
        {
            if (CommonServiceLocator.ServiceLocator.IsLocationProviderSet)
            {
                return CommonServiceLocator.ServiceLocator.Current.GetInstance<TInstance>();
            }
            return default(TInstance);
        }

        public static void NavigateToView<TView>(this object obj, string regionName)
        {
            GetInstance<IRegionManager>()?.RequestNavigate(regionName, typeof(TView).FullName.Replace(".", "/"), OnNavigated);
        }
        public static void RegisterView<TView>(this IContainerProvider containerProvider,string regionName)
        {
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion(regionName,typeof(TView));
        }
        public static void RegisterView<TView>(this IContainerRegistry containerRegistry, string regionName)
        {
            containerRegistry.RegisterForNavigation<TView>(typeof(TView).FullName.Replace(".", "/"));
        }
        private static void OnNavigated(NavigationResult obj)
        {
            GetInstance<IEventAggregator>()?.GetEvent<PubSubEvent<NavigationResult>>().Publish(obj);
        }
        public static void PublichNavigated(this NavigationResult result)
        {
            GetInstance<IEventAggregator>()?.GetEvent<PubSubEvent<NavigationResult>>().Publish(result);
        }
        public static void SubscribeNavigated(this object obj, Action<NavigationResult> onResult, ThreadOption threadOption = ThreadOption.PublisherThread)
        {
            GetInstance<IEventAggregator>()?.GetEvent<PubSubEvent<NavigationResult>>().Subscribe(onResult, threadOption);
        }
    }
}
