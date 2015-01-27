using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C4d
{
    public enum TagInfoFlag
    {
        TAG_VISIBLE
    }

    [AttributeUsage(System.AttributeTargets.Class)]
    public class TagPluginAttribute : PluginBaseAttribute
    {
        /*
         * Parameters from C4D SDK for a tag plugin:
            Int32 	id,
            const String & 	str,
            Int32 	info,
            DataAllocator * 	g,
            const String & 	description,
            BaseBitmap * 	icon,
            Int32 	disklevel 
        */

        public TagInfoFlag Info;
        public string Description;
        public int Disklevel;

        public TagPluginAttribute(int id) : base (id)
        {
            Name = "Plugin";
            IconFile = "icon.tif";
            Info = TagInfoFlag.TAG_VISIBLE;
            Description = "tagplugin";
            Disklevel = 0;
        }

        public TagPluginAttribute(int id, string name) : base(id, name)
        {
            IconFile = "icon.tif";
            Info = TagInfoFlag.TAG_VISIBLE;
            Description = "tagplugin";
            Disklevel = 0;
        }

        public TagPluginAttribute(int id, string name, string iconFile) : base(id, name, iconFile)
        {
            Info = TagInfoFlag.TAG_VISIBLE;
            Description = "tagplugin";
            Disklevel = 0;
        }

        /// <summary>
        /// This is the constructor for the attribute.
        /// </summary>
        /// <param name="id">Plugin ID</param>
        /// <param name="name">Plugin Name</param>
        /// <param name="iconFile">Plugin icon file</param>
        /// <param name="info">node plugin info flags</param>
        /// <param name="description" name of the description resource file. Default is obase></param>
        /// <param name="disklevel">plugin level is similar to a version number. The default level is 0</param>
        public TagPluginAttribute(int id, string name, string iconFile, TagInfoFlag info, string description = "obase", int disklevel = 0) : base(id, name, iconFile)
        {
            Info = info;
            Description = description;
            Disklevel = disklevel;
        }   
    }
}
