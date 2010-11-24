using System;
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// Helper class to create well known <see cref="ServerConnectionI"/> objects
    /// </summary>
    public sealed class ConnectionFactory
    {
        /// <summary>
        /// Creates a HTTP implementation of <see cref="ServerConnectionI"/>
        /// </summary>
        /// <param name="hosturl"></param>
        /// <param name="sessionid"></param>
        /// <param name="locale"></param>
        /// <param name="allowUntestedVersion"></param>
        /// <returns></returns>
        public static ServerConnectionI CreateHttpConnection(Uri hosturl, string sessionid, string locale, bool allowUntestedVersion)
        {
            System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
            builder[HttpServerConnection.PARAM_URL] = hosturl.ToString();
            builder[HttpServerConnection.PARAM_SESSION] = sessionid;
            builder[HttpServerConnection.PARAM_UNTESTED] = allowUntestedVersion;
            string connStr = string.Empty;
            if (!string.IsNullOrEmpty(locale))
            {
                builder[HttpServerConnection.PARAM_LOCALE] = locale;
            }
            return ConnectionProviderRegistry.CreateConnection("Maestro.Http", builder.ToString());
        }

        /// <summary>
        /// Creates a HTTP implementation of <see cref="ServerConnectionI"/>
        /// </summary>
        /// <param name="hosturl"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="locale"></param>
        /// <param name="allowUntestedVersion"></param>
        /// <returns></returns>
        public static ServerConnectionI CreateHttpConnection(Uri hosturl, string username, string password, string locale, bool allowUntestedVersion)
        {
            System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
            builder[HttpServerConnection.PARAM_URL] = hosturl.ToString();
            builder[HttpServerConnection.PARAM_USERNAME] = username;
            builder[HttpServerConnection.PARAM_PASSWORD] = password;
            builder[HttpServerConnection.PARAM_UNTESTED] = allowUntestedVersion;
            
            if (!string.IsNullOrEmpty(locale))
            {
                builder[HttpServerConnection.PARAM_LOCALE] = locale;
            }
            return ConnectionProviderRegistry.CreateConnection("Maestro.Http", builder.ToString());
        }

        /// <summary>
        /// Creates a local native implementation of <see cref="ServerConnectionI"/>
        /// </summary>
        /// <param name="configFile"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static ServerConnectionI CreateLocalNativeConnection(string configFile, string username, string password, string locale)
        {
            System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
            builder[LocalNativeConnection.PARAM_CONFIG] = configFile;
            builder[LocalNativeConnection.PARAM_USERNAME] = username;
            builder[LocalNativeConnection.PARAM_PASSWORD] = password;
            if (!string.IsNullOrEmpty(locale))
            {
                builder[LocalNativeConnection.PARAM_LOCALE] = locale;
            }
            return ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative", builder.ToString());
        }

        /// <summary>
        /// Creates a local native implementation of <see cref="ServerConnectionI"/>
        /// </summary>
        /// <param name="sessionid"></param>
        /// <returns></returns>
        public static ServerConnectionI CreateLocalNativeConnection(string sessionid)
        {
            System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
            builder[LocalNativeConnection.PARAM_SESSION] = sessionid;
            return ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative", builder.ToString());
        }
    }
}
