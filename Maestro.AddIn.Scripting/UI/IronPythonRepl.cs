using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Maestro.Shared.UI;

namespace Maestro.AddIn.Scripting.UI
{
    using Lang.Python;

    public partial class IronPythonRepl : SingletonViewContent
    {
        private TextEditor textEditor;
        private PythonConsoleHost host;

        public IronPythonRepl()
        {
            InitializeComponent();
            textEditorControl.CreateControl();

            //TODO: Setup python syntax highlighting

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
                return ViewRegion.Bottom;
            }
        }
    }
}
