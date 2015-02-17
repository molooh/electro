using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using fuProjectGen;

namespace GameAuthoringTools
{
    public static class GlobalValues
    {
        public static const String PROJECTFOLDER = "/projects/";
    }

    /// <summary>
    /// This enum is for returning more readable values in functions than just boolean.
    /// </summary>
    public enum ToolState {
        OK = 0,
        ERROR = 1,
        WARNING = 2
    }

    public enum ProjectState
    {
        Clean = 0, // means open, too
        Dirty = 1,
        Corrupt = 2,
        Closed = 3
    }

    public struct EngineProject
    {
        public String sysPath;
        public String projPath;
        public String csproj;
        public String csprojPath;
        public ProjectState projectState;
    }

    // TODO: Can give out references to other tool classes?
    /// <summary>
    /// The main part for the authoring tools. Registers modules and functionality.    
    /// </summary>
    public class FuseeAuthoringTools
    {
        // Tool Classes for communication
        private FuseeProjectManager fpManager;
        private FuseeFileManager ffManager;
        private FuseeClassManager fcManager;
        private FuseeBuildManager fbManager;
        
        // public
        public EngineProject project;

        public FuseeAuthoringTools()
        {
            ffManager = new FuseeFileManager();
        }
        
        /// <summary>
        /// Creates a project from a name and a path in a fusee binary engine "clone".
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pPath"></param>
        /// <returns></returns>
        public ToolState CreateProject(String pName, String pPath)
        {
            fpManager = new FuseeProjectManager(pName, pPath);

            if (fpManager.State == ProjectState.Clean)
                return ToolState.OK;
                        
            return ToolState.ERROR;
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
        private String pathToProject;   // the root dir.
        private ProjectState pstate;    // write state of the project.
        private ProjectGenerator pg;
        private String projectName;
        private EngineProject project;

        /// <summary>
        /// Constructor.
        /// </summary>
        public FuseeProjectManager(String pName, String pPath)
        {
            pathToProject = pPath;
            pstate = ProjectState.Clean;

            if (!(CreateProject(pName) == ToolState.OK))
            {
                pstate = ProjectState.Corrupt;
            }
                            

            if (State == ProjectState.Clean)
            {
                projectName = pName;
                project = new EngineProject
                {
                    sysPath = PathToProject,
                    projPath = GlobalValues.PROJECTFOLDER + GenerateCsProjName(ProjectName),
                    csproj = ProjectName,
                    csprojPath = PathToProject + GlobalValues.PROJECTFOLDER + GenerateCsProjName(ProjectName),
                    projectState = ProjectState.Clean
                };
            }

        }

        /// <summary>
        /// Creates a new fusee project, names it, inits the basics etc.
        /// </summary>
        /// <param name="pname"></param>
        /// <returns>ToolState enum state value</returns>
        public ToolState CreateProject(String pName)
        {
            pstate = ProjectState.Dirty;
            pg = new ProjectGenerator(pName, pathToProject);

            if(pg == null)
                return ToolState.ERROR;

            pstate = ProjectState.Clean;

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

            // TODO: Assign a new EngineProject Struct with all the paths so it is opened. Rebuild other paths etc.

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

        private String GenerateCsProjName(String pName)
        {
            return pName + ".csproj";
        }

        #region GetterAndSetter

        public String PathToProject
        {
            get { return pathToProject; }
        }

        public String ProjectName
        {
            get { return projectName; }
        }

        public ProjectState State
        {
            get { return pstate; }
        }

        public EngineProject GetProject
        {
            get { return project; }
        }
        #endregion

    }

    /// <summary>
    /// Will be used to create various files if needed.
    /// </summary>
    public class FuseeFileManager
    {
        FuseeClassManager fcManager;

        public FuseeFileManager()
        {
            fcManager = new FuseeClassManager();
        }

        public ToolState CreateCSharpClass(String fname, String fpath)
        {
            return ToolState.OK;
        }

    }

    /// <summary>
    /// Used to take care of the partial classes distributed in the system. Quite important manager to take care of the code. Creates classes and partials etc.
    /// </summary>
    public class FuseeClassManager
    {
        public FuseeClassManager()
        {
            
        }

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
    /// Will be used to call the compiler etc. from IDE software. Mostly 3D modeling software - to run a project.
    /// </summary>
    public class FuseeBuildManager
    {
        // This is for the builds.
    }

}
