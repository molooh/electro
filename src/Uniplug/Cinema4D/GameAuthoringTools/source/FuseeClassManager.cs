using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuseeAuthoringTools.source
{
    /// <summary>
    /// Used to take care of the partial classes distributed in the system. Quite important manager to take care of the code. Creates classes and partials etc.
    /// </summary>
    public class FuseeClassManager
    {
        public FuseeClassManager()
        {
        }

        public ToolState CreateNewClass(String cname)
        {
            // TODO: Create a new class for the ide and the coder.
            // 1) Create it as partial
            // 2) Create the file in the correct place. /project/src
            // 3) Add it to the *.csproj and or *.sln
            // 4) Save a ref to a file so we know what it's attached to.
            // create files from template

            //var classFile = new CSharpClass(cname, );
            //var classContent = classFile.
            //File.WriteAllText(classContent, mainFileContent);

            return ToolState.OK;
        }

        public ToolState UpdatePartialClass()
        {
            // TODO: Perhaps we need to sync the partial class sometimes?
            return ToolState.OK;
        }

    }
}
