using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Logging;
using Microsoft.CSharp;

namespace FuseeAuthoringTools.tools
{
    /// <summary>
    /// Will be used to call the compiler etc. from IDE software. Mostly 3D modeling software - to run a project.
    /// </summary>
    public static class FuseeBuildManager
    {
        /// <summary>
        /// This builds a whole solution.
        /// </summary>
        /// <returns></returns>
        public static ToolState BuildSolution(EngineProject ep)
        {
            String slnFile = ep.sysPath + "/" + ep.solutionName + ".sln";
            ProjectCollection pc = new ProjectCollection();
            Dictionary<string, string> GlobalProperty = new Dictionary<string, string>();
            
            // Some properties for the build.
            GlobalProperty.Add("Configuration", "Debug");
            GlobalProperty.Add("Platform", "x86");
 
            BuildRequestData BuidlRequest = new BuildRequestData(slnFile, GlobalProperty, null, new string[] { "Build" }, null);
 
            // Do the actual build.
            BuildResult buildResult = BuildManager.DefaultBuildManager.Build(new BuildParameters(pc), BuidlRequest);

            return ToolState.OK;
        }

        /// <summary>
        /// This builds a single csproj.
        /// </summary>
        /// <returns></returns>
        public static ToolState BuildProject()
        {
            
            return ToolState.OK;
        }
    }
}
