using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAuthoringTools
{
    /// <summary>
    /// This enum is for returning better values in functions.
    /// </summary>
    public enum ToolState {
        OK = 0,
        ERROR = 1,
        WARNING = 2
    }

    public enum ProjectState
    {
        Clean = 0,
        Dirty = 1,
        Corrupt = 2
    }

    /// <summary>
    /// The main part for the authoring tools. Registers modules and functionality.
    /// Can give out references to other tool classes.
    /// </summary>
    public class FuseeAuthoringTools
    {
        private String s;

        // Tool Classes for communication
        private FuseeProjectManager fpManager;
        private FuseeFileManager ffManager;
        private FuseeClassManager fcManager;
        private FuseeBuildManager fbManager;
                
        public FuseeAuthoringTools()
        {
            fpManager = new FuseeProjectManager();
            ffManager = new FuseeFileManager();
            fcManager = new FuseeClassManager();
            fbManager = new FuseeBuildManager();

            s = "This is a test from the fusee authoring tools set.";
        }

        public String RetrieveInformation()
        {
            return this.s;
        }

        #region Getter and Setter

        public FuseeProjectManager ProjectManager
        {
            get
            {
                return fpManager;
            }
        }

        public FuseeFileManager FileManager
        {
            get
            {
                return ffManager;
            }
        }

        public FuseeClassManager ClassManager
        {
            get
            {
                return fcManager;
            }
        }

        public FuseeBuildManager BuildManager
        {
            get
            {
                return fbManager;
            }
        }

        #endregion

    }

    /// <summary>
    /// Handles all the different work with the project. Saving, opening, creating. All this.
    /// </summary>
    public class FuseeProjectManager
    {
        String pathToProject;   // the root dir.
        ProjectState pstate;    // write state of the project.

        /// <summary>
        /// Constructor.
        /// </summary>
        public FuseeProjectManager()
        {
            pathToProject = "";
            pstate = ProjectState.Clean;
        }

        /// <summary>
        /// Creates a new fusee project, names it, inits the basics etc.
        /// </summary>
        /// <param name="pname"></param>
        /// <returns>ToolState enum state value</returns>
        public ToolState CreateProject(String pname)
        {
            // TODO: Check for allowed characters etc.

            // TODO: Create actual project. Duplicate engine.dlls etc.

            return ToolState.OK;
        }
        
        /// <summary>
        /// Opens a project and returns some handle? So the user can use it.
        /// </summary>
        /// <param name="pname"></param>
        /// /// <param name="pathToProject"></param>
        /// <returns>ToolState enum state value</returns>
        public ToolState OpenProject(String pname, out String pathToProject)
        {
            pathToProject = "";

            // TODO: Assign path value etc. Do some useful stuff. Can keep a pointer to the open project so it's faster to save etc?

            return ToolState.OK;
        }

        /// <summary>
        /// Updates a project with current values from the ide. Mostly called from "Save" or "Export".
        /// </summary>
        /// <param name="pname"></param>
        /// <returns>ToolState enum state value</returns>
        public ToolState UpdateProject(String pname)
        {
            // TODO: Open project, update stuff.
            String pathToProject;
            if (OpenProject(pname, out pathToProject) == ToolState.OK)
            {
                // Save.
            }

            return ToolState.OK;
        }

    }

    /// <summary>
    /// Used to take care of the partial classes distributed in the system. Quite important manager to take care of the code. Creates classes and partials etc.
    /// </summary>
    public class FuseeClassManager
    {

        public ToolState CreateNewClass(String cname)
        {
            // TODO: Create a new class for the ide and the coder.
            // 1) Create it as partial
            // 2) Create the file in the correct place. /project/src
            // 3) Add it to the *.csproj and or *.sln
            // 4) Save a ref to a file so we know what it's attached to.

            return ToolState.OK;
        }

        public ToolState UpdatePartialClass()
        {
            // TODO: Perhaps we need to sync the partial class sometimes?
            return ToolState.OK;
        }

    }

    /// <summary>
    /// Will be used to create various files if needed.
    /// </summary>
    public class FuseeFileManager
    {
        public ToolState CreateCSharpClass(String fname, String fpath)
        {
            return ToolState.OK;
        }

    }

    /// <summary>
    /// Will be used to call the compiler etc. from IDE software. Mostly 3D modeling software - to run a project.
    /// </summary>
    public class FuseeBuildManager
    {

    }

}
