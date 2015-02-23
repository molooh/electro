using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuseeAuthoringTools.source
{
    /// <summary>
    /// The main part for the authoring tools. Registers modules and functionality.    
    /// </summary>
    public class FuseeAuthoringToolsC4D
    {
        // Tool Classes for communication
        private FuseeProjectManager fpManager;
        private FuseeFileManager ffManager;

        public FuseeAuthoringToolsC4D()
        {
            ffManager = new FuseeFileManager(this);
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
            {
                return ToolState.OK;
            }

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

        public EngineProject EngineProject
        {
            get { return fpManager.GetProject; }
            set { fpManager.SetProject = value; }
        }
        #endregion

    }
}
