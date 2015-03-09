using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C4d;

using Fusee.Math;
using FuseeAuthoringTools;
using FuseeAuthoringTools.c4d;


namespace GameAuthoring
{
    // Plugin ID is final.
    [ TagPlugin(1034424,
        Name = "Fusee Asset Tag",
        IconFile = "icon.tif",
        Info = C4d.TagInfoFlag.TAG_VISIBLE,
        Description = "tagplugin",
        Disklevel = 0) ]
           
    class FuseeGameAuthoring : TagData
    {
        // private
        private FuseeAuthoringToolsC4D fat;

        public FuseeGameAuthoring() : base() {
            // Creating a connection to the logic behind.
            fat = new FuseeAuthoringToolsC4D();

            String slnName = "Engine";
            String projectName = "TestProjekt";
            String fuseeBinProjPath = "C:/Users/dominik/Development/TestFusee";

            if (fat.CreateProject(slnName, projectName, fuseeBinProjPath))
            {
                Logger.Debug("Created new Project.");

                Logger.Debug("A project is ready: " + fat.GetEngineProject().nameofCSPROJ);

            }
            else
            {
                Logger.Debug("ERROR creating new project!");
            }
        }

        public override bool Init(GeListNode node)
        {
            Logger.Debug("From Init." + GetObjectName(node));

            return true;
        }

        public override EXECUTIONRESULT Execute(BaseTag tag, BaseDocument doc, BaseObject op, BaseThread bt, int priority, EXECUTIONFLAGS flags)
        {
            tag.GetData().GetBool((int)TGameAuthoring.POWER_SWITCH);

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

        public override bool Message(GeListNode node, int type, SWIGTYPE_p_void data)
        {
            int i = 0;
            return base.Message(node, type, data);
        }

        public override bool GetModifiedObjects(BaseTag tag, BaseDocument doc, SWIGTYPE_p_p_BaseObject op, SWIGTYPE_p_Bool pluginownedop, ref double4x4 op_mg, double lod, int flags, BaseThread thread) {
            Logger.Debug("From GetModifiedObjects()");
            return true;
        }

        public override bool GetDDescription(GeListNode node, Description description, SWIGTYPE_p_DESCFLAGS_DESC flags)
        {
            int i = 1;
            return base.GetDDescription(node, description, flags);
        }


        private String GetObjectName(GeListNode node)
        {
            // Call some info.
            BaseTag tag = (BaseTag)node;
            BaseObject bo = tag.GetObject();

            return bo.GetName();;
        }
    }
}
