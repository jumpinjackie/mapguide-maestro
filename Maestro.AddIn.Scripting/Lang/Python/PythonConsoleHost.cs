#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
// 
// Original code from SharpDevelop 3.2.1 licensed under the same terms (LGPL 2.1)
// Copyright 2002-2010 by
//
//  AlphaSierraPapa, Christoph Wille
//  Vordernberger Strasse 27/8
//  A-8700 Leoben
//  Austria
//
//  email: office@alphasierrapapa.com
//  court of jurisdiction: Landesgericht Leoben
//
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
using ICSharpCode.Core;
using IronPython.Hosting;
using IronPython.Runtime;
using Maestro.AddIn.Scripting.Services;
using Maestro.AddIn.Scripting.UI;
using Maestro.Editors.Common;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Hosting.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Maestro.AddIn.Scripting.Lang.Python
{
    /// <summary>
    /// Hosts the python console.
    /// </summary>
    internal class PythonConsoleHost : ConsoleHost, IDisposable
    {
        Thread thread;
        ITextEditor textEditor;
        PythonConsole pythonConsole;

        public PythonConsole Console { get { return pythonConsole; } }

        public PythonConsoleHost(ITextEditor textEditor)
        {
            this.textEditor = textEditor;
        }

        protected override Type Provider
        {
            get { return typeof(PythonContext); }
        }

        /// <summary>
        /// Runs the console host in its own thread.
        /// </summary>
        public void Run()
        {
            thread = new Thread(RunConsole);
            thread.Start();
        }

        public void Dispose()
        {
            if (pythonConsole != null)
            {
                pythonConsole.Dispose();
            }

            if (thread != null)
            {
                thread.Join();
            }
        }

        protected override CommandLine CreateCommandLine()
        {
            return new PythonCommandLine();
        }

        protected override OptionsParser CreateOptionsParser()
        {
            return new PythonOptionsParser();
        }

        protected override ScriptRuntimeSetup CreateRuntimeSetup()
        {
            ScriptRuntimeSetup srs = ScriptRuntimeSetup.ReadConfiguration();
            var paths = PropertyService.Get(ScriptingConfigProperties.IronPythonModulePath, ScriptingConfigProperties.DefaultIronPythonModulePath).Split(';');
            if (srs.LanguageSetups.Count > 0)
            {
                foreach (var langSetup in srs.LanguageSetups)
                {
                    if (langSetup.FileExtensions.Contains(".py")) //NOXLATE
                    {
                        langSetup.Options["SearchPaths"] = paths; //NOXLATE
                    }
                }
            }
            else
            {
                srs.Options["SearchPaths"] = paths; //NOXLATE
            }
            return srs;
        }

        protected override void ExecuteInternal()
        {
            var pc = HostingHelpers.GetLanguageContext(Engine) as PythonContext;
            pc.SetModuleState(typeof(ScriptEngine), Engine);
            base.ExecuteInternal();
        }

        /// <remarks>
        /// After the engine is created the standard output is replaced with our custom Stream class so we
        /// can redirect the stdout to the text editor window.
        /// This can be done in this method since the Runtime object will have been created before this method
        /// is called.
        /// </remarks>
        protected override IConsole CreateConsole(ScriptEngine engine, CommandLine commandLine, ConsoleOptions options)
        {
            SetOutput(new PythonOutputStream(textEditor));
            pythonConsole = new PythonConsole(textEditor, commandLine);
            return pythonConsole;
        }

        protected virtual void SetOutput(PythonOutputStream stream)
        {
            Runtime.IO.SetOutput(stream, Encoding.UTF8);
        }

        /// <summary>
        /// Runs the console.
        /// </summary>
        void RunConsole()
        {
            Run(new string[0]);
        }
    }
}
