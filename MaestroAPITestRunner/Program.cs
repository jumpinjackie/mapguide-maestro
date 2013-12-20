#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using MaestroAPITests;

namespace MaestroAPITestRunner
{
    /// <summary>
    /// This is a simple NUnit test runner, to work around the fact that you can't launch an external program
    /// in a debug session (in our case, nunit-console.exe) in Visual Studio Express.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //NOTE: Maestro.Local and Maestro.LocalNative unit tests cannot be run at the
            //same time. They are mutually exclusive. You will need to remove the postbuild
            //and change the TestControl settings if you want to activate the Maestro.LocalNative tests
            //
            //For reference, this is the postbuild required for Maestro.Local tests

            /*
IF EXIST "$(ProjectDir)Setup" XCOPY /Y /I "$(ProjectDir)Setup\*.*" "$(ProjectDir)$(OutDir)"
IF NOT EXIST "$(OutDir)Dictionaries" XCOPY /S /Y /I "$(SolutionDir)..\Maestro.AddIn.Local\Dictionaries\*.*" "$(ProjectDir)$(OutDir)Dictionaries"
IF NOT EXIST "$(OutDir)FDO" XCOPY /S /Y /I "$(SolutionDir)..\Maestro.AddIn.Local\FDO\*.*" "$(ProjectDir)$(OutDir)FDO"
IF NOT EXIST "$(OutDir)Resources" XCOPY /S /Y /I "$(SolutionDir)..\Maestro.AddIn.Local\Resources\*.res" "$(ProjectDir)$(OutDir)Resources"
COPY /Y "$(SolutionDir)..\Maestro.AddIn.Local\*.dll" "$(ProjectDir)$(OutDir)"
             */
            /*
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";C:\\Program Files\\OSGeo\\MapGuide\\Web\\www\\mapviewernet\\bin");
            Environment.SetEnvironmentVariable("MENTOR_DICTIONARY_PATH", "C:\\Program Files\\OSGeo\\MapGuide\\CS-Map\\Dictionaries");

            Assembly.LoadFrom("MGOS22\\OSGeo.MapGuide.Foundation.dll");
            Assembly.LoadFrom("MGOS22\\OSGeo.MapGuide.Geometry.dll");
            Assembly.LoadFrom("MGOS22\\OSGeo.MapGuide.PlatformBase.dll");
            Assembly.LoadFrom("MGOS22\\OSGeo.MapGuide.MapGuideCommon.dll");
            Assembly.LoadFrom("MGOS22\\OSGeo.MapGuide.Web.dll");
            */
            var runnerArgs = new string[1];
            if (args.Length == 1)
                runnerArgs[0] = args[0];
            else
                runnerArgs[0] = "MaestroAPITests.dll";
            
            NUnit.ConsoleRunner.Runner.Main(runnerArgs);
        }
    }
}
