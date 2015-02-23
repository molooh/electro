using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FuseeAuthoringTools.source
{
    /// <summary>
    /// Will be used to create various files if needed.
    /// </summary>
    public class FuseeFileManager
    {
        private FuseeAuthoringToolsC4D fat;
        private FuseeClassManager fcManager;
        private List<String> csprojfile = null;
        private String csProjPath = "";

        /// <summary>
        /// Constructor.
        /// </summary>
        public FuseeFileManager(FuseeAuthoringToolsC4D f)
        {
            fat = f;
            fcManager = new FuseeClassManager();
        }

        /// <summary>
        /// Creates a c# Class and inserts it to the project csproj file.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="pName"></param>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        public ToolState CreateCSharpClass(String className, String pName, String projectPath)
        {
            EngineProject ep = fat.EngineProject;
            ep.projectState = ProjectState.Dirty;
            fat.EngineProject = ep;

            csProjPath = ep.pathToCSPROJ;

            // TODO: Call class Manager here to create a real c# class file and THEN insert it to the project.
            fcManager.CreateNewClass(className);

            pName = pName + ".cs"; // TODO!!!! change ending dependend on filetype.
            if (!true)
                return ToolState.ERROR;

            if (LoadCSProj(pName, projectPath) == ToolState.ERROR)
                return ToolState.ERROR;

            if (InsertFileToProject(className) == ToolState.ERROR)
                return ToolState.ERROR;

            if (WriteCSProj() == ToolState.ERROR)
                return ToolState.ERROR;

            ep = fat.EngineProject;
            ep.projectState = ProjectState.Clean;
            fat.EngineProject = ep;

            return ToolState.OK;
        }

        /// <summary>
        /// Loads a csproj file.
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private ToolState LoadCSProj(String fName, String path)
        {
            if (!File.Exists(path))
                return ToolState.ERROR;

            File.Copy(path, fat.EngineProject.sysPath + fat.EngineProject.projPath + "/" + fName + ".orig", true);

            if (!File.Exists(fat.EngineProject.sysPath + fat.EngineProject.projPath + "/" + fName + ".orig"))
                return ToolState.ERROR;

            csprojfile = File.ReadAllLines(csProjPath).ToList();

            // is now loaded in the memory. Can handle it now.

            return ToolState.OK;
        }

        /// <summary>
        /// Writes changes to a csproj file.
        /// </summary>
        /// <returns></returns>
        private ToolState WriteCSProj()
        {
            File.WriteAllLines(csProjPath, csprojfile);

            return ToolState.OK;
        }

        /// <summary>
        /// Inserts a class to a csproj file.
        /// </summary>
        /// <param name="fName"></param>
        /// <returns></returns>
        private ToolState InsertFileToProject(String fName)
        {
            // TODO: Have to really insert it. This is handling only.

            // Search correct line etc.
            int line = csprojfile.IndexOf("    <Compile Include=\"Main.cs\" />");

            if (line == -1)
                return ToolState.ERROR;

            string content = "    <Compile Include=\"" + fName + "\"/>"; // <Compile Include="file.cs"/>
            csprojfile.Insert(++line, content);

            // Now call WriteCSProj().

            return ToolState.OK;
        }

    }
}
