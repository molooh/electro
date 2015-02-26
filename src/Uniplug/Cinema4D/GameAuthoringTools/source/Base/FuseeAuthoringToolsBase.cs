using System;
using System.Xml.Serialization;

namespace FuseeAuthoringTools
{
    public static class GlobalValues
    {
        public const String PROJECTFOLDER = "/projects";
        public const String COMPILEINCLUDESTART = "     <Compile Include=\"Main.cs\" />";
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
        [XmlElement("PathToSolutionFolder")]
        public String sysPath; // path to solution dir

        [XmlElement("PathToProjectFolder")]
        public String projPath; // sysPath + /projects/pName/

        [XmlElement("ProjectName")]
        public String nameofCSPROJ; // name.csproj

        [XmlElement("PathToCSProj")]
        public String pathToCSPROJ; // path to csproj.

        [XmlElement("SolutionName")]
        public String solutionName;

        [XmlElement("CurrentProjectState")]
        public ProjectState projectState;
    }

    /// <summary>
    /// This is instanciated for every object that has an asset tag applied and code added
    /// to it. It represents the connection between modeling editor objects and code so
    /// the connections can be re-established when loading a project.
    /// </summary>
    public struct AssetObject
    {
        [XmlElement("ModelEditorName")]
        public String objectName;

        [XmlElement("ModelEditorID")]
        public String objectID;

        [XmlElement("ClassName")]
        public String className;

        [XmlElement("NameSpace")]
        public String nameSpace;
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

        Boolean CreateNewClass(String pName, String pPath);
        Boolean CreateNewFile();

        Boolean BuildProject();

        EngineProject GetEngineProject();
        int GetEngineProjectState();
    }
}