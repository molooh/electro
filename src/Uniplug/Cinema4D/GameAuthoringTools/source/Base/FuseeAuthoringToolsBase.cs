using System;
using System.Xml.Serialization;

namespace FuseeAuthoringTools
{
    public static class GlobalValues
    {
        public const String PROJECTFOLDER = "/projects";
        public const String COMPILEINCLUDESTART = "    <Compile Include=\"Main.cs\" />";
    }

    /// <summary>
    /// This enum is for returning more readable values in functions than just boolean.
    /// </summary>
    public enum ToolState {
        OK = 0,
        ERROR = 1,
        WARNING = 2,
    }

    /// <summary>
    /// This is the state of the project.
    /// It can help to prevent working with corrupt files when used correctly.
    /// </summary>
    public enum ProjectState
    {
        Clean = 0, // means open and okay, too
        Dirty = 1,
        Corrupt = 2,
        Closed = 3
    }

    /// <summary>
    /// A container for some information about a project.
    /// Keeps the project state and also the paths.
    /// </summary>
    public struct EngineProject
    {
        /// <summary>
        /// Full System Path to Solution
        /// </summary>
        [XmlElement("PathToSolutionFolder")]
        public String PathToSolutionFolder;

        /// <summary>
        /// /projects/ProjectName
        /// </summary>
        [XmlElement("PathToProjectFolder")]
        public String PathToProjectFolder;

        /// <summary>
        /// ProjectName
        /// </summary>
        [XmlElement("ProjectName")]
        public String NameofCsProject; // FileName

        /// <summary>
        /// Full path to ProjectName.csproj
        /// </summary>
        [XmlElement("PathToCSProjectFile")]
        public String PathToCsProjectFile;

        /// <summary>
        /// Solution name without .sln
        /// </summary>
        [XmlElement("SolutionName")]
        public String SolutionName;

        /// <summary>
        /// Current state of the project.
        /// </summary>
        [XmlElement("CurrentProjectState")]
        public ProjectState ProjectState;

        /// <summary>
        /// Accessor for the project path.
        /// </summary>
        /// <returns></returns>
        public String GetPathToProjectFolder()
        {
            return this.PathToSolutionFolder + this.PathToProjectFolder;
        }

        /// <summary>
        /// Accessor for the source code path of the project.
        /// </summary>
        /// <returns></returns>
        public String GetPathToProjectSource()
        {
            return this.PathToSolutionFolder + this.PathToProjectFolder + "/Source/";
        }

        /// <summary>
        /// Accessor for the projects name.
        /// </summary>
        /// <returns></returns>
        public String GetProjectFileName()
        {
            return this.NameofCsProject + ".csproj";
        }
    }

    /// <summary>
    /// Asset and Code Relationship Data.
    /// A containter to save an asset code file relation.
    /// </summary>
    public struct ACRelationData
    {
        /// <summary>
        /// An id or hash from the asset file from c4d.
        /// </summary>
        [XmlElement("AssetID")]
        public String AssetID;

        /// <summary>
        /// The name of the asset in c4d.
        /// </summary>
        [XmlElement("AssetName")]
        public String AssetName;

        /// <summary>
        /// The name of the code file added to the asset.
        /// </summary>
        [XmlElement("CodeFileName")]
        public String CodeFileName;

        /// <summary>
        /// This is the path to the code file.
        /// </summary>
        [XmlElement("CodeFilePath")]
        public String CodeFilePath;

        /// <summary>
        /// This is a hash to uniquely identify this connection.
        /// The hash should be generated out of the assetid and the codefilename.
        /// </summary>
        [XmlElement("ConnectionID")]
        public String ConnectionID;

        /// <summary>
        /// Retuns the complete path to the code file.
        /// </summary>
        /// <returns></returns>
        public String GetPathToCodeFile()
        {
            return CodeFilePath + CodeFileName;
        }
    }

    /// <summary>
    /// This is/should be instanciated for every object that has an asset tag applied and code added
    /// to it. It represents the connection between modeling editor objects and code so
    /// the connections can be re-established when loading a project.
    /// </summary>
    public struct AssetObject
    {
        [XmlElement("ModelEditorName")]
        public String objectName; // The name of the cinema 4d object.

        [XmlElement("ModelEditorID")]
        public String objectID; // Should be a generated hash code.

        [XmlElement("ClassName")]
        public String className; // The name of the attached class file? maybe just the file name and a hash?

        [XmlElement("NameSpace")]
        public String nameSpace; // Do not need this for now
    }

    /// <summary>
    /// This should be implemented by all classes that represent a connection to a plugin
    /// for a modeling software that should be used as en editor.
    /// </summary>
    interface IFuseeAuthoringTools
    {
        /// <summary>
        /// Creates a project from different parameters.
        /// </summary>
        /// <param name="slnName"></param>
        /// <param name="pName"></param>
        /// <param name="pPath"></param>
        /// <returns></returns>
        Boolean CreateProject(String slnName, String pName, String pPath);

        /// <summary>
        /// Saves a project that is currently opened.
        /// </summary>
        /// <returns></returns>
        Boolean SaveProject();

        /// <summary>
        /// Opens an existing FuseeAT project.
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pPath"></param>
        /// <returns></returns>
        Boolean OpenProject(String pName, String pPath);

        /// <summary>
        /// Updates a project state etc.
        /// </summary>
        /// <returns></returns>
        Boolean UpdateProject();

        /// <summary>
        /// Creates a new C# code class and inserts it to the project.
        /// </summary>
        /// <param name="pName"></param>
        /// <returns></returns>
        Boolean CreateNewClass(String pName);

        /// <summary>
        /// Creates every filetype that is not a C# code file.
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="fpath"></param>
        /// <returns></returns>
        Boolean CreateNewFile(String fname, String fpath);

        /// <summary>
        /// Should build the project with the msbuild framework.
        /// </summary>
        /// <returns></returns>
        Boolean BuildProject();

        /// <summary>
        /// Exports something to the fus file format.
        /// Using the fusee exporter library.
        /// </summary>
        /// <returns></returns>
        Boolean ExportToFus();

        /// <summary>
        /// Accessor for the engine project.
        /// </summary>
        /// <returns></returns>
        EngineProject GetEngineProject();

        /// <summary>
        /// Accessor for the project state.
        /// </summary>
        /// <returns></returns>
        int GetEngineProjectState();
    }
}