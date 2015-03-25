using System;
using C4d;
using Fusee.Math;
using FuseeAuthoringTools;
using FuseeAuthoringTools.c4dSet;

namespace GameAuthoring.source
{
    struct PluginReferenceHelper
    {
        private EngineProject _ep;

        public EngineProject EngineProject
        {
            get { return _ep; }
            set { _ep = value; }
        }

    }
    
    /// <summary>
    /// This communicates between the plugins.
    /// </summary>
    class PluginCommunicator
    {
        private PluginReferenceHelper _pluginReferenceHelper;
        private FuseeProjectLoader _fuseeProjectLoader;
        private FuseeGameAuthoring _fuseeGameAuthoring;

        public PluginReferenceHelper PluginReferenceHelper
        {
            get { return _pluginReferenceHelper;}
            set { _pluginReferenceHelper = value; }
        }

        public FuseeProjectLoader FuseeProjectLoader
        {
            get { return _fuseeProjectLoader;}
            set { _fuseeProjectLoader = value; }
        }

        public FuseeGameAuthoring FuseeGameAuthoring
        {
            get { return _fuseeGameAuthoring;}
            set { _fuseeGameAuthoring = value; }
        }
    }

    [CommandPlugin(1035056,
       Name = "Fusee Game Authoring Project Helper",
       HelpText = "Opens a Fusee Project and keeps it in memory.'",
       IconFile = "icon.tif")
    ]
    class FuseeProjectLoader : CommandData
    {
        private FuseeAuthoringToolsC4D fat;

        public FuseeProjectLoader() : base(false) { }

        public override bool Execute(BaseDocument doc)
        {
            fat = new FuseeAuthoringToolsC4D();

            String slnName = "Engine";
            String projectName = "TestProjekt";
            String fuseeBinProjPath = "C:/Users/dominik/Development/TestFusee";

            if (fat.CreateProject(slnName, projectName, fuseeBinProjPath))
            {
                Logger.Debug("Project opened or created.");
                Logger.Debug("A project is ready: " + fat.GetEngineProject().NameofCsProject);

                /*
                fat.CreateNewClass("TestClassDominik");
                Logger.Debug("Class created!");
                 */
            }
            else
            {
                Logger.Debug("ERROR creating new project!");
            }

            return true;
        }
    }

    // Plugin ID is final.
    [ TagPlugin(1034424,
        Name = "Fusee Asset Tag",
        IconFile = "icon.tif",
        Info = TagInfoFlag.TAG_VISIBLE,
        Description = "tagplugin",
        Disklevel = 0) ]
    class FuseeGameAuthoring : TagData
    {
        // private
        private FuseeAuthoringToolsC4D fat;

        public FuseeGameAuthoring() : base(false) { }
        
        public override bool Init(GeListNode node)
        {
            // Creating a connection to the logic behind.
            fat = new FuseeAuthoringToolsC4D();

            String slnName = "Engine";
            String projectName = "TestProjekt";
            String fuseeBinProjPath = "C:/Users/dominik/Development/TestFusee";

            // TODO: Work with tag stuff here.

            return true;
        }

        public override EXECUTIONRESULT Execute(BaseTag tag, BaseDocument doc, BaseObject op, BaseThread bt, int priority, EXECUTIONFLAGS flags)
        {
            //tag.GetData().GetBool((int)TGameAuthoring.POWER_SWITCH);

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
            return true;
        }

        public override bool GetModifiedObjects(BaseTag tag, BaseDocument doc, SWIGTYPE_p_p_BaseObject op, SWIGTYPE_p_Bool pluginownedop, ref double4x4 op_mg, double lod, int flags, BaseThread thread) {
            Logger.Debug("From GetModifiedObjects()");
            return true;
        }

        public override bool GetDDescription(GeListNode node, Description description, SWIGTYPE_p_DESCFLAGS_DESC flags)
        {
            return true;
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
