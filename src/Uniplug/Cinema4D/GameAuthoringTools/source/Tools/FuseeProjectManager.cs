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
        /// <summary>
        /// Reference to the fusee project generator.
        /// This code has been reused and changed during the master thesis.
        /// </summary>
        private ProjectGenerator _projectGenerator;

        /// <summary>
        /// Keeps a reference to the engine project in the memory.
        /// Supports if one needs paths or names during runtime etc.
        /// </summary>
        private EngineProject _engineProject;

        /// <summary>
        /// References the fileManager object.
        /// It can make calls on this object.
        /// </summary>
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

        /// <summary>
        /// Builds the project using msbuild.
        /// </summary>
        /// <param name="ep"></param>
        /// <returns></returns>
        public ToolState BuildProject(EngineProject ep)
        {
            ToolState res = FuseeBuildManager.BuildSolution(ep);

            return ToolState.OK;
        }

        /// <summary>
        /// Creates a new class with the corresponding parameter as its name.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public ToolState CreateClass(String className)
        {
            _fileManager.CreateCSharpClass(className, _engineProject.NameofCsProject);
            return ToolState.OK;
        }

        /// <summary>
        /// Adds a component to the project using an asset name and some code information.
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="codeFileName"></param>
        /// <param name="codeFilePath"></param>
        /// <param name="assetID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper function to generate a hash to represent unique ids for scene objects.
        /// </summary>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Should start the export of a scene to the .fus format.
        /// </summary>
        /// <returns></returns>
        public ToolState ExportSceneToFus()
        {
            // TODO: Export the scene to fus somehow.
            return ToolState.OK;
        }

        /// <summary>
        /// Serializes an engine project object to an xml file on disk in the fusee project.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private ToolState SerializeToXML(EngineProject p)
        {
            XmlSerializer ser = new XmlSerializer(typeof(EngineProject));
            TextWriter tw = new StreamWriter(p.PathToSolutionFolder + p.PathToProjectFolder + "/" + p.NameofCsProject + ".xml");
            ser.Serialize(tw, p);
            tw.Close();

            return ToolState.OK;
        }

        /// <summary>
        /// Deserializes an engine project object from an xml file on disk in the fusee project.
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pathToXML"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Serializes a xml representation of an asset relation that is in the memory. 
        /// </summary>
        /// <param name="acr"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        private ToolState SerializeAssetRelationToXML(ACRelationData acr, String hash)
        {
            XmlSerializer ser = new XmlSerializer(typeof(EngineProject));
            TextWriter tw = new StreamWriter(FuseeEngineProject.PathToSolutionFolder + "ProjectSettings/" + hash + ".xml");
            ser.Serialize(tw, acr);
            tw.Close();

            return ToolState.OK;
        }

        /// <summary>
        /// Deserializes a xml representation of an asset relation that is on disk. 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        private ACRelationData DeserializeAssetRelationFromXML(String Name, String hash)
        {
            XmlSerializer des = new XmlSerializer(typeof(ACRelationData));
            TextReader tr = new StreamReader(FuseeEngineProject.PathToSolutionFolder + "/ProjectSettings/" + hash + ".xml");
            var acr = (ACRelationData)des.Deserialize(tr);
            tr.Close();

            return acr;
        }

        /// <summary>
        /// Checks if a project does already exist in a fusee engine binary solution.
        /// Returns false if the project does not exist.
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pPath"></param>
        /// <returns></returns>
        private Boolean DoesProjectExist(String pName, String pPath)
        {
            if (File.Exists(pPath + GlobalValues.PROJECTFOLDER + "/" + pName + "/" + pName + ".csproj"))
                return true;

            return false;
        }

        /// <summary>
        /// This generates a name for a csproj file.
        /// </summary>
        /// <param name="pName"></param>
        /// <returns></returns>
        private String GenerateCsProjName(String pName)
        {
            return pName + ".csproj";
        }

        /// <summary>
        /// This creates a specific folder for the FuseeAT project
        /// </summary>
        /// <returns></returns>
        private bool CreateProjectSettings()
        {
            String slnPath = FuseeEngineProject.PathToSolutionFolder;

            if (!Directory.Exists(slnPath + "/ProjectSettings"))
                Directory.CreateDirectory(slnPath + "/ProjectSettings");

            return Directory.Exists(slnPath + "/ProjectSettings");
        }

        /// <summary>
        /// Represents the accessor for the fuseeEngineProject.
        /// </summary>
        public EngineProject FuseeEngineProject
        {
            get { return _engineProject; }
            set { _engineProject = value; }
        }

        /// <summary>
        /// Sets the project state to dirty.
        /// Used while changing important stuff on the project.
        /// </summary>
        public void SetProjectDirty()
        {
            _engineProject.ProjectState = ProjectState.Dirty;
        }

        /// <summary>
        /// Sets the project state to clean.
        /// Used after correctly done changes.
        /// </summary>
        public void SetProjectClean()
        {
            _engineProject.ProjectState = ProjectState.Clean;
        }

    }
}
