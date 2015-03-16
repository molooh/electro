using System;
using System.IO;
using FuseeAuthoringTools.tools;

namespace FuseeAuthoringTools.c4d
{
    /// <summary>
    /// The main part for the authoring tools. Registers modules and functionality.    
    /// </summary>
    public class FuseeAuthoringToolsC4D : IFuseeAuthoringTools
    {
        // Tool Classes for communication
        private FuseeProjectManager _projectManager;

        public FuseeAuthoringToolsC4D()
        {
            _projectManager = new FuseeProjectManager();
        }

        public bool CreateProject(String slnName, String pName, String pPath)
        {
            if (_projectManager.CreateProject(slnName, pName, pPath) == ToolState.OK)
                return true;

            return false;
        }

        public bool SaveProject()
        {
            if (_projectManager.SaveProject() == ToolState.OK)
                return true;

            return false;
        }

        public bool OpenProject(String pName, String pPath)
        {
            if (_projectManager.OpenProject(pName, pPath) == ToolState.OK)
                return true;

            return false;
        }

        public bool UpdateProject()
        {
            throw new NotImplementedException();
        }

        public bool CreateNewClass(String pName)
        {
            
            if (_projectManager.CreateClass(pName) == ToolState.OK)
                return true;
            
            return false;
        }

        public bool CreateNewFile(String fname, String fpath)
        {
            // TODO: Check if the file is existing in the correct location.

            return false;
        }

        public bool BuildProject()
        {
            _projectManager.BuildProject(_projectManager.FuseeEngineProject);

            return true;
        }

        /// <summary>
        /// Export the current scene to .fus format and save it in the project directory.
        /// </summary>
        /// <returns></returns>
        public bool ExportToFus()
        {
            // This method should use the exporter from christoph or so to somehow export the scene to .fus and save it in the projects directory.

            return true;
        }

        /// <summary>
        /// Returns the project struct with paths etc.
        /// </summary>
        /// <returns></returns>
        public EngineProject GetEngineProject()
        {
            return _projectManager.FuseeEngineProject;
        }

        /// <summary>
        /// Returns the current state of the project.
        /// </summary>
        /// <returns></returns>
        public int GetEngineProjectState()
        {
            return (int)_projectManager.FuseeEngineProject.ProjectState;
        }
    }
}
