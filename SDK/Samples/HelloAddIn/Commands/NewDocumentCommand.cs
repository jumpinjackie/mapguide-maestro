using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Core;
using Maestro.Base.Services;

namespace HelloAddIn.Commands
{
    public class NewDocumentCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            //The ViewContentManager class is the class responsible for managing the display of view content.
            //
            //It has been registered under the /Maestro/ApplicationServices extension point, thus it is available
            //via the ServiceRegistry utility class
            //
            //When working with resource editor view content, avoid using the ViewContentManager to spawn editor
            //instances, use the OpenResourceManager service instead.
            var mgr = ServiceRegistry.GetService<ViewContentManager>();

            mgr.OpenContent(Maestro.Shared.UI.ViewRegion.Document, () => { 
                return new DocumentView(); 
            });
        }
    }
}
