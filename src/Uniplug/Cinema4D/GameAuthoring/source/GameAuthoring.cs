using System;
using System.Diagnostics;
using C4d;
using Fusee.Math;
using FuseeAuthoringTools;
using FuseeAuthoringTools.c4dSet;

namespace GameAuthoring.source
{
    /// <summary>
    /// This struct is meant to hold a reference to the engine project during runtime.
    /// </summary>
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
    /// This is meant to communicate between the plugins.
    /// It can send and receive messages.
    /// </summary>
    [MessagePlugin(ID = 1035161, Name = "MessageHandler", Info = 1 << 29)]
    class FatPluginCommunicator : MessageData
    {
        /// <summary>
        /// The timer value in ms.
        /// So for now it checks for messages after each xx ms.
        /// </summary>
        private int _timerValue = 100;

        /// <summary>
        /// Not used constructor
        /// </summary>
        public FatPluginCommunicator() : base(false) { }

        /// <summary>
        /// This is the core message function from c4d.
        /// It can receive messages from the api.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bc"></param>
        /// <returns></returns>
        public override bool CoreMessage(int id, BaseContainer bc)
        {
            Logger.Debug("MessageHandler ID received: " + id);

            // Now switch for needed ids like C4d.MSG_DOCUMENTINFO_TYPE_SAVE_BEFORE

            return false;
        }

        /// <summary>
        /// This sets the timer interval. It measures how often we check for new messages and is an api function.
        /// It is measured in ms.
        /// </summary>
        /// <returns></returns>
 
        public override int GetTimer()
        {
            return _timerValue;
        }
    }

    
    /// <summary>
    /// This is the project loader Plugin class.
    /// It will load a fus project from a given path.
    /// If the project does not exist, it can create a new one.
    /// </summary>
    [CommandPlugin(1035056,
       Name = "Fusee Game Authoring Project Helper",
       HelpText = "Opens a Fusee Project and keeps it in memory.'",
       IconFile = "icon.tif")
    ]
    class FatProjectLoaderPlugin : CommandData
    {
        private int PL_PLUGINID = 1035056;

        private FuseeAuthoringToolsC4D fat;
        private CmdUiDialog cmdUi;

        public FatProjectLoaderPlugin() : base(false) { }

        /// <summary>
        /// This function is executed when starting the plugin. NOT at initializing.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
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

            return true;
        }

        /// <summary>
        /// This function receives messages for a plugin. 
        /// For now it only prints them out as this plugin type does not use messages in FuseeAT for now.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool Message(int type, SWIGTYPE_p_void data)
        {
            Logger.Debug("Data type from Command Plugin Message: " + data.GetType());
            return true;
        }
    }

    /// <summary>
    /// This is the asset tag plugin.
    /// It can create a tag on different scene objects and do stuff with it.
    /// It can also react to different messages calls from the api.
    /// </summary>
    [ TagPlugin(1034424,
        Name = "Fusee Asset Tag",
        IconFile = "icon.tif",
        Info = TagInfoFlag.TAG_VISIBLE,
        Description = "tagplugin",
        Disklevel = 0) ]
    class FatAssetTagPlugin : TagDataM
    {
        /// <summary>
        /// This is the reference to FuseeAT.
        /// </summary>
        private FuseeAuthoringToolsC4D fat;

        public FatAssetTagPlugin() : base(false) { }
        
        /// <summary>
        /// This is called when the plugin is initialized. So during startup of the application mostly.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This reacts to draw calls from the API.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="op"></param>
        /// <param name="bd"></param>
        /// <param name="bh"></param>
        /// <returns></returns>
        public override bool Draw(BaseTag tag, BaseObject op, BaseDraw bd, BaseDrawHelp bh) {
            //Logger.Debug("From Draw()");
            return true;
        }

        /// <summary>
        /// Reacts to different messages coming from the API and Application.
        /// Can detect if the scene has been saved etc.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool MessageDocumentInfo(GeListNode node, DocumentInfoData data)
        {
            if (data.type == C4dApi.MSG_DOCUMENTINFO_TYPE_SAVE_BEFORE)
            {
                Logger.Debug("MSG_DOCUMENTINFO_TYPE_SAVE_BEFORE");
            }
            else if (data.type == C4dApi.MSG_DOCUMENTINFO_TYPE_SAVE_AFTER)
            {
                Logger.Debug("MSG_DOCUMENTINFO_TYPE_SAVE_AFTER");
                // Now start the .fus export 
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

        /// <summary>
        /// Little helper function to retrieve the name of a scene object.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
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
    /// It can display a form but it cannot react for now due to a very hard api bug due to the wrapping.
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

        /// <summary>
        /// This function creates a layout for a dialog window. For now it is a little opening dialog.
        /// It cannot use decorations like buttons for now because of the bug in the api / wrapping.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// This function receives messages from the api so we can react to changes in the application.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override int Message(BaseContainer msg, BaseContainer result)
        {
            Logger.Debug("From ui Message.");
            int msgid = msg.GetId();

            return 0;
        }
    }
}