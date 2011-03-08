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
using System.Text;
using System.Xml;

namespace Maestro.Editors.FeatureSource.Providers.Odbc
{
    internal static class OdbcDriverMap
    {
        static Dictionary<string, Type> _drivers;

        static OdbcDriverMap()
        {
            _drivers = new Dictionary<string, Type>();
            if (System.IO.File.Exists("OdbcEditorMap.xml"))
            {
                var doc = new XmlDocument();
                doc.Load("OdbcEditorMap.xml");
                var list = doc.SelectNodes("//OdbcDriverMap/Driver");
                foreach (XmlNode node in list)
                {
                    try
                    {
                        string provider = node.Attributes["name"].Value.ToUpper();
                        string typeName = node.Attributes["type"].Value;

                        _drivers[provider] = Type.GetType(typeName);
                    }
                    catch { }
                }
            }
        }

        public static string[] EnumerateDrivers()
        {
            return new List<string>(_drivers.Keys).ToArray();
        }
        
        public static OdbcDriverInfo GetDriver(string provider)
        {
            OdbcDriverInfo driver = null;

            string name = provider.ToUpper();
            if (_drivers.ContainsKey(name))
            {
                driver = (OdbcDriverInfo)Activator.CreateInstance(_drivers[name]);
            }
            else
            {
                throw new OdbcDriverNotFoundException(provider);
            }

            return driver;
        }
    }

    [global::System.Serializable]
    public class OdbcDriverNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public OdbcDriverNotFoundException() { }
        public OdbcDriverNotFoundException(string message) : base(message) { }
        public OdbcDriverNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected OdbcDriverNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
