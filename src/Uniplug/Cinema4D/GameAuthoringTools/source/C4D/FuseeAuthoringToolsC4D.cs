using System;
using System.IO;
using FuseeAuthoringTools.tools;

namespace FuseeAuthoringTools.c4dSet
{
    /// <summary>
    /// The main part for the authoring tools. Registers modules and functionality.    
    /// </summary>
    public class FuseeAuthoringToolsC4D : IFuseeAuthoringTools
    {
        /// <summary>
        /// The reference to the manager of the FuseeAT project.
        /// </summary>
        private FuseeProjectManager _projectManager;

        /// <summary>
        /// Constructor for the module.
        /// </summary>
        public FuseeAuthoringToolsC4D()
        {
            _projectManager = new FuseeProjectManager();
        }

        /// <summary>
        /// Creates a project using the parameters given.
        /// Returns true - converts fuseeAT state - if the project is created correctly.
        /// </summary>
        /// <param name="slnName"></param>
        /// <param name="pName"></param>
        /// <param name="pPath"></param>
        /// <returns></returns>
        public bool CreateProject(String slnName, String pName, String pPath)
        {
            if (_projectManager.CreateProject(slnName, pName, pPath) == ToolState.OK)
                return true;

            return false;
        }

        /// <summary>
        /// Saves a project to a FuseeEngineProject typed object.
        /// </summary>
        /// <returns></returns>
        public bool SaveProject()
        {
            if (_projectManager.SaveProject() == ToolState.OK)
                return true;

            return false;
        }

        /// <summary>
        /// Can open a project if existing.
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pPath"></param>
        /// <returns></returns>
        public bool OpenProject(String pName, String pPath)
        {
            if (_projectManager.OpenProject(pName, pPath) == ToolState.OK)
                return true;

            return false;
        }

        /// <summary>
        /// Updates a project with different new information.
        /// </summary>
        /// <returns></returns>
        public bool UpdateProject()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new class in the C# project.
        /// The class is named with the parameter.
        /// </summary>
        /// <param name="pName"></param>
        /// <returns></returns>
        public bool CreateNewClass(String pName)
        {
            
            if (_projectManager.CreateClass(pName) == ToolState.OK)
                return true;
            
            return false;
        }

        /// <summary>
        /// Creates a new file that is not a class. Not used right now.
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="fpath"></param>
        /// <returns></returns>
        public bool CreateNewFile(String fname, String fpath)
        {
            // TODO: Check if the file is existing in the correct location.

            return false;
        }

        /// <summary>
        /// Calls the build manager to build the project.
        /// </summary>
        /// <returns></returns>
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
            // TODO: This method should use the exporter from christoph or so to somehow export the scene to .fus and save it in the projects directory.
            // Perhaps it's possible to just use the same functionality that is used in the exporter.

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
