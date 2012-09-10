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
            if (System.IO.File.Exists("OdbcEditorMap.xml")) //NOXLATE
            {
                var doc = new XmlDocument();
                doc.Load("OdbcEditorMap.xml"); //NOXLATE
                var list = doc.SelectNodes("//OdbcDriverMap/Driver"); //NOXLATE
                foreach (XmlNode node in list)
                {
                    try
                    {
                        string provider = node.Attributes["name"].Value.ToUpper(); //NOXLATE
                        string typeName = node.Attributes["type"].Value; //NOXLATE

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

    /// <summary>
    /// Thrown when an ODBC driver is not found
    /// </summary>
    [global::System.Serializable]
    public class OdbcDriverNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDriverNotFoundException"/> class.
        /// </summary>
        public OdbcDriverNotFoundException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDriverNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public OdbcDriverNotFoundException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDriverNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public OdbcDriverNotFoundException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDriverNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        ///   </exception>
        ///   
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        ///   </exception>
        protected OdbcDriverNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
