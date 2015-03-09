using System;
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

        public bool CreateNewClass(String pName, String pPath)
        {
            
            if (_projectManager.CreateClass(pName) == ToolState.OK)
                return true;
            
            return false;
        }

        public bool CreateNewFile()
        {
            throw new NotImplementedException();
        }

        public bool BuildProject()
        {
            _projectManager.BuildProject(_projectManager.FuseeEngineProject);

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
            return (int)_projectManager.FuseeEngineProject.projectState;
        }
    }
}
