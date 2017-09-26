#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Inspects a resource content stream to determine the version of the resource content within
    ///
    /// The stream to be inspected is copied and the inspection is made on the copy
    /// </summary>
    public sealed class ResourceContentVersionChecker : IDisposable
    {
        private readonly Stream _stream;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stream">The resource content stream. Inspection is done on a copy of this stream</param>
        public ResourceContentVersionChecker(Stream stream)
        {
            var ms = MemoryStreamPool.GetStream("ResourceContentVersionChecker.ctor");
            Utils.CopyStream(stream, ms);
            ms.Position = 0L; //Rewind
            _stream = ms;
        }

        /// <summary>
        /// Alternate constructor
        /// </summary>
        /// <param name="xmlContent"></param>
        public ResourceContentVersionChecker(string xmlContent)
        {
            var ms = MemoryStreamPool.GetStream("ResourceContentVersionChecker.ctor", Encoding.UTF8.GetBytes(xmlContent)); ;
            ms.Position = 0L; //Rewind
            _stream = ms;
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
            XmlReader xr = null;
            try
            {
                xr = XmlReader.Create(ms);
                xr.MoveToContent();
                if (!xr.HasAttributes)
                    throw new SerializationException();

                //Parse version number from ResourceType-x.y.z.xsd
                string xsd = xr.GetAttribute("xsi:noNamespaceSchemaLocation"); //NOXLATE
                if (xsd == null)
                    return null;

                int start = (xsd.LastIndexOf("-")); //NOXLATE
                int end = xsd.IndexOf(".xsd") - 1; //NOXLATE
                if ((start + 1) + (xsd.Length - end) <= xsd.Length)
                {
                    version = xsd.Substring(start + 1, xsd.Length - end);
                    if (start > 0)
                    {
                        string typeStr = xsd.Substring(0, start);
                        return new ResourceTypeDescriptor(typeStr, version);
                    }
                }
            }
            finally
            {
                xr?.Close();
                xr?.Dispose();
                xr = null;
            }
            return null;
        }

        /// <summary>
        /// Disposes this instance
        /// </summary>
        public void Dispose()
        {
            if (_stream != null)
                _stream.Dispose();
        }
    }
}