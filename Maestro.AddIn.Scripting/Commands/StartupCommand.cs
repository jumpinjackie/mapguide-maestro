using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Core;
using Maestro.Base;
using Maestro.AddIn.Scripting.UI;
using Maestro.Shared.UI;
using Maestro.Base.Services;

namespace Maestro.AddIn.Scripting.Commands
{
    internal class StartupCommand : AbstractCommand
    {
        public override void Run()
        {
            ResourceService.RegisterNeutralImages(Properties.Resources.ResourceManager);
            ResourceService.RegisterNeutralStrings(Properties.Resources.ResourceManager);

            Workbench.WorkbenchInitialized += (sender, e) =>
            {
                var mgr = ServiceRegistry.GetService<ViewContentManager>();
                mgr.OpenContent<IronPythonRepl>(ViewRegion.Bottom);
            };
        }
    }
}
