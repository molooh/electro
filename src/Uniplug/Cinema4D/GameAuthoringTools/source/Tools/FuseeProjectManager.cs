using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using FuseeProjectGenerator;

namespace FuseeAuthoringTools.tools
{
    /// <summary>
    /// Handles all the different work with the project. Saving, opening, creating. All this.
    /// </summary>
    public class FuseeProjectManager
    {
        private ProjectGenerator _projectGenerator;
        private EngineProject _engineProject;
        private FuseeFileManager _fileManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public FuseeProjectManager()
        {
           
        }

        /// <summary>
        /// Creates a new fusee project, names it, inits the basics etc.
        /// </summary>
        /// <param name="pname"></param>
        /// <returns>ToolState enum state value</returns>
        public ToolState CreateProject(String slnName, String pName, String pPath)
        {
            // Check if project already exists?
            if (DoesProjectExist(pName, pPath))
            {
                // If yes open it.
                OpenProject(pName, pPath);
            }
            else
            {
                // If not create it.
                _engineProject.ProjectState = ProjectState.Dirty;
                _projectGenerator = new ProjectGenerator(pName, pPath);

                if (_projectGenerator == null)
                    return ToolState.ERROR;

                _engineProject.ProjectState = ProjectState.Clean;
                
                _engineProject = new EngineProject
                {
                    PathToSolutionFolder = pPath,
                    PathToProjectFolder = GlobalValues.PROJECTFOLDER + "/" + pName,
                    NameofCsProject = pName,
                    PathToCsProjectFile = pPath + GlobalValues.PROJECTFOLDER + "/" + pName + "/" + GenerateCsProjName(pName),
                    ProjectState = ProjectState.Clean,
                    SolutionName = slnName
                };
                
                // Create the xml project folder for the relations this tool uses.
                if (!CreateProjectSettings())
                    return ToolState.ERROR;    
                    
                SaveProject();
            }
            // Create a file manager so we can actually work with files ;)
            _fileManager = new FuseeFileManager(this);

            return ToolState.OK;
        }

        /// <summary>
        /// Saves a project to an XML file. So it can be loaded again if needed.
        /// </summary>
        /// <returns>ToolState enum state value</returns>
        public ToolState SaveProject()
        {
            if (_engineProject.ProjectState != ProjectState.Dirty || _engineProject.ProjectState != ProjectState.Corrupt)
            {
                SerializeToXML(_engineProject);
                return ToolState.OK;
            }
            // Cannot save because project is not clean.
            return ToolState.ERROR;
        }

        /// <summary>
        /// Opens a project and returns some handle? So the user can use it.
        /// </summary>
        /// <param name="pname"></param>
        /// /// <param name="pathToProject"></param>
        /// <returns>ToolState enum state value.</returns>
        public ToolState OpenProject(String pName, String pPath)
        {
            _engineProject = DeserializeFromXML(pName, pPath);

            return ToolState.OK;
        }

        public ToolState BuildProject(EngineProject ep)
        {
            ToolState res = FuseeBuildManager.BuildSolution(ep);

            return ToolState.OK;
        }

        public ToolState CreateClass(String className)
        {
            _fileManager.CreateCSharpClass(className, _engineProject.NameofCsProject);
            return ToolState.OK;
        }

        public ToolState AddCodeComponent(String assetName, String codeFileName, String codeFilePath, String assetID)
        {
            ACRelationData acr = new ACRelationData()
            {
                AssetID = assetID,
                ConnectionID = GenerateHash(assetID, GenerateHash(codeFilePath + codeFileName)),
                AssetName = assetName,
                CodeFileName = codeFileName,
                CodeFilePath = codeFilePath
            };

            SerializeAssetRelationToXML(acr, acr.ConnectionID);

            // TODO: Insert the relation in the csproj of the engine project.
            // How?

            return ToolState.OK;
        }

        private string GenerateHash(String h1, String h2 = null)
        {
            // Code idea from msdn.
            UnicodeEncoding UE = new UnicodeEncoding();

            String hashStr = h2 == null ? h1 : h1 + h2;

            byte[] MessageBytes = UE.GetBytes(hashStr);

            SHA1Managed SHhash = new SHA1Managed();

            var hash = SHhash.ComputeHash(MessageBytes);

            return hash.ToString();
        }

        public ToolState ExportSceneToFus()
        {
            // TODO: Export the scene to fus somehow.
            return ToolState.OK;
        }

        private ToolState SerializeToXML(EngineProject p)
        {
            XmlSerializer ser = new XmlSerializer(typeof(EngineProject));
            TextWriter tw = new StreamWriter(p.PathToSolutionFolder + p.PathToProjectFolder + "/" + p.NameofCsProject + ".xml");
            ser.Serialize(tw, p);
            tw.Close();

            return ToolState.OK;
        }

        private EngineProject DeserializeFromXML(String pName, String pathToXML)
        {
            if (_engineProject.NameofCsProject !=  null)
                return _engineProject;

            XmlSerializer des = new XmlSerializer(typeof(EngineProject));
            TextReader tr = new StreamReader(pathToXML + "/projects/" + pName + "/" + pName + ".xml");
            var ep = (EngineProject)des.Deserialize(tr);
            tr.Close();

            return ep;
        }

        private ToolState SerializeAssetRelationToXML(ACRelationData acr, String hash)
        {
            XmlSerializer ser = new XmlSerializer(typeof(EngineProject));
            TextWriter tw = new StreamWriter(FuseeEngineProject.PathToSolutionFolder + "ProjectSettings/" + hash + ".xml");
            ser.Serialize(tw, acr);
            tw.Close();

            return ToolState.OK;
        }

        private ACRelationData DeserializeAssetRelationFromXML(String Name, String hash)
        {
            XmlSerializer des = new XmlSerializer(typeof(ACRelationData));
            TextReader tr = new StreamReader(FuseeEngineProject.PathToSolutionFolder + "/ProjectSettings/" + hash + ".xml");
            var acr = (ACRelationData)des.Deserialize(tr);
            tr.Close();

            return acr;
        }

        private Boolean DoesProjectExist(String pName, String pPath)
        {
            if (File.Exists(pPath + GlobalValues.PROJECTFOLDER + "/" + pName + "/" + pName + ".csproj"))
                return true;

            return false;
        }

        private String GenerateCsProjName(String pName)
        {
            return pName + ".csproj";
        }

        private bool CreateProjectSettings()
        {
            String slnPath = FuseeEngineProject.PathToSolutionFolder;

            if (!Directory.Exists(slnPath + "/ProjectSettings"))
                Directory.CreateDirectory(slnPath + "/ProjectSettings");

            return Directory.Exists(slnPath + "/ProjectSettings");
        }

        public EngineProject FuseeEngineProject
        {
            get { return _engineProject; }
            set { _engineProject = value; }
        }

        public void SetProjectDirty()
        {
            _engineProject.ProjectState = ProjectState.Dirty;
        }

        public void SetProjectClean()
        {
            _engineProject.ProjectState = ProjectState.Clean;
        }

    }
}
