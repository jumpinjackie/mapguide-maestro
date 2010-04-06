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
            string connStr = string.Empty;
            if (string.IsNullOrEmpty(locale))
            {
                connStr = string.Format("{0}={1};{2}={3};{4}={5}",
                    HttpServerConnection.PARAM_URL, hosturl.ToString(),
                    HttpServerConnection.PARAM_SESSION, sessionid,
                    HttpServerConnection.PARAM_UNTESTED, allowUntestedVersion);
            }
            else
            {
                connStr = string.Format("{0}={1};{2}={3};{4}={5};{6}={7}",
                    HttpServerConnection.PARAM_URL, hosturl.ToString(),
                    HttpServerConnection.PARAM_SESSION, sessionid,
                    HttpServerConnection.PARAM_LOCALE, locale,
                    HttpServerConnection.PARAM_UNTESTED, allowUntestedVersion);
            }
            return ConnectionProviderRegistry.CreateConnection("Maestro.Http", connStr);
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
            string connStr = string.Empty;
            if (string.IsNullOrEmpty(locale))
            {
                connStr = string.Format("{0}={1};{2}={3};{4}={5};{6}={7}",
                    HttpServerConnection.PARAM_URL, hosturl.ToString(),
                    HttpServerConnection.PARAM_USERNAME, username,
                    HttpServerConnection.PARAM_PASSWORD, password,
                    HttpServerConnection.PARAM_UNTESTED, allowUntestedVersion);
            }
            else
            {
                connStr = string.Format("{0}={1};{2}={3};{4}={5};{6}={7};{8}={9}",
                    HttpServerConnection.PARAM_URL, hosturl.ToString(),
                    HttpServerConnection.PARAM_USERNAME, username,
                    HttpServerConnection.PARAM_PASSWORD, password,
                    HttpServerConnection.PARAM_LOCALE, locale,
                    HttpServerConnection.PARAM_UNTESTED, allowUntestedVersion);
            }
            return ConnectionProviderRegistry.CreateConnection("Maestro.Http", connStr);
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
            string connStr = string.Empty;
            if (string.IsNullOrEmpty(locale))
            {
                connStr = string.Format("{0}={1};{2}={3};{4}={5}",
                    LocalNativeConnection.PARAM_CONFIG, configFile,
                    LocalNativeConnection.PARAM_USERNAME, username,
                    LocalNativeConnection.PARAM_PASSWORD, password);
            }
            else
            {
                connStr = string.Format("{0}={1};{2}={3};{4}={5};{6}={7}",
                    LocalNativeConnection.PARAM_CONFIG, configFile,
                    LocalNativeConnection.PARAM_USERNAME, username,
                    LocalNativeConnection.PARAM_PASSWORD, password,
                    LocalNativeConnection.PARAM_LOCALE, locale);
            }
            return ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative", connStr);
        }

        /// <summary>
        /// Creates a local native implementation of <see cref="ServerConnectionI"/>
        /// </summary>
        /// <param name="sessionid"></param>
        /// <returns></returns>
        public static ServerConnectionI CreateLocalNativeConnection(string sessionid)
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative", LocalNativeConnection.PARAM_SESSION + "=" + sessionid);
        }
    }
}
