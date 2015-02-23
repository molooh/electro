using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using fuProjectGen;

namespace FuseeAuthoringTools.source
{
    /// <summary>
    /// Handles all the different work with the project. Saving, opening, creating. All this.
    /// </summary>
    public class FuseeProjectManager
    {
        private String pathToProject;   // the root dir.
        private ProjectGenerator pg;
        private String projectName;
        private EngineProject project;

        /// <summary>
        /// Constructor.
        /// </summary>
        public FuseeProjectManager(String pName, String pPath)
        {
            pathToProject = pPath;
            project.projectState = ProjectState.Clean;

            if (!(CreateProject(pName) == ToolState.OK))
            {
                project.projectState = ProjectState.Corrupt;
            }

            if (State == ProjectState.Clean)
            {
                projectName = pName;
                project = new EngineProject
                {
                    sysPath = PathToProject,
                    projPath = GlobalValues.PROJECTFOLDER + "/" + pName,
                    nameofCSPROJ = ProjectName,
                    pathToCSPROJ = PathToProject + GlobalValues.PROJECTFOLDER + "/" + pName + "/" + GenerateCsProjName(ProjectName),
                    projectState = ProjectState.Clean
                };

                SaveProject();
            }

        }

        /// <summary>
        /// Creates a new fusee project, names it, inits the basics etc.
        /// </summary>
        /// <param name="pname"></param>
        /// <returns>ToolState enum state value</returns>
        public ToolState CreateProject(String pName)
        {
            project.projectState = ProjectState.Dirty;
            pg = new ProjectGenerator(pName, pathToProject);

            if (pg == null)
                return ToolState.ERROR;

            project.projectState = ProjectState.Clean;

            return ToolState.OK;
        }

        /// <summary>
        /// Saves a project to an XML file. So it can be loaded again if needed.
        /// </summary>
        /// <returns>ToolState enum state value</returns>
        public ToolState SaveProject()
        {
            project.projectState = ProjectState.Dirty;

            SerializeToXML(project);

            project.projectState = ProjectState.Clean;

            return ToolState.OK;
        }

        /// <summary>
        /// Opens a project and returns some handle? So the user can use it.
        /// </summary>
        /// <param name="pname"></param>
        /// /// <param name="pathToProject"></param>
        /// <returns>ToolState enum state value</returns>
        public ToolState OpenProject(String pname, String path)
        {
            // TODO: Assign a new EngineProject Struct with all the paths so it is opened. Rebuild other paths etc.
            if (DeserializeFromXML(pname, path) == ToolState.OK)
                return ToolState.OK;

            return ToolState.ERROR;
        }

        public ToolState SerializeToXML(EngineProject p)
        {
            XmlSerializer ser = new XmlSerializer(typeof(EngineProject));
            TextWriter tw = new StreamWriter(p.sysPath + p.projPath + "/" + p.nameofCSPROJ + ".xml");
            ser.Serialize(tw, p);
            tw.Close();

            return ToolState.OK;
        }

        public ToolState DeserializeFromXML(String pName, String pathToXML)
        {
            XmlSerializer des = new XmlSerializer(typeof(EngineProject));
            TextReader tr = new StreamReader(pathToXML + "/" + pName + ".xml"); // path to xml.
            var ep = (EngineProject)des.Deserialize(tr);
            tr.Close();

            project = ep;

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
            get { return project.projectState; }
        }

        public EngineProject GetProject
        {
            get { return project; }
        }

        public EngineProject SetProject
        {
            set { project = value; }
        }

        #endregion

    }
}
