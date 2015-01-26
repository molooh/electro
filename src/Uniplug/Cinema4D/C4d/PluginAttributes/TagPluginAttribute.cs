using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C4d
{
    [AttributeUsage(System.AttributeTargets.Class)]
    public class TagPluginAttribute : PluginBaseAttribute
    {
        public string HelpText;

        public int Info
        {
            get
            {
                return 0; // TODO: seems wrong. Look documentation for plugin flags.
            }
        }

        public TagPluginAttribute(int id) : base(id)
        {
        }
        public TagPluginAttribute(int id, string name) : base(id, name)
        {
        }
        public TagPluginAttribute(int id, string name, string iconFile)
            :base(id, name, iconFile)
        {
        }
        public TagPluginAttribute(int id, string name, string iconFile, string helpText)
            : base(id, name, iconFile)
        {
             HelpText = helpText;
        }   
    }
}
