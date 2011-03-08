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
using System.Reflection;
using System.Collections.Specialized;
using System.Data.Common;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// Represents an entry in the Connection Provider Registry
    /// </summary>
    public class ConnectionProviderEntry
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is multi platform.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is multi platform; otherwise, <c>false</c>.
        /// </value>
        public bool IsMultiPlatform { get; private set; }

        internal ConnectionProviderEntry(string name, string desc, bool multiPlatform)
        {
            this.Name = name;
            this.Description = desc;
            this.IsMultiPlatform = multiPlatform;
        }
    }

    /// <summary>
    /// The entry point of the Maestro API. The <see cref="ConnectionProviderRegistry"/> is used to create <see cref="IServerConnection"/>
    /// objects. <see cref="IServerConnection"/> is the root object of the Maestro API, and is where most of the functionality provided
    /// by this API is accessed from.
    /// 
    /// The <see cref="ConnectionProviderRegistry"/> supports dynamic creation of <see cref="IServerConnection"/> objects given a provider name
    /// and a connection string, which specifies the initialization parameters of the connection. The connection providers are defined in an XML
    /// file called ConnectionProviders.xml which contains all the registered providers. Each provider has the following properties:
    /// 
    /// 1. The name of the provider
    /// 2. The assembly containing the <see cref="IServerConnection"/> implementation
    /// 3. The name of this <see cref="IServerConnection"/> implementation.
    /// 
    /// The <see cref="IServerConnection"/> implementation is expected to have a non-public constructor which takes a single parameter, 
    /// a <see cref="System.Collections.Specialized.NameValueCollection"/> containing the initialization parameters parsed from the given connection
    /// string.
    /// </summary>
    /// <example>
    /// This example shows how to create a http-based MapGuide Server connection to the server's mapagent interface.
    /// <code>
    /// using OSGeo.MapGuide.MaestroAPI;
    /// 
    /// ...
    /// 
    /// IServerConnection conn = ConnectionProviderRegistry.CreateConnection("Maestro.Http",
    ///     "Url", "http://localhost/mapguide/mapagent/mapagent.fcgi",
    ///     "Username", "Administrator",
    ///     "Password", "admin");
    /// 
    /// </code>
    /// </example>
    /// <example>
    /// This example shows how to create a TCP/IP connection that wraps the official MapGuide API
    /// <code>
    /// using OSGeo.MapGuide.MaestroAPI;
    /// 
    /// ...
    /// 
    /// IServerConnection conn = ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative",
    ///     "ConfigFile", "webconfig.ini",
    ///     "Username", "Administrator",
    ///     "Password", "admin");
    /// </code>
    /// </example>
    public sealed class ConnectionProviderRegistry
    {
        const string PROVIDER_CONFIG = "ConnectionProviders.xml";

        static Dictionary<string, Type> _ctors;
        static List<ConnectionProviderEntry> _providers;

        static string _dllRoot;

        static ConnectionProviderRegistry()
        {
            _ctors = new Dictionary<string, Type>();
            _providers = new List<ConnectionProviderEntry>();

            var dir = System.IO.Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            var path = System.IO.Path.Combine(dir, PROVIDER_CONFIG);

            _dllRoot = System.IO.Path.GetDirectoryName(path);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList providers = doc.SelectNodes("//ConnectionProviderRegistry/ConnectionProvider");
            foreach (XmlNode prov in providers)
            {
                string name = prov["Name"].InnerText.ToUpper();
                string desc = prov["Description"].InnerText;
                string dll = prov["Assembly"].InnerText;
                string type = prov["Type"].InnerText;

                if (!System.IO.Path.IsPathRooted(dll))
                    dll = System.IO.Path.Combine(_dllRoot, dll);

                try
                {
                    Assembly asm = Assembly.LoadFrom(dll);
                    MaestroApiProviderAttribute[] attr = asm.GetCustomAttributes(typeof(MaestroApiProviderAttribute), true) as MaestroApiProviderAttribute[];
                    if (attr != null && attr.Length == 1)
                    {
                        name = attr[0].Name.ToUpper();
                        desc = attr[0].Description;
                        _ctors[name] = attr[0].ImplType;
                        _providers.Add(new ConnectionProviderEntry(name, desc, attr[0].IsMultiPlatform));    
                    }

                    
                }
                catch
                {

                }
            }
        }

        internal static NameValueCollection ParseConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = connectionString;

            NameValueCollection values = new NameValueCollection();

            foreach (string key in builder.Keys)
            {
                values.Add(key, builder[key].ToString());
            }
            return values;
        }

        /// <summary>
        /// Gets a list of registered provider names. The returned names are in upper-case.
        /// </summary>
        /// <returns></returns>
        public static ConnectionProviderEntry[] GetProviders()
        {
            return _providers.ToArray();
        }

        /// <summary>
        /// Creates an initialized <see cref="IServerConnection"/> object given the provider name and connection string
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServerConnection CreateConnection(string provider, string connectionString)
        {
            string name = provider.ToUpper();
            if (!_ctors.ContainsKey(name))
                throw new ArgumentException("Provider not registered: " + provider);

            ConnectionProviderEntry prv = FindProvider(provider);
            if (prv != null && !prv.IsMultiPlatform && Platform.IsRunningOnMono)
                throw new NotSupportedException("The specified provider is not usable in your operating system");

            Type t = _ctors[name];

            NameValueCollection initParams = ParseConnectionString(connectionString);

            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance;
            IServerConnection conn = (IServerConnection)t.InvokeMember(null, flags, null, null, new object[] { initParams });
            return conn;
        }

        /// <summary>
        /// Creates an initialized <see cref="IServerConnection"/> object given the provider name and the initalization parameters
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="connInitParams"></param>
        /// <returns></returns>
        public static IServerConnection CreateConnection(string provider, NameValueCollection connInitParams)
        {
            string name = provider.ToUpper();
            if (!_ctors.ContainsKey(name))
                throw new ArgumentException("Provider not registered: " + provider);

            ConnectionProviderEntry prv = FindProvider(provider);
            if (prv != null && !prv.IsMultiPlatform && Platform.IsRunningOnMono)
                throw new NotSupportedException("The specified provider is not usable in your operating system");

            Type t = _ctors[name];
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance;
            IServerConnection conn = (IServerConnection)t.InvokeMember(null, flags, null, null, new object[] { connInitParams });
            return conn;
        }

        /// <summary>
        /// Creates an initialized <see cref="IServerConnection"/> object given the provider name and the initalization parameters.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="initParameters">A variable list of initialization parameters. They must be specified in the form: [Param1], [Value1], [Param2], [Value2], etc</param>
        /// <returns></returns>
        public static IServerConnection CreateConnection(string provider, params string[] initParameters)
        {
            var initP = new NameValueCollection();

            for (int i = 0; i < initParameters.Length; i += 2)
            {
                string name = null;
                string value = null;

                if (i < initParameters.Length - 1)
                    name = initParameters[i];
                if (i + 1 <= initParameters.Length - 1)
                    value = initParameters[i + 1];

                if (name != null)
                    initP[name] = value ?? string.Empty;
            }

            return CreateConnection(provider, initP);
        }

        private static ConnectionProviderEntry FindProvider(string provider)
        {
            foreach (var prv in _providers)
            {
                if (prv.Name == provider)
                    return prv;
            }

            return null;
        }
    }
}
