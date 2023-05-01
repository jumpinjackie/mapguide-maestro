﻿using ICSharpCode.Core;
using Maestro.AddIn.Scripting.Services;
using Maestro.Base;
using Maestro.Editors.Common;
using Maestro.Scripting.Core.Lang.Python;
using Maestro.Shared.UI;
using Microsoft.Scripting.Hosting.Shell;
using System;
using System.Windows.Forms;

namespace Maestro.AddIn.Scripting.UI
{
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
                host = new PythonConsoleHost(textEditor, new HostApplication());
                host.Run();
            }
        }

        public void Shutdown()
        {
            Console.WriteLine("Terminate python host");
            host?.Terminate(0);
        }

        protected override void OnLoad(EventArgs e)
        {
            textEditor.SetParent(this.ParentForm);
            Workbench.Instance.ApplyThemeTo(toolStrip1);
            base.OnLoad(e);
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            Console.WriteLine("Dispose python host");
            host?.Dispose();
            host = null;

            textEditor?.Dispose();
            textEditor = null;
        }

        public override ViewRegion DefaultRegion => ViewRegion.Bottom | ViewRegion.Floating;

        private static void NewPrompt(IConsole con) => con.Write(">>> ", Style.Prompt); //HACK: Should be a way to get this from IronPython

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