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
    /// <summary>
    /// Defines the command for querying the cache status of FDO connections
    /// </summary>
    /// <example>
    /// This example shows how invoke the <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.IGetFdoCacheInfo"/>
    /// command. Note that you should check if the connection supports this command through its capabilities
    /// <code>
    /// <![CDATA[
    /// IServerConnection conn;
    /// ...
    /// IGetFdoCacheInfo command = (IGetFdoCacheInfo)conn.CreateCommand(CommandType.GetFdoCacheInfo);
    /// FdoCacheInfo cacheInfo = command.Execute();
    /// ]]>
    /// </code>
    /// </example>
    public interface IGetFdoCacheInfo : ICommand
    {
        /// <summary>
        /// Executes this command and returns a <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.FdoCacheInfo"/> that
        /// represents this cache status information
        /// </summary>
        /// <returns></returns>
        FdoCacheInfo Execute();
    }

    /// <summary>
    /// Represents the cache status of FDO connections currently managed by the MapGuide Server
    /// </summary>
    [XmlRoot(ElementName = "FdoCacheInformation")] //NOXLATE
    public class FdoCacheInfo
    {
        private static XmlSerializer smSerializer;

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        public static XmlSerializer Serializer
        {
            get
            {
                if (null == smSerializer)
                    smSerializer = new XmlSerializer(typeof(FdoCacheInfo));

                return smSerializer;
            }
        }

        /// <summary>
        /// Gets the timestamp
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        [XmlElement]
        public DateTime TimeStamp { get; set; }


        /// <summary>
        /// Gets the configuration settings
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        [XmlElement(ElementName = "ConfigurationSettings")] //NOXLATE
        [DisplayName("Configuration Settings")] //NOXLATE
        public ConfigurationSettings Configuration { get; set; }


        /// <summary>
        /// Gets an array of cache status of connections for each provider
        /// </summary>
        [XmlElement(ElementName = "Provider")] //NOXLATE
        [DisplayName("Cached FDO Providers")] //NOXLATE
        public CachedProviderInfo[] Providers { get; set; }
    }

    /// <summary>
    /// Represents information about a cached FDO connection
    /// </summary>
    [Serializable]
    public class CachedFdoConnection
    {

        /// <summary>
        /// Gets the feature source
        /// </summary>
        [XmlElement]
        [DisplayName("Feature Source")] //NOXLATE
        public string Name { get; set; }

        /// <summary>
        /// Gets the state of this connection
        /// </summary>
        [XmlElement]
        public string ConnectionState { get; set; }

        /// <summary>
        /// Gets whether this connection is currently in use
        /// </summary>
        [XmlElement]
        public string InUse { get; set; }

        /// <summary>
        /// Gets the number of objects currently using this connection
        /// </summary>
        [XmlElement]
        public int UseCount { get; set; }

        /// <summary>
        /// Gets the long transaction
        /// </summary>
        [XmlElement]
        public string LongTransaction { get; set; }

        /// <summary>
        /// Gets the date this connection was last used
        /// </summary>
        [XmlElement]
        public DateTime LastUsed { get; set; }

        /// <summary>
        /// Gets whether this connection is valid
        /// </summary>
        [XmlElement]
        public string Valid { get; set; }
    }

    /// <summary>
    /// Represents the cache status of connections for a particular provider
    /// </summary>
    [Serializable]
    public class CachedProviderInfo
    {
        /// <summary>
        /// Gets the feature source
        /// </summary>
        [XmlElement(ElementName = "Name")] //NOXLATE
        public string FeatureSourceId { get; set; }

        /// <summary>
        /// Gets the maximum connection pool size 
        /// </summary>
        [XmlElement]
        public int MaximumDataConnectionPoolSize { get; set; }

        /// <summary>
        /// Gets the current connection pool size
        /// </summary>
        [XmlElement]
        public int CurrentDataConnectionPoolSize { get; set; }

        /// <summary>
        /// Gets the number of current connections
        /// </summary>
        [XmlElement]
        public int CurrentDataConnections { get; set; }

        /// <summary>
        /// Gets the thread model
        /// </summary>
        [XmlElement]
        public string ThreadModel { get; set; }

        /// <summary>
        /// Gets whether connections are cached
        /// </summary>
        [XmlElement]
        public string KeepDataConnectionsCached { get; set; }

        /// <summary>
        /// Gets an array of cached connections using this provider
        /// </summary>
        [XmlElement(ElementName = "CachedFdoConnection")] //NOXLATE
        [DisplayName("Cached Feature Sources")] //NOXLATE
        public CachedFdoConnection[] CachedFdoConnections { get; set; }
    }

    /// <summary>
    /// Represents the FDO cache configuration
    /// </summary>
    [Serializable]
    public class ConfigurationSettings
    {
        /// <summary>
        /// Gets whether connection pooling is enabled
        /// </summary>
        [XmlElement]
        public string DataConnectionPoolEnabled { get; set; }

        /// <summary>
        /// Gets the FDO providers which are excluded from connection pooling
        /// </summary>
        [XmlElement]
        public string DataConnectionPoolExcludedProviders { get; set; }

        /// <summary>
        /// Gets the size of the connection pool
        /// </summary>
        [XmlElement]
        public int DataConnectionPoolSize { get; set; }

        /// <summary>
        /// Gets a delimited list of custom connection pool sizes by provider
        /// </summary>
        [XmlElement]
        public string DataConnectionPoolSizeCustom { get; set; }

        /// <summary>
        /// Gets the connection timeout
        /// </summary>
        [XmlElement]
        public int DataConnectionTimeout { get; set; }
    }
}
