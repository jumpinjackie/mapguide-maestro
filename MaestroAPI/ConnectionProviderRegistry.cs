using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Collections.Specialized;

namespace OSGeo.MapGuide.MaestroAPI
{
    public class ConnectionProviderEntry
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        internal ConnectionProviderEntry(string name, string desc)
        {
            this.Name = name;
            this.Description = desc;
        }
    }

    /// <summary>
    /// The <see cref="ConnectionProviderRegistry"/> supports dynamic creation of <see cref="ServerConnectionI"/> objects given a provider name
    /// and a connection string, which specifies the initialization parameters of the connection. The connection providers are defined in an XML
    /// file called ConnectionProviders.xml which contains all the registered providers. Each provider has the following properties:
    /// 
    /// 1. The name of the provider
    /// 2. The assembly containing the <see cref="ServerConnectionI"/> implementation
    /// 3. The name of this <see cref="ServerConnectionI"/> implementation.
    /// 
    /// The <see cref="ServerConnectionI"/> implementation is expected to have a non-public constructor which takes a single parameter, 
    /// a <see cref="System.Collections.Specialized.NameValueCollection"/> containing the initialization parameters parsed from the given connection
    /// string.
    /// </summary>
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

                Assembly asm = Assembly.LoadFrom(dll);
                Type t = asm.GetType(type);

                _ctors[name] = t;
                _providers.Add(new ConnectionProviderEntry(name, desc));
            }
        }

        private static NameValueCollection ParseConnectionString(string connectionString)
        {
            System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
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
        /// Creates an initialized <see cref="ServerConnectionI"/> object given the provider name and connection string
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static ServerConnectionI CreateConnection(string provider, string connectionString)
        {
            string name = provider.ToUpper();
            if (!_ctors.ContainsKey(name))
                throw new ArgumentException("Provider not registered: " + provider);

            Type t = _ctors[name];

            NameValueCollection initParams = ParseConnectionString(connectionString);

            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance;
            ServerConnectionI conn = (ServerConnectionI)t.InvokeMember(null, flags, null, null, new object[] { initParams });
            return conn;
        }
    }
}
