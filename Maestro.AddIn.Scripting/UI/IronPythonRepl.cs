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

namespace Maestro.AddIn.Scripting.UI
{
    using Lang.Python;

    internal partial class IronPythonRepl : SingletonViewContent
    {
        private TextEditor textEditor;
        private PythonConsoleHost host;

        public IronPythonRepl()
        {
            InitializeComponent();
            textEditorControl.CreateControl();

            textEditorControl.ShowVRuler = false;
            textEditorControl.ShowHRuler = false;

            //Our ICSharpCode.TextEditor has been modified to include the Python syntax file
            //allowing us to do this
            textEditorControl.SetHighlighting("Python"); //NOXLATE

            this.Title = this.Description = Strings.Title_IronPython_Console;
            this.Disposed += OnDisposed;

            textEditor = new TextEditor(textEditorControl);
            host = new PythonConsoleHost(textEditor);
            host.Run();	
        }

        void OnDisposed(object sender, EventArgs e)
        {
            host.Dispose();
        }

        public override ViewRegion DefaultRegion
        {
            get
            {
                return ViewRegion.Bottom | ViewRegion.Floating;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textEditorControl.Text = string.Empty;
            var con = host.Console;
            var cmdline = con.CommandLine;
            //HACK: Should be a way to get this from IronPython
            con.Write(">>> ", Microsoft.Scripting.Hosting.Shell.Style.Prompt);
            textEditorControl.Refresh();
        }
    }
}
