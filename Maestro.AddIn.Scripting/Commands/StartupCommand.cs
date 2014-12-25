using ICSharpCode.Core;
using Maestro.AddIn.Scripting.UI;
using Maestro.Base;
using Maestro.Base.Services;
using Maestro.Shared.UI;
using Props = ICSharpCode.Core.PropertyService;

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

        private void OnWorkbenchClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            if (_repl != null)
                _repl.Shutdown();
        }
    }
}