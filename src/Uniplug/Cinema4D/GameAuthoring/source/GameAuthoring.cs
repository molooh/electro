using System;
using System.Diagnostics;
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
        //Plugin stuff.
        private int PL_PLUGINID = 1035056;

        private FuseeAuthoringToolsC4D fat;
        private CmdUiDialog cmdUi;

        public FuseeProjectLoader() : base(false) { }

        public override bool Execute(BaseDocument doc)
        {
            cmdUi = new CmdUiDialog();
            cmdUi.InitValues();

            cmdUi.CreateLayout();
            
            cmdUi.AddStaticText(3003, C4dApi.BFH_CENTER, 200, 30, "Textfeld", C4dApi.BORDER_NONE);
            
            cmdUi.Open(DLG_TYPE.DLG_TYPE_MODAL_RESIZEABLE, PL_PLUGINID, 200, 200, 350, 200);

            fat = new FuseeAuthoringToolsC4D();

            String slnName = "Engine";
            String projectName = "TestProjekt";
            String fuseeBinProjPath = "C:/Users/dominik/Development/TestFusee";

            /*
            if (fat.CreateProject(slnName, projectName, fuseeBinProjPath))
            {
                Logger.Debug("Project opened or created.");
                Logger.Debug("A project is ready: " + fat.GetEngineProject().NameofCsProject);

                fat.CreateNewClass("TestClassDominik");
                Logger.Debug("Class created!");
                 
            }
            else
            {
                Logger.Debug("ERROR creating new project!");
            }
            */
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
    class FuseeGameAuthoring : TagDataM
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

        public override bool MessageDocumentInfo(GeListNode node, DocumentInfoData data)
        {
            Logger.Debug("In Message" + data.filename);
            return true;
        }

        /*
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

                //DocumentInfoData d = (DocumentInfoData)data;
                
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
        */

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

    /// <summary>
    /// Handles the GUI functions for the command plugin.
    /// </summary>
    public class CmdUiDialog : GeDialog
    {
        private int EDITTEXT = 3001;
        private int STATICTEXT = 3002;

        public override bool InitValues()
        {
            Logger.Debug("From UI Init.");

            return true;
        }

        public override bool CreateLayout()
        {
            Logger.Debug("From create Layout.");

            SetTitle("My Dialog");

            bool b1 = GroupBegin(100010, C4dApi.BFH_CENTER, 1, 0, "Title of Group", 0);
            bool b2 = GroupSpace(4, 4);
            bool b3 = GroupBorderSpace(4, 4, 4, 4);

            AddStaticText(STATICTEXT, C4dApi.BFH_CENTER, 0, 0, "Text:", C4dApi.BORDER_NONE);

            GroupEnd();

            if (AddDlgGroup(C4dApi.DLG_OK | C4dApi.DLG_CANCEL) == false)
            {
                Logger.Debug("Error in AddDlgGroup. Buttons could not be added.");
                return false;
            }

            return true;
        }

        public override int Message(BaseContainer msg, BaseContainer result)
        {
            Logger.Debug("From ui Message.");
            // TODO: return true if taken care of message, return false if otherwise.
            int msgid = msg.GetId();

            return 0;
        }
    }
}