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
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace OSGeo.MapGuide.MaestroAPI.Commands
{
    public interface IGetFdoCacheInfo : ICommand
    {
        FdoCacheInfo Execute();
    }

    [XmlRoot(ElementName = "FdoCacheInformation")]
    public class FdoCacheInfo
    {
        private static XmlSerializer smSerializer;

        public static XmlSerializer Serializer
        {
            get
            {
                if (null == smSerializer)
                    smSerializer = new XmlSerializer(typeof(FdoCacheInfo));

                return smSerializer;
            }
        }

        [XmlElement]
        public DateTime TimeStamp { get; set; }

        
        [XmlElement(ElementName = "ConfigurationSettings")]
        [DisplayName("Configuration Settings")]
        public ConfigurationSettings Configuration { get; set; }


        [XmlElement(ElementName = "Provider")]
        [DisplayName("Cached FDO Providers")]
        public CachedProviderInfo[] Providers { get; set; }
    }

    [Serializable]
    public class CachedFdoConnection
    {
        
        [XmlElement]
        [DisplayName("Feature Source")]
        public string Name { get; set; }

        
        [XmlElement]
        public string ConnectionState { get; set; }

        
        [XmlElement]
        public string InUse { get; set; }

        
        [XmlElement]
        public int UseCount { get; set; }

        
        [XmlElement]
        public string LongTransaction { get; set; }

        
        [XmlElement]
        public DateTime LastUsed { get; set; }

        
        [XmlElement]
        public string Valid { get; set; }
    }

    [Serializable]
    public class CachedProviderInfo
    {
        
        [XmlElement(ElementName = "Name")]
        public string FeatureSourceId { get; set; }

        
        [XmlElement]
        public int MaximumDataConnectionPoolSize { get; set; }

        
        [XmlElement]
        public int CurrentDataConnectionPoolSize { get; set; }

        
        [XmlElement]
        public int CurrentDataConnections { get; set; }

        
        [XmlElement]
        public string ThreadModel { get; set; }

        
        [XmlElement]
        public string KeepDataConnectionsCached { get; set; }

        [XmlElement(ElementName = "CachedFdoConnection")]
        [DisplayName("Cached Feature Sources")]
        public CachedFdoConnection[] CachedFdoConnections { get; set; }
    }

    [Serializable]
    public class ConfigurationSettings
    {
        
        [XmlElement]
        public string DataConnectionPoolEnabled { get; set; }

        
        [XmlElement]
        public string DataConnectionPoolExcludedProviders { get; set; }

        
        [XmlElement]
        public int DataConnectionPoolSize { get; set; }

        
        [XmlElement]
        public string DataConnectionPoolSizeCustom { get; set; }

        
        [XmlElement]
        public int DataConnectionTimeout { get; set; }
    }
}
