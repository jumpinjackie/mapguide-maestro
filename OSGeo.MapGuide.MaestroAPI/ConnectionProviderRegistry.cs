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
        /// <summary>
        /// Gets whether this provider has global connection state. This effectively indicates that subsequent connections after the first one
        /// created for this provider will re-use the same connection information and may/will disregard that values of the connection parameters
        /// you pass in
        /// </summary>
        public bool HasGlobalState { get; private set; }
        /// <summary>
        /// Gets the path of the assembly containing the provider implementation
        /// </summary>
        public string AssemblyPath { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionProviderEntry"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="desc">The desc.</param>
        /// <param name="asmPath">The assembly path</param>
        /// <param name="multiPlatform">if set to <c>true</c> [multi platform].</param>
        internal ConnectionProviderEntry(string name, string desc, string asmPath, bool multiPlatform)
        {
            this.Name = name;
            this.Description = desc;
            this.IsMultiPlatform = multiPlatform;
            this.AssemblyPath = asmPath;
        }
    }

    /// <summary>
    /// A method that creates <see cref="T:OSGeo.MapGuide.MaestroAPI.IServerConnection"/> instances from the given parameters
    /// </summary>
    /// <param name="initParams">The init params.</param>
    /// <returns></returns>
    public delegate IServerConnection ConnectionFactoryMethod(NameValueCollection initParams);

    /// <summary>
    /// <para>
    /// The entry point of the Maestro API. The <see cref="ConnectionProviderRegistry"/> is used to create <see cref="IServerConnection"/>
    /// objects. <see cref="IServerConnection"/> is the root object of the Maestro API, and is where most of the functionality provided
    /// by this API is accessed from.
    /// </para>
    /// <para>
    /// The <see cref="ConnectionProviderRegistry"/> supports dynamic creation of <see cref="IServerConnection"/> objects given a provider name
    /// and a connection string, which specifies the initialization parameters of the connection. The connection providers are defined in an XML
    /// file called ConnectionProviders.xml which contains all the registered providers. Each provider has the following properties:
    /// </para>
    /// <list type="number">
    ///     <item><description>The name of the provider</description></item>
    ///     <item><description>The assembly containing the <see cref="IServerConnection"/> implementation</description></item>
    ///     <item><description>The name of this <see cref="IServerConnection"/> implementation</description></item>
    /// </list>
    /// <para>
    /// The <see cref="IServerConnection"/> implementation is expected to have a non-public constructor which takes a single parameter, 
    /// a <see cref="System.Collections.Specialized.NameValueCollection"/> containing the initialization parameters parsed from the given connection
    /// string.
    /// </para>
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
        const string PROVIDER_CONFIG = "ConnectionProviders.xml"; //NOXLATE

        static Dictionary<string, ConnectionFactoryMethod> _ctors;
        static List<ConnectionProviderEntry> _providers;
        static Dictionary<string, int> _callCount;

        static string _dllRoot;

        static ConnectionProviderRegistry()
        {
            _ctors = new Dictionary<string, ConnectionFactoryMethod>();
            _providers = new List<ConnectionProviderEntry>();
            _callCount = new Dictionary<string, int>();

            var dir = System.IO.Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            var path = System.IO.Path.Combine(dir, PROVIDER_CONFIG);

            _dllRoot = System.IO.Path.GetDirectoryName(path);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList providers = doc.SelectNodes("//ConnectionProviderRegistry/ConnectionProvider"); //NOXLATE
            foreach (XmlNode prov in providers)
            {
                string name = prov["Name"].InnerText.ToUpper(); //NOXLATE
                string desc = prov["Description"].InnerText; //NOXLATE
                string dll = prov["Assembly"].InnerText; //NOXLATE
                string type = prov["Type"].InnerText; //NOXLATE

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
                        var impl = attr[0].ImplType;
                        _ctors[name] = new ConnectionFactoryMethod((initParams) =>
                        {
                            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance;
                            IServerConnection conn = (IServerConnection)impl.InvokeMember(null, flags, null, null, new object[] { initParams });
                            return conn;
                        });
                        _providers.Add(new ConnectionProviderEntry(name, desc, dll, attr[0].IsMultiPlatform));
                        _callCount[name] = 0;
                    }
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Parses the given Maestro connection string into a <see cref="T:System.Collections.Specialized.NameValueCollection"/>
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static NameValueCollection ParseConnectionString(string connectionString)
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
        /// Gets the invocation count for the given provider (the number of times a connection has been created for that provider)
        /// </summary>
        /// <remarks>
        /// This (in conjunction with the <see cref="P:OSGeo.MapGuide.MaestroAPI.ConnectionProviderEntry.HasGlobalState"/> property) can
        /// be used to programmatically determine if creating a connection for a given provider will respect the connection parameter values
        /// you pass to it. (0 calls will respect your parameter values. 1 or more will not)
        /// </remarks>
        /// <param name="provider"></param>
        /// <returns>The invocation count for the given provider. -1 if the provider is not registered or does not exist</returns>
        public static int GetInvocationCount(string provider)
        {
            if (_callCount.ContainsKey(provider.ToUpper()))
                return _callCount[provider.ToUpper()];

            return -1;
        }

        /// <summary>
        /// Registers a new connection provider
        /// </summary>
        /// <param name="entry">The provider entry.</param>
        /// <param name="method">The factory method.</param>
        public static void RegisterProvider(ConnectionProviderEntry entry, ConnectionFactoryMethod method)
        {
            string name = entry.Name.ToUpper();
            if (_ctors.ContainsKey(name))
                throw new ArgumentException(string.Format(Strings.ErrorProviderAlreadyRegistered, entry.Name));

            _ctors[name] = method;
            _providers.Add(entry);
        }

        /// <summary>
        /// Creates an initialized <see cref="IServerConnection"/> object given the provider name and connection string
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="connectionString"></param>
        /// <remarks>
        /// The Maestro.Local provider (that wraps mg-desktop) and Maestro.LocalNative providers (that wraps the official MapGuide API)
        /// are unique in that it has global connection state. What this means is that subsequent connections after the first one for 
        /// these providers may re-use existing state for the first connection. The reason for this is that creating this connection 
        /// internally calls MgdPlatform.Initialize(iniFile) and MapGuideApi.MgInitializeWebTier(iniFile) respectively, that initializes 
        /// the necessary library parameters in the process space of your application. Creating another connection will call 
        /// MgdPlatform.Initialize and MapGuideApi.MgInitializeWebTier again, but these methods are by-design only made to be called once 
        /// as subsequent calls are returned immediately.
        /// 
        /// Basically, the connection parameters you pass in are for initializing the provider the first time round. Subsequent calls may not
        /// (most likely will not) respect the values of your connection parameters. 
        /// 
        /// You can programmatically check this via the <see cref="P:OSGeo.MapGuide.MaestroAPI.ConnectionProviderEntry.HasGlobalState"/> property
        /// </remarks>
        /// <returns></returns>
        public static IServerConnection CreateConnection(string provider, string connectionString)
        {
            string name = provider.ToUpper();
            if (!_ctors.ContainsKey(name))
                throw new ArgumentException(string.Format(Strings.ErrorProviderNotRegistered, provider));

            ConnectionProviderEntry prv = FindProvider(provider);
            if (prv != null && !prv.IsMultiPlatform && Platform.IsRunningOnMono)
                throw new NotSupportedException(string.Format(Strings.ErrorProviderNotUsableForYourPlatform, provider));

            ConnectionFactoryMethod method = _ctors[name];

            NameValueCollection initParams = ParseConnectionString(connectionString);
            IServerConnection result = method(initParams);
            _callCount[name]++;
            return result;
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
                throw new ArgumentException(string.Format(Strings.ErrorProviderNotRegistered, provider));

            ConnectionProviderEntry prv = FindProvider(provider);
            if (prv != null && !prv.IsMultiPlatform && Platform.IsRunningOnMono)
                throw new NotSupportedException(string.Format(Strings.ErrorProviderNotUsableForYourPlatform, provider));

            var method = _ctors[name];
            IServerConnection result = method(connInitParams);
            _callCount[name]++;
            return result;
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

        /// <summary>
        /// Gets the entry for the given connection provider name
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static ConnectionProviderEntry FindProvider(string provider)
        {
            string cmp = provider.ToUpper();
            foreach (var prv in _providers)
            {
                if (prv.Name == cmp)
                    return prv;
            }

            return null;
        }
    }
}
