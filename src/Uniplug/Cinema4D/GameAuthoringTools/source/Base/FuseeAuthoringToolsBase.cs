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

    public enum ProjectState
    {
        Clean = 0, // means open, too
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

        public String GetPathToProjectFolder()
        {
            return this.PathToSolutionFolder + this.PathToProjectFolder;
        }

        public String GetPathToProjectSource()
        {
            return this.PathToSolutionFolder + this.PathToProjectFolder + "/Source/";
        }

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
    /// This is instanciated for every object that has an asset tag applied and code added
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
        Boolean CreateProject(String slnName, String pName, String pPath);
        Boolean SaveProject();
        Boolean OpenProject(String pName, String pPath);
        Boolean UpdateProject();

        Boolean CreateNewClass(String pName);
        Boolean CreateNewFile(String fname, String fpath);

        Boolean BuildProject();

        Boolean ExportToFus();

        EngineProject GetEngineProject();
        int GetEngineProjectState();
    }
}