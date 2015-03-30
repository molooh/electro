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

    // Plugin ID is final.
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

        public override bool Draw(BaseTag tag, BaseObject op, BaseDraw bd, BaseDrawHelp bh) {
            Logger.Debug("From Draw()");
            return true;
        }

        //TODO: This should accept C4dApi.DocumentInfoData typed instance etc. as the third paramter.
        public override bool Message(GeListNode node, int type, SWIGTYPE_p_void data)
        {

            if (type == C4dApi.MSG_EDIT)
            {
                Logger.Debug("MSG_EDIT = " + type);
            }
            else if (type == C4dApi.MSG_GETCUSTOMICON)
            {
                Logger.Debug("MSG_GETCUSTOMICON = " + type);
            }
            else if (type == C4dApi.COLORSYSTEM_HSVTAB)
            {
                Logger.Debug("MSG_COLORSYSTEM_HSVTAB = " + type);
            }
            else if (type == C4dApi.MSG_DOCUMENTINFO)
            {
                Logger.Debug("MSG_DOCUMENTINFO = " + type);

                //DocumentInfoData dInfoData = data;
                // TODO: Here comes an objeft from type DocumentInfoData with void*
                // TODO: Need to parse it somehow and then i can grab all the types from DocumentInfoData()->type
                // TODO: At least it should work like this.

                DocumentInfoData d = (DocumentInfoData)data;
                
                if (type == C4dApi.MSG_DOCUMENTINFO_TYPE_SAVEPROJECT_BEFORE)
                {
                    Logger.Debug("INFODATA: -> MSG_DOCUMENTINFO_TYPE_BEFORE = " + type);
                }
                else if (type == C4dApi.MSG_DOCUMENTINFO_TYPE_SAVEPROJECT_AFTER)
                {
                    Logger.Debug("INFODATA: -> MSG_DOCUMENTINFO_TYPE_AFTER = " + type);
                }
                else if (type == C4dApi.MSG_DOCUMENTINFO_TYPE_TAG_INSERT)
                {
                    Logger.Debug("INFODATA: -> MSG_DOCUMENTINFO_TYPE_TAG_INSERT = " + type);
                }
                else if (type == C4dApi.MSG_DOCUMENTINFO_TYPE_LOAD)
                {
                    Logger.Debug("INFODATA: -> MSG_DOCUMENTINFO_TYPE_TAG_LOAD = " + type);
                }

            }
            else if (type == C4dApi.MSG_DESCRIPTION_GETINLINEOBJECT)
            {
                Logger.Debug("MSG_DESCRIPTION_GETINLINEOBJECT = " + type);
            }
            else if (type == C4dApi.DRAW_PARAMETER_OGL_PRIMITIVERESTARTINDEX)
            {
                Logger.Debug("DRAW_PARAMETER_OGL_PRIMITIVERESTARTINDEX = " + type);
            }
            else
            {
                Logger.Debug("MSG_ID: " + type);
            }
                    
            //Logger.Debug("From Message, node: " + node.GetNodeID());

            string filename = node.GetDocument().GetDocumentName().GetString();
            //Logger.Debug("fname: " + filename);

            return true;
        }

        public override bool GetDDescription(GeListNode node, Description description, SWIGTYPE_p_DESCFLAGS_DESC flags)
        {
            Logger.Debug("From GetDDescription: " + description.GetDescription());
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