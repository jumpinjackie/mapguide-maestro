﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

using ICSharpCode.Core;
using ICSharpCode.Core.Services;
using ICSharpCode.Core.WinForms;
using Maestro.Base;
using Maestro.Base.Services;
using Maestro.Base.UI;
using Maestro.Editors.Common;
using Maestro.Login;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Maestro
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            string lang = PreferredSiteList.InitCulture();

            //var btw = BroadcastTextWriter.Instance;

            // Logging service by default uses System.Diagnostics.Debug. Re-route this
            // to our writer
            //var wfmsg = WinFormsMessageService.Instance;
            //ServiceManager.LoggingService = new TextWriterLoggingService(btw);
            //ServiceManager.MessageService = wfmsg;

            ServiceManager.Instance = new MaestroServiceManager();
            LoggingService.Info("Application start"); //NOXLATE

            //Init the Maestro connection registry with additional providers from ConnectionProviders.xml
            ConnectionProviderRegistry.InitRegistry();

            if (Platform.IsRunningOnMono)
            {
                LoggingService.Info(Strings.Warn_Mono);
            }

            //Init our default set of resource validators
            ResourceValidatorLoader.LoadStockValidators();

            AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // Get a reference to the entry assembly (Startup.exe)
            Assembly exe = typeof(Program).Assembly;

            // Set the root path of our application. ICSharpCode.Core looks for some other
            // paths relative to the application root:
            // "data/resources" for language resources, "data/options" for default options
            FileUtility.ApplicationRootPath = Path.GetDirectoryName(exe.Location);

            LoggingService.Info("Starting core services..."); //NOXLATE

            // CoreStartup is a helper class making starting the Core easier.
            // The parameter is used as the application name, e.g. for the default title of
            // MessageService.ShowMessage() calls.
            CoreStartup coreStartup = new CoreStartup("MapGuide Maestro"); //NOXLATE
            // It is also used as default storage location for the application settings:
            // "%Application Data%\%Application Name%", but you can override that by setting c.ConfigDirectory

            // #1955: Each version of Maestro from here on in will store their user data under
            // %APPDATA%\Maestro-x.y
            coreStartup.ConfigDirectory = MaestroPaths.BasePath;

            // Specify the name of the application settings file (.xml is automatically appended)
            coreStartup.PropertiesName = "AppProperties"; //NOXLATE

            // Initializes the Core services (ResourceService, PropertyService, etc.)
            coreStartup.StartCoreServices();

            LoggingService.Info("Looking for AddIns..."); //NOXLATE
            // Searches for ".addin" files in the application directory.
            coreStartup.AddAddInsFromDirectory(Path.Combine(FileUtility.ApplicationRootPath, "AddIns")); //NOXLATE

            // Searches for a "AddIns.xml" in the user profile that specifies the names of the
            // add-ins that were deactivated by the user, and adds "external" AddIns.
            coreStartup.ConfigureExternalAddIns(Path.Combine(PropertyService.ConfigDirectory, "AddIns.xml")); //NOXLATE

            // Searches for add-ins installed by the user into his profile directory. This also
            // performs the job of installing, uninstalling or upgrading add-ins if the user
            // requested it the last time this application was running.
            coreStartup.ConfigureUserAddIns(Path.Combine(PropertyService.ConfigDirectory, "AddInInstallTemp"), //NOXLATE
                                            Path.Combine(PropertyService.ConfigDirectory, "AddIns")); //NOXLATE

            ResourceService.Language = lang;
            LoggingService.Info("Loading AddInTree..."); //NOXLATE
            // Now finally initialize the application. This parses the ".addin" files and
            // creates the AddIn tree. It also automatically runs the commands in
            // "/Workspace/Autostart"
            coreStartup.RunInitialization();

            LoggingService.Info("Initializing Workbench..."); //NOXLATE
            // Workbench is our class from the base project, this method creates an instance
            // of the main form.
            ServiceRegistry.Initialize(() =>
            {
                //DIRTY DIRTY HACK: I'm getting tired of Mono workarounds. But background resource
                //preview preparation has a chance of clogging up if the main window is initially maximized
                Workbench.InitializeWorkbench(new WorkbenchInitializer(!Platform.IsRunningOnMono));
                try
                {
                    LoggingService.Info("Running application..."); //NOXLATE
                    // Workbench.Instance is the instance of the main form, run the message loop.
                    Application.Run(Workbench.Instance);
                }
                finally
                {
                    try
                    {
                        // Save changed properties
                        PropertyService.Save();
                    }
                    catch (Exception ex)
                    {
                        ErrorDialog.Show(Strings.Error_StoreProperties, ex.ToString());
                    }
                }
                LoggingService.Info("Application shutdown"); //NOXLATE
            });
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                if (XmlContentErrorDialog.IsDbXmlError(ex) && XmlContentErrorDialog.HasOriginalXml(ex))
                {
                    XmlContentErrorDialog.Show(ex);
                }
                else
                {
                    ErrorDialog.Show(ex);
                }
            }
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            LoggingService.InfoFormatted($"Loaded assembly: {args.LoadedAssembly.GetName().Name}"); //NOXLATE
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            if (XmlContentErrorDialog.IsDbXmlError(ex) && XmlContentErrorDialog.HasOriginalXml(ex))
            {
                XmlContentErrorDialog.Show(ex);
            }
            else
            {
                ErrorDialog.Show(ex);
            }
        }
    }
    
    internal sealed class MaestroServiceManager : ServiceManager
    {
        private static readonly ILoggingService loggingService = new TextWriterLoggingService(BroadcastTextWriter.Instance);
        private static readonly IMessageService messageService = new WinFormsMessageService(); //new TextWriterMessageService(Console.Out);

        public override ILoggingService LoggingService => loggingService;

        public override IMessageService MessageService => messageService;

        public override object GetService(Type serviceType)
        {
            if (serviceType == typeof(ILoggingService))
                return loggingService;
            else if (serviceType == typeof(IMessageService))
                return messageService;
            else
                return null;
        }
    }
}