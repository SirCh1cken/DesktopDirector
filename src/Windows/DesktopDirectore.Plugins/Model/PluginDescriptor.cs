namespace DesktopDirector.Plugins.Model
{
    public class PluginDescriptor
    {
        private readonly string name;
        private readonly Type pluginType;

        public PluginDescriptor(string name, Type pluginType)
        {
            this.name = name;
            this.pluginType = pluginType;
        }

        public string Name { get { return name; } }
        public Type PluginType { get { return pluginType; } }
    }
}
