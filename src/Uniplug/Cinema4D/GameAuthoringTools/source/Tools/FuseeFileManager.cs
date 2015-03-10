using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            public static ToolState CreateNewCSharpClass(String cName, String pPath, String pName)
            {
                // Create an instance from the template class
                var sc = new SimpleClass(cName, pName);
                var scContent = sc.TransformText();

                // Write the file to disk from generated template code
                File.WriteAllText( pPath + @"/Source/" + cName + ".cs", scContent);

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
        /// <returns></returns>
        public ToolState CreateCSharpClass(String className, String pName)
        {
            _projectManager.SetProjectDirty();

            csProjPath = _projectManager.FuseeEngineProject.pathToCSPROJ;

            // TODO: Call class Manager here to create a real c# class file and THEN insert it to the project.
            FuseeClassHelper.CreateNewCSharpClass(className, _projectManager.FuseeEngineProject.sysPath + _projectManager.FuseeEngineProject.projPath, pName);

            className += ".cs"; // Add the ending here

            if (LoadCSProj(pName, _projectManager.FuseeEngineProject.pathToCSPROJ) == ToolState.ERROR)
                return ToolState.ERROR;

            if (InsertClassToProject(className) == ToolState.ERROR)
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
        private ToolState LoadCSProj(String pName, String path)
        {
            if (!File.Exists(path))
                return ToolState.ERROR;

            File.Copy(path, _projectManager.FuseeEngineProject.sysPath + _projectManager.FuseeEngineProject.projPath + "/" + pName + ".orig", true);

            if (!File.Exists(_projectManager.FuseeEngineProject.sysPath + _projectManager.FuseeEngineProject.projPath + "/" + pName + ".orig"))
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
        private ToolState InsertClassToProject(String fName)
        {
            // Search correct line etc.
            int line = csprojfile.IndexOf("    <Compile Include=\"Main.cs\" />");

            if (line == -1)
                return ToolState.ERROR;

            string content = "    <Compile Include=\"Source\\" + fName + "\"/>"; // <Compile Include="file.cs"/>
            csprojfile.Insert(++line, content);

            // Now call WriteCSProj().

            return ToolState.OK;
        }

    }
}
