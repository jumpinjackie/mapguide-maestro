#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using OSGeo.MapGuide;
using System;
using System.IO;
using System.Reflection;

namespace MpuCalc
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MENTOR_DICTIONARY_PATH")))
                {
                    var currentDir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
                    var dictionaryDir = Path.Combine(currentDir, "Dictionaries");
                    if (Directory.Exists(dictionaryDir))
                        Environment.SetEnvironmentVariable("MENTOR_DICTIONARY_PATH", dictionaryDir, EnvironmentVariableTarget.Process);
                    else
                        Console.WriteLine("Error: Could not find CS-Map dictionary path");
                }

                MgCoordinateSystemFactory csFact = null;
                MgCoordinateSystem cs = null;
                try
                {
                    csFact = new MgCoordinateSystemFactory();
                    cs = csFact.Create(args[0]);
                    double mpu = cs.ConvertCoordinateSystemUnitsToMeters(1.0);
                    Console.WriteLine(mpu);
                    cs.Dispose();
                    csFact.Dispose();
                }
                catch (MgException ex)
                {
                    Console.WriteLine(ex.Message);
                    ex.Dispose();
                }
                finally
                {
                    if (cs != null)
                        cs.Dispose();
                    if (csFact != null)
                        csFact.Dispose();
                }
            }
            else
            {
                Console.WriteLine("Error: Insufficient arguments. Usage: MpuCalc.exe [Coord sys WKT]");
            }
        }
    }
}