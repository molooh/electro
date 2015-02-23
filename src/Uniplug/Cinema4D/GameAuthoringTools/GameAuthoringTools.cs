using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using fuProjectGen;

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
        WARNING = 2
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

        [XmlElement("CurrentProjectState")]
        public ProjectState projectState;
    }

    interface IFuseeAuthoringTools
    {
        ToolState CreateProject();
        ToolState SaveProject();
        ToolState OpenProject();
        ToolState UpdateProject();

        ToolState CreateNewClass();
        ToolState CreateNewFile();

        ToolState BuildProject();

        EngineProject GetEngineProject();
        ProjectState GetEngineProjectState();
    }
}
