using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using fuProjectGen.Templates;

namespace fuProjectGen
{
    public class ProjectGenerator
    {
        private static List<string> _engineSolution; 

        private static bool ValidChars(string name)
        {
            if (name.Length == 0 || name.Length > 1023) return false;

            var validationRegex = new Regex(@"^(([_]+[a-zA-Z0-9]+\w*)|([a-zA-Z]+\w*))");
            return validationRegex.IsMatch(name);
        }

        private static bool DuplicateName(string name)
        {
            String pName = name;
            return (_engineSolution.FindAll(s => s.ToLower().Contains(name.ToLower() + ".csproj")).Count > 0) ||
                   Directory.Exists("projects/" + pName + "/" + name);
        }

        private static string GetValidGUID()
        {
            var guid = Guid.NewGuid().ToString("B").ToUpper();

            while (_engineSolution.FindAll(s => s.ToUpper().Contains(guid)).Count > 0)
                guid = Guid.NewGuid().ToString("B").ToUpper();

            return guid;
        }

        private static string GetProjectName()
        {
            var validName = false;
            var projectName = "";

            while (!validName)
            {
                Console.WriteLine("Please enter a valid and unique project name:");
                Console.Write("> ");

                projectName = Console.ReadLine();
                validName = ValidChars(projectName) && !DuplicateName(projectName);

                Console.WriteLine();
            }

            return projectName;
        }

        private static int GetLine(string search, int start = 0)
        {
            var line = _engineSolution.IndexOf(search, start);

            /*
            if (line == -1)
                Error("Error while parsing Engine.sln!");
            */

            return line;
        }

        private static void Error(string msg)
        {
            var action = "Press enter to exit.\n";

            try
            {
                if (File.Exists(@"Engine.orig"))
                {
                    File.Copy(@"Engine.orig", @"Engine.sln", true);
                    File.Delete(@"Engine.orig");

                    action = "Engine.sln restored. " + action;
                }
            }
            catch (Exception e)
            {
                msg += "\n\n" + e.Message;
            }

            //Environment.Exit(1);
        }

        public ProjectGenerator(String pName, String pPath)
        {            
            try
            {
                String pathToSln = pPath + "/Engine.sln";

                // pre-checks
                if (!File.Exists(pathToSln))
                    Error("Could not find" + pathToSln);

                if (!Directory.Exists(pPath + "/projects"))
                    Error("Could not find " + pathToSln + "/projects!");

                // backup of Engine.sln
                File.Copy(pathToSln, pPath + "/Engine.orig", true);

                if (!File.Exists(pPath + "/Engine.orig"))
                    Error("Couldn't backup Engine.sln!");

                // open Engine.sln
                _engineSolution = File.ReadAllLines(pathToSln).ToList();

                // get a valid project name
                var validName = (pName.Length == 0) || (!ValidChars(pName));
                var projectName = (validName) ? GetProjectName() : pName;

                if (!ValidChars(projectName))
                    Error("No valid project name!");              

                // get GUID for new project
                var guid = GetValidGUID();

                // check if project already exists
                if (Directory.Exists(pPath + "/projects/" + projectName))
                    return;                

                // create project directories
                Directory.CreateDirectory(pPath + "/projects/" + projectName);

                if (!Directory.Exists(pPath + "/projects/" + projectName))
                    Error("Could not create a directory for the project, because it already exists!");

                Directory.CreateDirectory(pPath + "/projects/" + projectName + @"/Properties");
                Directory.CreateDirectory(pPath + "/projects/" + projectName + @"/Assets");
                Directory.CreateDirectory(pPath + "/projects/" + projectName + @"/Assets/Models");
                Directory.CreateDirectory(pPath + "/projects/" + projectName + @"/Assets/Textures");
                Directory.CreateDirectory(pPath + "/projects/" + projectName + @"/Assets/Sound");
                Directory.CreateDirectory(pPath + "/projects/" + projectName + @"/Assets/Misc");
                Directory.CreateDirectory(pPath + "/projects/" + projectName + @"/Source");

                // create files from template
                var mainFile = new MainFile(projectName);
                var mainFileContent = mainFile.TransformText();

                var projFile = new ProjFile(projectName);
                var projFileContent = projFile.TransformText();

                var assemblyFile = new AssemblyFile(projectName);
                var assemblyFileContent = assemblyFile.TransformText();

                File.WriteAllText(pPath + "/projects/" + projectName + @"/Main.cs", mainFileContent);
                File.WriteAllText(pPath + "/projects/" + projectName + @"/" + projectName + @".csproj", projFileContent);
                File.WriteAllText(pPath + "/projects/" + projectName + @"/Properties/AssemblyInfo.cs", assemblyFileContent);

                if (!File.Exists(pPath + "/projects/" + projectName + @"/Main.cs") ||
                    !File.Exists(pPath + "/projects/" + projectName + @"/" + projectName + @".csproj") ||
                    !File.Exists(pPath + "/projects/" + projectName + @"/Properties/AssemblyInfo.cs"))
                    Error("Could not create necessary files for the project!");

                // add project to Engine.sln (Part1)
                var globalLine = GetLine("Global");

                var slnFilePt1 = new SolutionFilePt1(guid, projectName);
                var slnContentPt1 = slnFilePt1.TransformText();

                _engineSolution.Insert(globalLine, slnContentPt1);

                // add project to Engine.sln (Part2)
                var postSlnLine = GetLine("	GlobalSection(ProjectConfigurationPlatforms) = postSolution");
                var postSlnEndLine = GetLine("	EndGlobalSection", postSlnLine);

                var slnFilePt2 = new SolutionFilePt2(guid);
                var slnContentPt2 = slnFilePt2.TransformText();

                _engineSolution.Insert(postSlnEndLine, slnContentPt2);

                // add project to Engine.sln (Part3)
                var preSlnLine = GetLine("	GlobalSection(NestedProjects) = preSolution");
                var preSlnEndLine = GetLine("	EndGlobalSection", preSlnLine);

                var slnContentPt3 = "		" + guid + " = {2DC1CA2C-F4F6-4779-B000-597CB6A54A04}";
                _engineSolution.Insert(preSlnEndLine, slnContentPt3);

                // save new Engine.sln
                File.WriteAllLines(pathToSln, _engineSolution);

                return;
            }
            catch (Exception e)
            {
                Error("Creating new project failed!\n\n" + e.Message);
            }
        }
    }
}

