using DesktopDirector.App.Model;
using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.Plugins.Model;
using System.Collections.ObjectModel;

namespace DesktopDirector.Ui
{
    public partial class MainPage : ContentPage
    {
        private readonly DesktopDirector.App.DesktopDirectorApp app;
        int count = 0;

        public MainPage()
        {
            this.app = new DesktopDirector.App.DesktopDirectorApp();

            InitializeComponent();

            Components = new ObservableCollection<ComponentConfiguration>(app.Components);
            components.ItemsSource = Components;

            AvailablePluginDescriptors = new ObservableCollection<PluginDescriptor>(app.PluginDescriptors);
            availablePluginDescriptors.ItemsSource = AvailablePluginDescriptors;

            //selectedComponentPlugins.ItemsSource = AvailablePlugins;
        }


        public ObservableCollection<ComponentConfiguration> Components { get; set; }
        public ObservableCollection<PluginDescriptor> AvailablePluginDescriptors { get; set; }
        //public ComponentConfiguration SelectedComponentConfiguration { get; set; }
        //public Plugin SelectedPlugin { get; set; }

        private void OnComponentsItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //selectedPluginConfiguration.Text = string.Empty;

            var selectedComponent = components.SelectedItem as ComponentConfiguration;

            if (selectedComponent != null)
            {
                selectedComponentName.Text = selectedComponent.Component.Name;
                selectedComponentAddress.Text = selectedComponent.Component.Address;
                selectedComponentComponentType.Text = selectedComponent.Component.ComponentType;

                selectedComponentConfiguredPlugins.ItemsSource = new ObservableCollection<PluginConfiguration>(selectedComponent.Plugins);
                //    selectedComponentName.Text = selectedComponent.Component.Name;
                //    selectedComponentAddress.Text = selectedComponent.Component.Address;
                //    selectedComponentType.Text = selectedComponent.Component.ComponentType;

                //    if(selectedComponent.Plugins != null && selectedComponent.Plugins.Any())
                //    {
                //        var plugin = selectedComponent.Plugins.First();

                //        selectedComponentPlugins.SelectedItem = plugin.Plugin.Name;
                //        selectedPluginConfiguration.Text = plugin.Configuration;
                //    }
                //    else
                //    {
                //        selectedComponentPlugins.SelectedItem = null;
                //        selectedPluginConfiguration.Text= string.Empty;
                //    }
            }
        }
        private void OnAddPluginClicked(object sender, EventArgs e)
        {
            var selectedComponent = components.SelectedItem as ComponentConfiguration;
            var selectedPlugin = availablePluginDescriptors.SelectedItem as PluginDescriptor;

            if (selectedComponent != null && selectedPlugin != null)
            {
                selectedComponent.Plugins.Add(new PluginConfiguration
                {
                    Plugin = selectedPlugin
                });

                selectedComponentConfiguredPlugins.ItemsSource = new ObservableCollection<PluginConfiguration>(selectedComponent.Plugins);

            }
        }



        private void OnSelectedComponentPluginsSelectedItemChanged(object sender, EventArgs e)
        {
            //if (SelectedComponentConfiguration != null)
            //{
            //    var plugin = app.Plugins.First(plugin => plugin.Name == (string)selectedComponentPlugins.SelectedItem);
            //    SelectedPlugin = plugin;
            //}
            //else
            //{

            //}

            //var selectedComponent = components.SelectedItem as ComponentConfiguration;

            //if(selectedComponentPlugins.SelectedItem!= null) 
            //{
            //    var plugin = app.Plugins.First(plugin => plugin.Name == (string)selectedComponentPlugins.SelectedItem);
            //    selectedComponent.Plugins = new PluginConfiguration[] { new PluginConfiguration { Plugin = plugin } };
            //}
            //else
            //{
            //    selectedComponent.Plugins = null;
            //}
        }



        private void OnAdditionalPluginSelected(object sender, EventArgs e)
        {
            if (components.SelectedItem != null)
            {
                var selectedComponent = components.SelectedItem as ComponentConfiguration;
                var selectedPlugin = availablePluginDescriptors.SelectedItem as PluginDescriptor;

                if (selectedPlugin != null)
                {
                    selectedComponent.Plugins.Add(new PluginConfiguration
                    {
                        Plugin = selectedPlugin
                    });

                    availablePluginDescriptors.SelectedItem = null;
                }
            }
        }
    }
}