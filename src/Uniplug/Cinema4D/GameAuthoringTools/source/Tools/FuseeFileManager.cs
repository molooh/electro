using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using FuseeAuthoringTools.Templates;

namespace FuseeAuthoringTools.tools
{
    /// <summary>
    /// Will be used to create various files if needed.
    /// </summary>
    public class FuseeFileManager
    {
        /// <summary>
        /// Used to take care of the partial classes distributed in the system. Quite important manager to take care of the code. Creates classes and partials etc.
        /// </summary>
        internal static class FuseeClassHelper
        {
            public static ToolState CreateNewClass(String cname)
            {
                // TODO: Create a new class for the ide and the coder.
                // 1) Create it as partial
                // 2) Create the file in the correct place. /project/src

                var classFile = new StandardClass(cname);
                //var mainFileContent = classFile

                return ToolState.OK;
            }

            public static ToolState UpdatePartialClass()
            {
                // TODO: Perhaps we need to sync the partial class sometimes?
                return ToolState.OK;
            }

        }

        private readonly FuseeProjectManager _projectManager;
        private List<String> csprojfile = null;
        private String csProjPath = "";

        /// <summary>
        /// Constructor.
        /// </summary>
        public FuseeFileManager(FuseeProjectManager fpm)
        {
            _projectManager = fpm;
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
            _projectManager.SetProjectDirty();

            csProjPath = _projectManager.FuseeEngineProject.pathToCSPROJ;

            // TODO: Call class Manager here to create a real c# class file and THEN insert it to the project.
            FuseeClassHelper.CreateNewClass(className);

            pName = pName + ".cs"; // Add the ending here

            if (LoadCSProj(pName, projectPath) == ToolState.ERROR)
                return ToolState.ERROR;

            if (InsertFileToProject(className) == ToolState.ERROR)
                return ToolState.ERROR;

            if (WriteCSProj() == ToolState.ERROR)
                return ToolState.ERROR;

            _projectManager.SetProjectClean();

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

            File.Copy(path, _projectManager.FuseeEngineProject.sysPath + _projectManager.FuseeEngineProject.projPath + "/" + fName + ".orig", true);

            if (!File.Exists(_projectManager.FuseeEngineProject.sysPath + _projectManager.FuseeEngineProject.projPath + "/" + fName + ".orig"))
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
