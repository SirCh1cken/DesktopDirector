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
        }


        public ObservableCollection<ComponentConfiguration> Components { get; set; }
        public ObservableCollection<PluginDescriptor> AvailablePluginDescriptors { get; set; }
        public bool IsListening { get { return app.IsListening; } }


        private void OnComponentsItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var selectedComponent = components.SelectedItem as ComponentConfiguration;

            if (selectedComponent != null)
            {
                selectedComponentName.Text = selectedComponent.Component.Name;
                selectedComponentAddress.Text = selectedComponent.Component.Address;
                selectedComponentComponentType.Text = selectedComponent.Component.ComponentType;

                selectedComponentConfiguredPlugins.ItemsSource = new ObservableCollection<PluginConfiguration>(selectedComponent.Plugins);
            }
        }
        private void OnAddPluginClicked(object sender, EventArgs e)
        {
            var selectedComponent = components.SelectedItem as ComponentConfiguration;
            var selectedPlugin = availablePluginDescriptors.SelectedItem as PluginDescriptor;

            if (selectedComponent != null && selectedPlugin != null)
            {
                var pluginConfiguration = new PluginConfiguration
                {
                    Plugin = selectedPlugin
                };
                selectedComponent.Plugins.Add(pluginConfiguration);

                selectedComponentConfiguredPlugins.ItemsSource = new ObservableCollection<PluginConfiguration>(selectedComponent.Plugins);

                //var component = app.Components.First(component => component.Component.Name == selectedComponent.Component.Name);
                //component.Plugins.Add(pluginConfiguration);
            }
        }

        private void OnStartListeningClicked(object sender, EventArgs e)
        {
            this.app.StartListening();
            this.startListeningButton.IsEnabled = false;
            this.stopListeningButton.IsEnabled = true;

        }

        private void OnStopListeningClicked(object sender, EventArgs e)
        {
            this.app.StopListening();
            this.startListeningButton.IsEnabled = true;
            this.stopListeningButton.IsEnabled = false;
        }

        private void OnsavePluginsButtonClicked(object sender, EventArgs e)
        {
            this.app.UpdatePluginInstanceMap();
        }
    }
}