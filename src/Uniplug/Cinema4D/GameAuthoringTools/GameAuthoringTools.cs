using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAuthoringTools
{
    /// <summary>
    /// The main part for the authoring tools. Registers modules and functionality.
    /// Can give out references to other tool classes.
    /// </summary>
    public class FuseeAuthoringTools
    {
        private String s;
        public FuseeAuthoringTools()
        {
            s = "This is a test from the fusee authoring tools set.";
        }

        public String RetrieveInformation()
        {
            return this.s;
        }

    }

    public class FuseeProjectManager
    {

    }

    public class FuseeClassManager
    {

    }

    public class FuseeFileManager
    {

    }

    public class FuseeBuildManager
    {

    }

}
