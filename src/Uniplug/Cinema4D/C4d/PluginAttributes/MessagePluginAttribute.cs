using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C4d
{
    [AttributeUsage(System.AttributeTargets.Class)]
    public class MessagePluginAttribute : PluginBaseAttribute
    {
        public int Info;
        public MessageData Data;

        public MessagePluginAttribute() : base()
        {

        }

        public MessagePluginAttribute(int id) : base(id)
        {

        }

        public MessagePluginAttribute(int id, string name) : base(id, name)
        {

        }

        public MessagePluginAttribute(int id, string name, int info) : base(id, name)
        {
            Info = 1 << 29;
        }

        public MessagePluginAttribute(int id, string name, int info, MessageData data) : base(id, name)
        {
            Info = 1 << 29;
            Data = data;
        }
    }
}
