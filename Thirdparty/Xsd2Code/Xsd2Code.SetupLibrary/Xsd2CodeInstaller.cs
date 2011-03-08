using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.Win32;

namespace Xsd2Code.SetupLibrary
{
    /// <summary>
    /// Custom action for add-in deployment.
    /// </summary>
    [RunInstaller(true)]
    public partial class Xsd2CodeInstaller : Installer
    {
        /// <summary>
        /// Namespace used in the .addin configuration file.
        /// </summary>         
        private const string ExtNameSpace = "http://schemas.microsoft.com/AutomationExtensibility";

        /// <summary>
        /// Addin control file name  
        /// </summary>
        private const string addinControlFileName = "Xsd2Code.Addin.Addin";

        /// <summary>
        /// Addin assembly file name  
        /// </summary>
        private const string addinAssemblyFileName = "Xsd2Code.Addin.dll";

        /// <summary>
        /// Saved state key  
        /// </summary>
        private const string savedStateVs2008Key = "vs2008AddinPath";

        /// <summary>
        /// Saved state key  
        /// </summary>
        private const string savedStateVs2010Key = "vs2010AddinPath";


        /// <summary>
        /// Vs2008 registry key
        /// </summary>
        private const string vs2008Key = @"SOFTWARE\Microsoft\VisualStudio\9.0";

        /// <summary>
        /// Vs2010 registry key
        /// </summary>
        private const string vs2010Key = @"SOFTWARE\Microsoft\VisualStudio\10.0";


        /// <summary>
        /// Constructor. Initializes components.
        /// </summary>
        public Xsd2CodeInstaller()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Overrides Installer.Install,
        /// which will be executed during install process.
        /// </summary>
        /// <param name="savedState">The saved state.</param>
        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);

            // Setup .addin path and assembly path
            string vs2008AddinTarget = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                @"Visual Studio 2008\Addins");

            string vs2010AddinTarget = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                @"Visual Studio 2010\Addins");

            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                string sourceFile = Path.Combine(assemblyPath, addinControlFileName);

                var addinXml = new XmlDocument();
                addinXml.Load(sourceFile);

                var nsmgr = new XmlNamespaceManager(addinXml.NameTable);
                nsmgr.AddNamespace("def", ExtNameSpace);

                // Update Addin/Assembly node
                SetNodeValue(addinXml, nsmgr,
                             "/def:Extensibility/def:Addin/def:Assembly",
                             Path.Combine(assemblyPath, addinAssemblyFileName));

                // Update ToolsOptionsPage/Assembly node
                SetNodeValue(addinXml, nsmgr,
                             "/def:Extensibility/def:ToolsOptionsPage/def:Category/def:SubCategory/def:Assembly",
                             Path.Combine(assemblyPath, addinAssemblyFileName));

                addinXml.Save(sourceFile);

                if (!string.IsNullOrEmpty(GetRegisteryValue(Registry.LocalMachine, vs2008Key, "InstallDir")))
                {
                    var targetFolder = new DirectoryInfo(vs2008AddinTarget);
                    if (!targetFolder.Exists) targetFolder.Create();

                    string targetFile = Path.Combine(vs2008AddinTarget, addinControlFileName);
                    File.Copy(sourceFile, targetFile, true);
                    savedState.Add(savedStateVs2008Key, targetFile);
                }

                if (!string.IsNullOrEmpty(GetRegisteryValue(Registry.LocalMachine, vs2010Key, "InstallDir")))
                {
                    var targetFolder = new DirectoryInfo(vs2010AddinTarget);
                    if (!targetFolder.Exists) targetFolder.Create();

                    string targetFile = Path.Combine(vs2010AddinTarget, addinControlFileName);
                    File.Copy(sourceFile, targetFile, true);
                    savedState.Add(savedStateVs2010Key, targetFile);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private static void SetNodeValue(XmlNode sourceNode, XmlNamespaceManager nsmgr, string xpath, string value)
        {
            var node = sourceNode.SelectSingleNode(xpath, nsmgr);
            if (node != null) node.InnerText = value;
        }

        /// <summary>
        /// Overrides Installer.Rollback, which will be executed during rollback process.
        /// </summary>
        /// <param name="savedState">The saved state.</param>
        public override void Rollback(IDictionary savedState)
        {
            ////Debugger.Break();

            base.Rollback(savedState);

            try
            {
                var fileName = (string)savedState[savedStateVs2008Key];
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (File.Exists(fileName)) File.Delete(fileName);
                }

                fileName = (string)savedState[savedStateVs2010Key];
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (File.Exists(fileName)) File.Delete(fileName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Overrides Installer.Uninstall, which will be executed during uninstall process.
        /// </summary>
        /// <param name="savedState">The saved state.</param>
        public override void Uninstall(IDictionary savedState)
        {
             base.Uninstall(savedState);

            try
            {
                var fileName = (string)savedState[savedStateVs2008Key];
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (File.Exists(fileName)) File.Delete(fileName);
                }

                fileName = (string)savedState[savedStateVs2010Key];
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (File.Exists(fileName)) File.Delete(fileName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Gets the registery value.
        /// </summary>
        /// <param name="hklm">The HKLM.</param>
        /// <param name="rootKey">The root key.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static string GetRegisteryValue(RegistryKey hklm, string rootKey, string keyName)
        {
            string result = string.Empty;
            hklm = hklm.OpenSubKey(rootKey, false);
            var val = hklm.GetValue(keyName);
            if (val != null)
            {
                return val.ToString();
            }
            return string.Empty;
        }
    }
}