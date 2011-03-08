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
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace OSGeo.MapGuide.MaestroAPI.SchemaOverrides
{
    public abstract class RasterItem : IFdoSerializable
    {
        public ConfigurationDocument Parent { get; internal set; }

        public string Name { get; set; }

        private string _featureClassName;

        public string FeatureClassName
        {
            get { return _featureClassName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var tokens = value.Split(':');
                    if (tokens.Length != 2)
                        throw new ArgumentException("Not a qualified class name: " + value); //LOCALIZEME
                }
                _featureClassName = value;
            }
        }

        public string SchemaName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.FeatureClassName))
                {
                    var tokens = this.FeatureClassName.Split(':');
                    if (tokens.Length == 2)
                        return tokens[0];
                }
                return string.Empty;
            }
        }

        public string ClassName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.FeatureClassName))
                {
                    var tokens = this.FeatureClassName.Split(':');
                    if (tokens.Length == 2)
                        return tokens[1];
                }
                return string.Empty;
            }
        }

        public string Description { get; set; }

        public string SpatialContextName { get; set; }
        
        public abstract void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode);
        
        public abstract void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr);
    }
}
