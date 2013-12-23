using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProviderTemplate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnFxDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtFxDir.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnMgDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtMgDir.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            btnBuild.Enabled = false;
            worker.RunWorkerAsync(new BuildArgs() { 
                FxDir = txtFxDir.Text,
                MgDir = txtMgDir.Text,
                MgVersion = txtMgVersion.Text,
                RefMapGuideDotNetApi = rdPre22.Checked,
                DebugMode = chkDebug.Checked
            });
        }

        private void AppendMessage(string msg)
        {
            Action action = () =>
            {
                txtMessages.AppendText(msg + Environment.NewLine);
            };
            txtMessages.BeginInvoke(action);
        }

        class BuildArgs
        {
            public string FxDir;
            public string MgDir;
            public string MgVersion;
            public bool RefMapGuideDotNetApi;
            public bool DebugMode;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var ba = (BuildArgs)e.Argument;
            AppendMessage("Validating input parameters");
            if (!Directory.Exists(ba.FxDir))
            {
                throw new Exception("The specified .net framework 4.0 directory does not exist");
            }
            else
            {
                AppendMessage(".net Framework 4.0  directory found");
            }
            if (!Directory.Exists(ba.MgDir))
            {
                throw new Exception("The specified MapGuide .net assemblies directory does not exist");
            }
            else
            {
                AppendMessage("MapGuide .net assemblies directory found");
            }
            var sdkBinDir = new DirectoryInfo(Path.Combine(Application.StartupPath, "..\\Bin"));
            var outputDir = new DirectoryInfo(Path.Combine(Application.StartupPath, "Bin"));
            var srcDir = new DirectoryInfo(Path.Combine(Application.StartupPath, "Src"));
            var srcCmdDir = new DirectoryInfo(Path.Combine(srcDir.ToString(), "Commands"));
            var libDir = new DirectoryInfo(Path.Combine(Application.StartupPath, "Lib\\MapGuide"));
            var signer = new FileInfo(Path.Combine(sdkBinDir.ToString(), "SignMapGuideApi.exe"));
            var keyfile = new FileInfo(Path.Combine(sdkBinDir.ToString(), "maestroapi.key"));

            if (!sdkBinDir.Exists)
            {
                throw new Exception("The SDK binaries directory could not be found: " + sdkBinDir + ". Ensure this executable is run from the 'LocalNativeProvider' subdirectory of your SDK root directory");
            }
            else
            {
                AppendMessage("SDK binaries directory exists");
                if (!signer.Exists)
                {
                    throw new Exception("Could not find the SignMapGuideApi.exe in the SDK binaries directory");
                }
                if (!keyfile.Exists)
                {
                    throw new Exception("Could not find the maestroapi.key in the SDK binaries directory");
                }
            }
            if (!outputDir.Exists)
            {
                outputDir.Create();
                AppendMessage("Created: " + outputDir);
            }
            else
            {
                foreach (var f in outputDir.GetFiles("*.dll"))
                    f.Delete();
                AppendMessage("Cleaned output directory: " + outputDir);
            }
            if (!libDir.Exists)
            {
                libDir.Create();
                AppendMessage("Created: " + libDir);
            }
            else
            {
                foreach (var f in libDir.GetFiles("*.dll"))
                    f.Delete();
                foreach (var f in libDir.GetFiles("*.exe"))
                    f.Delete();
                foreach (var f in libDir.GetFiles("*.key"))
                    f.Delete();
                AppendMessage("Cleaned lib directory: " + libDir);
            }
            AppendMessage("Copying MapGuide dlls to lib");
            foreach (string f in Directory.GetFiles(txtMgDir.Text, "*.dll"))
            {
                var fo = Path.Combine(libDir.ToString(), Path.GetFileName(f));
                File.Copy(f, fo);
                AppendMessage("Copied: " + f);
            }
            AppendMessage("Signing MapGuide assemblies");
            var targetSignerPath = Path.Combine(libDir.ToString(), signer.Name);
            var targetKeyPath = Path.Combine(libDir.ToString(), keyfile.Name);
            File.Copy(signer.ToString(), targetSignerPath);
            File.Copy(keyfile.ToString(), targetKeyPath);
            using (var proc = SetupProcess(targetSignerPath, null))
            {
                proc.Start();
                proc.BeginOutputReadLine();
                proc.WaitForExit();
                proc.CancelOutputRead();
                File.Delete(targetSignerPath);
                File.Delete(targetKeyPath);
            }

            AppendMessage("Write out AssemblyInfo.cs");
            File.WriteAllText(Path.Combine(srcDir.ToString(), "AssemblyInfo.cs"), string.Format(Properties.Resources.AssemblyInfo, ba.MgVersion));

            string asmName = "OSGeo.MapGuide.MaestroAPI.Native-" + ba.MgVersion + ".dll";
            AppendMessage("http://xkcd.com/303/ - " + asmName);
            var args = new List<string>();
            if (ba.DebugMode)
            {
                args.Add("/debug+");
                args.Add("/debug:full");
            }
            else
            {
                args.Add("/debug-");
                args.Add("/debug:pdbonly");
                args.Add("/optimize+");
            }
            args.Add("/target:library");
            //args.Add("/platform:x86");
            args.Add("/keyfile:\"" + keyfile + "\"");
            args.Add("/out:\"" + Path.Combine(outputDir.ToString(), asmName) + "\"");
            args.Add("/reference:\"" + Path.Combine(sdkBinDir.ToString(), "OSGeo.MapGuide.MaestroAPI.dll") + "\"");
            args.Add("/reference:\"" + Path.Combine(sdkBinDir.ToString(), "NetTopologySuite.Merged.dll") + "\"");
            if (ba.RefMapGuideDotNetApi)
            {
                args.Add("/reference:\"" + Path.Combine(libDir.ToString(), "MapGuideDotNetApi.dll") + "\"");
            }
            else
            {
                args.Add("/reference:\"" + Path.Combine(libDir.ToString(), "OSGeo.MapGuide.Foundation.dll") + "\"");
                args.Add("/reference:\"" + Path.Combine(libDir.ToString(), "OSGeo.MapGuide.Geometry.dll") + "\"");
                args.Add("/reference:\"" + Path.Combine(libDir.ToString(), "OSGeo.MapGuide.PlatformBase.dll") + "\"");
                args.Add("/reference:\"" + Path.Combine(libDir.ToString(), "OSGeo.MapGuide.MapGuideCommon.dll") + "\"");
                args.Add("/reference:\"" + Path.Combine(libDir.ToString(), "OSGeo.MapGuide.Web.dll") + "\"");
            }
            args.Add("\"" + Path.Combine(srcDir.ToString(), "*.cs") + "\"");
            args.Add("\"" + Path.Combine(srcCmdDir.ToString(), "*.cs") + "\"");
            var cscPath = Path.Combine(ba.FxDir, "csc.exe");
            using (var proc = SetupProcess(cscPath, string.Join(" ", args.ToArray()).Trim()))
            {
                proc.Start();
                proc.BeginOutputReadLine();
                proc.WaitForExit();
                proc.CancelOutputRead();
                AppendMessage("csc.exe returned " + proc.ExitCode);
            }
        }

        private Process SetupProcess(string exe, string argsStr)
        {
            var proc = new Process();
            proc.StartInfo.FileName = exe;
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(exe);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            if (!string.IsNullOrEmpty(argsStr))
                proc.StartInfo.Arguments = argsStr;
            proc.OutputDataReceived += (sndr, args) => AppendMessage(args.Data);
            return proc;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnBuild.Enabled = true;
            if (e.Error != null)
            {
                AppendMessage("Build FAILED");
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                AppendMessage("Build SUCCESS");
                MessageBox.Show("Build completed. See readme.txt for further instructions");
            }
        }
    }
}
