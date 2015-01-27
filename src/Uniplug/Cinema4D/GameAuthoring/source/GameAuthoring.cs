using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C4d;
using GameAuthoringTools;

using Fusee.Math;


namespace GameAuthoring
{
    // Plugin ID is final from maxon.
    [ TagPlugin(1034424,
        Name = "Fusee Asset Tag",
        IconFile = "icon.tif",
        Info = C4d.TagInfoFlag.TAG_VISIBLE,
        Description = "tagplugin",
        Disklevel = 0) ]

           
    class FuseeGameAuthoring : TagData
    {
        FuseeAuthoringTools fat;

        public FuseeGameAuthoring() : base() {
            fat = new FuseeAuthoringTools();
            string s = fat.RetrieveInformation();
            Logger.Debug(s);
        }

        public override bool Init(GeListNode node)
        {
            Logger.Debug("From Init.");

            // Call some info.
            BaseTag tag = (BaseTag)node;
            BaseObject bo = tag.GetObject();
            string name = bo.GetName();

            Logger.Debug("My name is: " + name);

            return true;
        }

        public override EXECUTIONRESULT Execute(BaseTag tag, BaseDocument doc, BaseObject op, BaseThread bt, int priority, EXECUTIONFLAGS flags)
        {
            Logger.Debug("From Execute()");
            return EXECUTIONRESULT.EXECUTIONRESULT_OK;
        }

        public override bool AddToExecution(BaseTag tag, PriorityList list) {
            Logger.Debug("From AddToExecution()");
            return false;
        }

        public override bool Draw(BaseTag tag, BaseObject op, BaseDraw bd, BaseDrawHelp bh) {
            Logger.Debug("From Draw()");
            return true;
        }

        public override bool GetModifiedObjects(BaseTag tag, BaseDocument doc, SWIGTYPE_p_p_BaseObject op, SWIGTYPE_p_Bool pluginownedop, ref double4x4 op_mg, double lod, int flags, BaseThread thread) {
            Logger.Debug("From GetModifiedObjects()");
            return true;
        }

    }
}
