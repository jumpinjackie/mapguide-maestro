using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Shared.UI;
using Maestro.Base;

namespace HelloAddIn.Commands
{
    public class StartupCommand : AbstractCommand
    {
        public override void Run()
        {
            /*
             Startup commands are run as part of application startup, but before the Workbench (main window) has yet to be fully initialized
             as such, it is generally recommended to execute your startup logic after the workbench has been initialized as all the required
             application services will be ready by then
             */
            Workbench.WorkbenchInitialized += (sender, e) =>
            {
                MessageService.ShowMessage("If you see this message, it means the AddIn infrastructure has successfully loaded this add-in and is executing this add-in's startup command");
                //The ViewContentManager class is the class responsible for managing the display of view content.
                //
                //It has been registered under the /Maestro/ApplicationServices extension point, thus it is available
                //via the ServiceRegistry utility class
                //
                //When working with resource editor view content, avoid using the ViewContentManager to spawn editor
                //instances, use the OpenResourceManager service instead.
                var mgr = ServiceRegistry.GetService<ViewContentManager>();

                //Show the side panel to the right
                mgr.OpenContent<SidePanel>(ViewRegion.Right);
            };
        }
    }
}
