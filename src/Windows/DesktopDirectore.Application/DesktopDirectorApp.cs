using DesktopDirector.App.Models;

namespace DesktopDirector.App
{
    public class DesktopDirectorApp
    {
        public DesktopDirectorApp()
        {
            Initialise();
        }

        public IEnumerable<Component> Components { get; private set; }

        private void Initialise()
        {
            // get list of components
            Components = new List<Component>
            {
                new Component{Name="button0", Type="button"},
                new Component{Name="button1", Type="button"},
                new Component{Name="button2", Type="button"},
                new Component{Name="button3", Type="button"},
                new Component{Name="button4", Type="button"},
                new Component{Name="button5", Type="button"},
                new Component{Name="pot0", Type="button"},
                new Component{Name="pot1", Type="button"},
                new Component{Name="pot2", Type="button"},
                new Component{Name="led0", Type="button"},
                new Component{Name="led1", Type="button"},
                new Component{Name="led2", Type="button"},
                new Component{Name="led3", Type="button"},
                new Component{Name="led4", Type="button"},
                new Component{Name="led5", Type="button"},
            };
        }
    }
}