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
    /// TODO: Might not be needed?
    /// </summary>
    [MessagePlugin(ID = 1035161, Name = "MessageHandler", Info = 1 << 29)]
    class FatPluginCommunicator : MessageData
    {
        public FatPluginCommunicator() : base(false) { }

        public override bool CoreMessage(int id, BaseContainer bc)
        {
            Logger.Debug("MessageHandler ID: " + id);
            return false;
        }

        public override int GetTimer()
        {
            return 100;
        }
    }

    // Plugin ID is final.
    [CommandPlugin(1035056,
       Name = "Fusee Game Authoring Project Helper",
       HelpText = "Opens a Fusee Project and keeps it in memory.'",
       IconFile = "icon.tif")
    ]
    class FatProjectLoaderPlugin : CommandData
    {
        //Plugin stuff.
        private int PL_PLUGINID = 1035056;

        private FuseeAuthoringToolsC4D fat;
        private CmdUiDialog cmdUi;

        public FatProjectLoaderPlugin() : base(false) { }

        public override bool Execute(BaseDocument doc)
        {
            cmdUi = new CmdUiDialog();
            cmdUi.InitValues();

            cmdUi.CreateLayout();
            
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

        public override bool Message(int type, SWIGTYPE_p_void data)
        {
            Logger.Debug("Data type from Command Plugin Message: " + data.GetType());
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
    class FatAssetTagPlugin : TagDataM
    {
        // private
        private FuseeAuthoringToolsC4D fat;

        public FatAssetTagPlugin() : base(false) { }
        
        public override bool Init(GeListNode node)
        {
            // Creating a connection to the logic behind.
            fat = new FuseeAuthoringToolsC4D();

            String slnName = "Engine";
            String projectName = "TestProjekt";
            String fuseeBinProjPath = "C:/Users/dominik/Development/TestFusee";

            // TODO: Work with tag stuff here.
            Logger.Debug("From TagData Init: initialized.");

            // TODO: Send message to my own plugin, so i can get data from the object.
            C4dApi.SpecialEventAdd(1034424, 230187);

            return true;
        }

        public override bool Draw(BaseTag tag, BaseObject op, BaseDraw bd, BaseDrawHelp bh) {
            //Logger.Debug("From Draw()");
            return true;
        }

        public override bool MessageDocumentInfo(GeListNode node, DocumentInfoData data)
        {
            // TODO: Check what the message is saying.
            if (data.type == C4dApi.MSG_DOCUMENTINFO_TYPE_SAVE_BEFORE)
            {
                Logger.Debug("MSG_DOCUMENTINFO_TYPE_SAVE_BEFORE");
            }
            else if (data.type == C4dApi.MSG_DOCUMENTINFO_TYPE_SAVE_AFTER)
            {
                Logger.Debug("MSG_DOCUMENTINFO_TYPE_SAVE_AFTER");
                // TODO: Now export .fus
            }
            else if (data.type == C4dApi.MSG_DOCUMENTINFO_TYPE_SAVEPROJECT_BEFORE)
            {
                Logger.Debug("MSG_DOCUMENTINFO_TYPE_SAVEPROJECT_BEFORE");
            }
            else if (data.type == C4dApi.MSG_DOCUMENTINFO_TYPE_SAVEPROJECT_AFTER)
            {
                Logger.Debug("MSG_DOCUMENTINFO_TYPE_SAVEPROJECT_AFTER");
            }
            else if (data.type == C4dApi.MSG_DOCUMENTINFO_TYPE_LOAD)
            {
                Logger.Debug("MSG_DOCUMENTINFO_TYPE_LOAD");
            }

            return true;
        }
/*
        public override bool Message(GeListNode node, int type, SWIGTYPE_p_void data)
        {
            Logger.Debug("From Message old.");
            return false;
        }
*/
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
        private const int FILEPATHTXT = 3001;
        private const int PROJECTNAMETXT = 3002;

        public override bool InitValues()
        {
            Logger.Debug("From UI Init.");

            return true;
        }

        public override bool CreateLayout()
        {
            bool res = base.CreateLayout();
            if (!res)
            {
                Logger.Debug("Parent call to GeDialog->CreateLayout() result in an ERROR.");
            }

            Logger.Debug("From create Layout.");

            SetTitle("My Dialog");

            bool b1 = GroupBegin(0, C4dApi.BFH_SCALEFIT, 5, 0, "GroupOne", 0);
            {
                bool b2 = GroupSpace(4, 4);
                bool b3 = GroupBorderSpace(4, 4, 4, 4);

                AddEditText(FILEPATHTXT, C4dApi.BFH_CENTER, 200, 30, C4dApi.EDITTEXT_HELPTEXT);
            }            
            GroupEnd();

            if (AddDlgGroup(C4dApi.DLG_OK | C4dApi.DLG_CANCEL) == false)
            {
                Logger.Debug("Error in AddDlgGroup. Buttons have not been added.");
                return false;
            }

            return res;
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