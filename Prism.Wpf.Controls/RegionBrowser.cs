using Prism.Regions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Prism.Wpf
{
    [TemplatePart(Name =PART_Header,Type =typeof(ListBox))]
    [StyleTypedProperty(Property = nameof(HeaderContainerStyle), StyleTargetType = typeof(ListBoxItem))]
    [StyleTypedProperty(Property = nameof(DropDownHeaderContainerStyle), StyleTargetType = typeof(ComboBoxItem))]
    public class RegionBrowser : ContentControl
    {
        public const string PART_Header = nameof(PART_Header);
        public static RoutedCommand CloseViewCommand = new RoutedCommand();
        private static readonly DependencyPropertyKey RegionPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Region), typeof(IRegion), typeof(RegionBrowser), new PropertyMetadata(null));
        public static readonly DependencyProperty RegionProperty = RegionPropertyKey.DependencyProperty;
        private static readonly DependencyPropertyKey ViewsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Views), typeof(IEnumerable), typeof(RegionBrowser), new PropertyMetadata(null));
        public static readonly DependencyProperty ViewsProperty = ViewsPropertyKey.DependencyProperty;
        public static readonly DependencyProperty HeaderContainerStyleProperty =
            DependencyProperty.Register(nameof(HeaderContainerStyle), typeof(Style), typeof(RegionBrowser), new PropertyMetadata(null));
        public static readonly DependencyProperty DropDownHeaderContainerStyleProperty =
            DependencyProperty.Register(nameof(DropDownHeaderContainerStyle), typeof(Style), typeof(RegionBrowser), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(RegionBrowser), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentControlTemplateProperty =
            DependencyProperty.Register(nameof(ContentControlTemplate), typeof(ControlTemplate), typeof(RegionBrowser), new PropertyMetadata(null));
        public static readonly DependencyProperty MenuContextProperty =
            DependencyProperty.Register(nameof(MenuContext), typeof(object), typeof(RegionBrowser), new PropertyMetadata(null));
        public static readonly DependencyProperty ToolContextProperty =
            DependencyProperty.Register(nameof(ToolContext), typeof(object), typeof(RegionBrowser), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.Register(nameof(ContentBackground), typeof(Brush), typeof(RegionBrowser), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderMouseOverBackgroundProperty =
            DependencyProperty.Register(nameof(MouseOverBackground), typeof(Brush), typeof(RegionBrowser), new PropertyMetadata(null));

        static RegionBrowser()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RegionBrowser), new FrameworkPropertyMetadata(typeof(RegionBrowser)));
        }
        private ListBox _headerBox;
        public RegionBrowser()
        {
            this.CommandBindings.Add(new CommandBinding(CloseViewCommand, OnCloseViewHandler));
        }
        public IRegion Region
        {
            get { return (IRegion)GetValue(RegionProperty); }
            protected set { SetValue(RegionPropertyKey, value); }
        }
        [Bindable(true)]
        [Category("Appearance")]
        public Brush ContentBackground
        {
            get { return (Brush)GetValue(ContentBackgroundProperty); }
            set { SetValue(ContentBackgroundProperty, value); }
        }
        [Bindable(true)]
        [Category("Appearance")]
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(HeaderMouseOverBackgroundProperty); }
            set { SetValue(HeaderMouseOverBackgroundProperty, value); }
        }
        public object MenuContext
        {
            get { return (object)GetValue(MenuContextProperty); }
            set { SetValue(MenuContextProperty, value); }
        }
        public object ToolContext
        {
            get { return (object)GetValue(ToolContextProperty); }
            set { SetValue(ToolContextProperty, value); }
        }
        public Style HeaderContainerStyle
        {
            get { return (Style)GetValue(HeaderContainerStyleProperty); }
            set { SetValue(HeaderContainerStyleProperty, value); }
        }
        public Style DropDownHeaderContainerStyle
        {
            get { return (Style)GetValue(DropDownHeaderContainerStyleProperty); }
            set { SetValue(DropDownHeaderContainerStyleProperty, value); }
        }
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }
        public ControlTemplate ContentControlTemplate
        {
            get { return (ControlTemplate)GetValue(ContentControlTemplateProperty); }
            set { SetValue(ContentControlTemplateProperty, value); }
        }
        public IEnumerable Views
        {
            get { return (IEnumerable)GetValue(ViewsProperty); }
            protected set { SetValue(ViewsPropertyKey, value); }
        }
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            _headerBox?.ScrollIntoView(newContent);
            base.OnContentChanged(oldContent, newContent);
        }
        private void OnCloseViewHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var region = Region;
            if (region?.Views?.Contains(e.Parameter) == true)
            {
                region.Remove(e.Parameter);
                if (region.ActiveViews.Count() == 0)
                {
                    var view = region.Views.FirstOrDefault();
                    if (view != null)
                    {
                        region.Activate(view);
                    }
                }
            }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _headerBox = this.Template.FindName(PART_Header, this) as ListBox;
            ApplyRegion();
        }
        private void ApplyRegion()
        {
            var regionManager = RegionManager.GetRegionManager(this);
            if (regionManager == null && CommonServiceLocator.ServiceLocator.IsLocationProviderSet)
                regionManager = CommonServiceLocator.ServiceLocator.Current.GetInstance<IRegionManager>();
            if (regionManager != null)
            {
                var regionName = RegionManager.GetRegionName(this);
                if (string.IsNullOrEmpty(regionName) == false)
                {
                    if (regionManager.Regions.ContainsRegionWithName(regionName))
                    {
                        InitializeRegion(regionManager.Regions[regionName]);
                    }
                    else
                    {
                        regionManager.Regions.CollectionChanged += (s, e) =>
                        {
                            if (e.NewItems != null)
                            {
                                foreach (IRegion region in e.NewItems)
                                {
                                    if (region.Name == regionName)
                                    {
                                        this.Dispatcher.BeginInvoke(new Action(() => InitializeRegion(region)));
                                        InitializeRegion(region);
                                        break;
                                    }
                                }
                            }
                        };
                    }
                }
            }
        }      
        private void InitializeRegion(IRegion region)
        {
            if (Region == null && region != null)
            {
                Region = region;
                Views = region.Views;
            }
        }
    }
}
