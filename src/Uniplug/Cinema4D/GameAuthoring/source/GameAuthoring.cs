using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C4d;
using GameAuthoringTools;


namespace GameAuthoring
{
    // Plugin ID is final from maxon.
    [C4d.ObjectPlugin(
        1034424,
        Name = "GameAuthoringPlugin",
        IconFile = "YourLogoPattern.tif"
        )]

    class FuseeGameAuthoring : ObjectDataM
    {
        FuseeAuthoringTools tools;

        public FuseeGameAuthoring() : base(false)
        {
            tools = new FuseeAuthoringTools();
        }

        public override bool Execute(BaseDocument doc)
        {
            // Add some functionality here.
            Logger.Debug("GameAuthoring plugin is running.");

            Logger.Debug(tools.RetrieveInformation());

            return true;
        }
    }
}
