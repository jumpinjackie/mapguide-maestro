using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maestro.Base;
using ICSharpCode.Core;

namespace HelloAddIn.Commands
{
    public class SelectedDocumentsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            //The workbench represents the main window
            //
            //There's only going to be ever one main window so you access it via the Instance static property
            var wb = Workbench.Instance;

            //The Site Explorer is attached to the main workbench, which can be accessed via the ActiveSiteExplorer property
            var siteExp = wb.ActiveSiteExplorer;

            //If this command was registered under the /Maestro/Shell/SiteExplorer/SelectedDocuments extension point you will
            //only ever get more than one selected item. However if this command was registered under other extension points, this may
            //not be the case, so as good practice you should always check your pre-conditions before continuing. In our case, 
            //we require more than one document be selected in the Site Explorer
            var items = siteExp.SelectedItems;
            if (items.Length > 1)
            {
                MessageService.ShowMessageFormatted("Selected documents", "Selected {0}", string.Join(Environment.NewLine, 
                    items.Where(x => !x.IsFolder)
                         .Select(x => x.ResourceId)
                         .ToArray()));
            }
        }
    }
}
