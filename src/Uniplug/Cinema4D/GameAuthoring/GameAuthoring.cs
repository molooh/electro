using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C4d;


namespace GameAuthoring
{

    [CommandPlugin(
        1000007,
        Name = "GameAuthoringPlugin",
        HelpText = "This plugin can help to develop software with FUSEE in Cinema 4d R16.",
        IconFile = "YourLogoPattern.tif"
        )]

    class FuseeGameAuthoring : CommandData
    {
        public FuseeGameAuthoring() : base(false) { }

        public override bool Execute(BaseDocument doc)
        {
            // Add some functionality here.
            Logger.Debug("GameAuthoring plugin is running.");

            return true;
        }
    }
}
