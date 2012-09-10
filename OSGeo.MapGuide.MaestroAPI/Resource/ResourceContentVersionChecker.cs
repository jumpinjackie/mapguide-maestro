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
using System.IO;
using System.Xml;
using OSGeo.MapGuide.MaestroAPI.Exceptions;

namespace OSGeo.MapGuide.MaestroAPI.Resource
{
    /// <summary>
    /// Inspects a resource content stream to determine the version of the resource content within
    /// 
    /// The stream to be inspected is copied and the inspection is made on the copy
    /// </summary>
    public sealed class ResourceContentVersionChecker : IDisposable
    {
        private XmlReader _reader;
        private Stream _stream;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stream">The resource content stream. Inspection is done on a copy of this stream</param>
        public ResourceContentVersionChecker(Stream stream)
        {
            var ms = new MemoryStream();
            Utility.CopyStream(stream, ms);
            ms.Position = 0L; //Rewind
            _stream = ms;
            _reader = new XmlTextReader(_stream);
        }

        /// <summary>
        /// Alternate constructor
        /// </summary>
        /// <param name="xmlContent"></param>
        public ResourceContentVersionChecker(string xmlContent)
        {
            _stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent));
        }

        private ResourceTypeDescriptor _rtd;

        /// <summary>
        /// Gets the resource content version
        /// </summary>
        /// <returns></returns>
        public ResourceTypeDescriptor GetVersion()
        {
            if (_rtd == null)
            {
                _rtd = GetVersionFromXmlStream(_stream);
            }
            return _rtd;
        }

        /// <summary>
        /// Gets the version from XML stream.
        /// </summary>
        /// <param name="ms">The ms.</param>
        /// <returns></returns>
        public static ResourceTypeDescriptor GetVersionFromXmlStream(Stream ms)
        {
            string version = "1.0.0"; //NOXLATE
            using (var xr = XmlReader.Create(ms))
            {
                xr.MoveToContent();
                if (!xr.HasAttributes)
                    throw new SerializationException();

                try
                {
                    //Parse version number from ResourceType-x.y.z.xsd
                    string xsd = xr.GetAttribute("xsi:noNamespaceSchemaLocation"); //NOXLATE
                    if (xsd == null)
                        return null;

                    int start = (xsd.LastIndexOf("-")); //NOXLATE
                    int end = xsd.IndexOf(".xsd") - 1; //NOXLATE
                    version = xsd.Substring(start + 1, xsd.Length - end);
                    string typeStr = xsd.Substring(0, start);

                    return new ResourceTypeDescriptor((ResourceTypes)Enum.Parse(typeof(ResourceTypes), typeStr), version);

                }
                finally
                {
                    xr.Close();
                }   
            }
        }

        /// <summary>
        /// Disposes this instance
        /// </summary>
        public void Dispose()
        {
            if (_stream != null)
                _stream.Dispose();

            if (_reader != null)
                _reader.Close();
        }
    }
}
