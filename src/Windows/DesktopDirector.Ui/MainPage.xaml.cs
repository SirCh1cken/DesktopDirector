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

            Components = new ObservableCollection<Component>(app.Components);
            components.ItemsSource = Components;
        }

        public ObservableCollection<Component> Components { get; set; }


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