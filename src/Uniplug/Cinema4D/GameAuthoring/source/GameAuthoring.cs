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
    [TagPlugin(
        1034424,
        Name = "GameAuthoringPlugin",
        IconFile = "YourLogoPattern.tif"
        )]

           
    class FuseeGameAuthoring : TagData
    {
        FuseeAuthoringTools fat;

        public FuseeGameAuthoring() : base() {
        }

        public override bool Init(GeListNode node)
        {
            BaseTag tag = (BaseTag)node;

            Logger.Debug("From Init.");

            return true;
        }

        public override EXECUTIONRESULT Execute(BaseTag tag, BaseDocument doc, BaseObject op, BaseThread bt, int priority, EXECUTIONFLAGS flags)
        {
            Logger.Debug("This is an execution test!");
            return EXECUTIONRESULT.EXECUTIONRESULT_OK;
        }

        public override bool AddToExecution(BaseTag tag, PriorityList list) {
            Logger.Debug("This is an add to execution test!");
            return false;
        }

        public override bool Draw(BaseTag tag, BaseObject op, BaseDraw bd, BaseDrawHelp bh) {
            Logger.Debug("This is a draw test!");
            return true;
        }

        public override bool GetModifiedObjects(BaseTag tag, BaseDocument doc, SWIGTYPE_p_p_BaseObject op, SWIGTYPE_p_Bool pluginownedop, ref double4x4 op_mg, double lod, int flags, BaseThread thread) {
            Logger.Debug("This is a modified object test!");
            return true;
        }

    }
}
