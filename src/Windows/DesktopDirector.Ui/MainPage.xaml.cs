using DesktopDirector.App.Model;
using DesktopDirector.ArduinoInterface.Model;
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

            Plugins = new ObservableCollection<string>(app.Plugins.Select(plugin => plugin.Name));
            plugins.ItemsSource = Plugins; 
        }

        public ObservableCollection<ComponentConfiguration> Components { get; set; }
        public ObservableCollection<string> Plugins { get; set; }


        private void OnCounterClicked(object sender, EventArgs e)
        {
            //count++;

            //if (count == 1)
            //    CounterBtn.Text = $"Clicked {count} time";
            //else
            //    CounterBtn.Text = $"Clicked {count} times";

            //SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}