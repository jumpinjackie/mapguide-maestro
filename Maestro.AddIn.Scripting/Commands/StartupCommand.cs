using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Core;
using Maestro.Base;
using Maestro.AddIn.Scripting.UI;
using Maestro.Shared.UI;
using Maestro.Base.Services;
using Props = ICSharpCode.Core.PropertyService;
using Maestro.Base.UI.Preferences;
using ICSharpCode.TextEditor.Document;

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
                var wb = Workbench.Instance;
                wb.FormClosed += OnWorkbenchClosed;
                var mgr = ServiceRegistry.GetService<ViewContentManager>();
                if (Props.Get(ScriptingConfigProperties.ShowIronPythonConsole, ScriptingConfigProperties.DefaultShowIronPythonConsole))
                    _repl = mgr.OpenContent<IronPythonRepl>(ViewRegion.Bottom);
            };
        }

        private IronPythonRepl _repl;

        void OnWorkbenchClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            if (_repl != null)
                _repl.Shutdown();
        }
    }
}
