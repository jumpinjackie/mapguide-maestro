using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Maestro.Shared.UI;
using ICSharpCode.TextEditor.Document;
using System.IO;
using Maestro.Editors.Common;
using Microsoft.Scripting.Hosting.Shell;

namespace Maestro.AddIn.Scripting.UI
{
    using Lang.Python;
    using ICSharpCode.Core;

    internal partial class IronPythonRepl : SingletonViewContent
    {
        private ITextEditor textEditor;
        private PythonConsoleHost host;

        public IronPythonRepl()
        {
            InitializeComponent();
            textEditorControl.CreateControl();

            textEditorControl.ShowLineNumbers = false;
            textEditorControl.ShowVRuler = false;
            textEditorControl.ShowHRuler = false;

            //Our ICSharpCode.TextEditor has been modified to include the Python syntax file
            //allowing us to do this
            textEditorControl.SetHighlighting("Python"); //NOXLATE

            this.Title = this.Description = Strings.Title_IronPython_Console;
            this.Disposed += OnDisposed;

            textEditor = TextEditorFactory.CreateEditor(textEditorControl);

            if (PropertyService.Get(ScriptingConfigProperties.ShowIronPythonConsole, ScriptingConfigProperties.DefaultShowIronPythonConsole))
            {
                Console.WriteLine("Run python host");
                host = new PythonConsoleHost(textEditor);
                host.Run();
            }
        }

        public void Shutdown()
        {
            if (host != null)
            {
                Console.WriteLine("Terminate python host");
                host.Terminate(0);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            textEditor.SetParent(this.ParentForm);
            base.OnLoad(e);
        }

        void OnDisposed(object sender, EventArgs e)
        {
            if (host != null)
            {
                Console.WriteLine("Dispose python host");
                host.Dispose();
            }
        }

        public override ViewRegion DefaultRegion
        {
            get
            {
                return ViewRegion.Bottom | ViewRegion.Floating;
            }
        }

        private static void NewPrompt(IConsole con)
        {
            //HACK: Should be a way to get this from IronPython
            con.Write(">>> ", Microsoft.Scripting.Hosting.Shell.Style.Prompt);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textEditorControl.Text = string.Empty;
            if (host != null)
            {
                var con = host.Console;
                var cmdline = con.CommandLine;
                NewPrompt(con);
                textEditorControl.Refresh();
            }
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            if (host == null)
                return;

            using (var picker = DialogFactory.OpenFile())
            {
                picker.Filter = "*.py|*.py"; //NOXLATE
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    var con = host.Console;
                    var cmdline = con.CommandLine;
                    con.WriteLine();
                    cmdline.ScriptScope.Engine.ExecuteFile(picker.FileName, cmdline.ScriptScope);
                    NewPrompt(con);
                }
            }
        }
    }
}
