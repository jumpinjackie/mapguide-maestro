#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maestro.Shared.UI;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using ICSharpCode.Core;

namespace Maestro.AddIn.Scripting.Services
{
    public class ScriptingService : ServiceBase
    {
        private ScriptEngine _pyEngine;
        private ScriptScope _pyGlobalScope;

        public override void Initialize()
        {
            base.Initialize();
            _pyEngine = CreateDefaultEngine();
            _pyGlobalScope = _pyEngine.CreateScope();
            InitializeScope(_pyGlobalScope);
        }

        private void InitializeScope(ScriptScope scope)
        {
            scope.SetVariable("app", new HostApplication());
        }

        private static ScriptEngine CreateDefaultEngine()
        {
            return Python.CreateEngine();
        }

        /// <summary>
        /// Evaluates the given expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public dynamic Evaluate(string expression)
        {
            return _pyEngine.Execute(expression, _pyGlobalScope);
        }

        /// <summary>
        /// Compiles and executes the script from the given file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public dynamic CompileAndExecute(string file)
        {
            var source = _pyEngine.CreateScriptSourceFromFile(file);
            var compiled = source.Compile();
            return compiled.Execute(_pyGlobalScope);
        }
    }
}
