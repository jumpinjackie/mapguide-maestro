using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SignMapGuideApi
{
    class Program
    {
        static string[] files = { "MapGuideDotNetApi", "OSGeo.MapGuide.Foundation", "OSGeo.MapGuide.Geometry", "OSGeo.MapGuide.MapGuideCommon", "OSGeo.MapGuide.PlatformBase", "OSGeo.MapGuide.Web" };
        
        static Dictionary<string, string[]> ilasm32Paths = new Dictionary<string, string[]>()
        {
            { "2.0", new string [] { "C:\\Windows\\Microsoft.NET\\Framework\\v2.0.50727\\ilasm.exe" } },
            { "3.5", new string [] { "C:\\Windows\\Microsoft.NET\\Framework\\v2.0.50727\\ilasm.exe" } },
            { "4.0", new string [] { "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\ilasm.exe" } },
        };

        static Dictionary<string, string[]> ilasm64Paths = new Dictionary<string, string[]>()
        {
            { "2.0", new string [] { "C:\\Windows\\Microsoft.NET\\Framework64\\v2.0.50727\\ilasm.exe" } },
            { "3.5", new string [] { "C:\\Windows\\Microsoft.NET\\Framework64\\v2.0.50727\\ilasm.exe" } },
            { "4.0", new string [] { "C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\ilasm.exe" } },
        };

        static Dictionary<string, string[]> ildasm32Paths = new Dictionary<string, string[]>()
        {
            { "2.0", new string [] { "C:\\Program Files\\Microsoft SDKs\\Windows\\v6.0A\\Bin\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v6.0A\\Bin\\ildasm.exe",
                                 "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\ildasm.exe",
                                 "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.1\\Bin\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.1\\Bin\\ildasm.exe" }},
            { "3.5", new string [] { "C:\\Program Files\\Microsoft SDKs\\Windows\\v6.0A\\Bin\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v6.0A\\Bin\\ildasm.exe",
                                 "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\ildasm.exe",
                                 "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.1\\Bin\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.1\\Bin\\ildasm.exe" }},
            { "4.0", new string [] { "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.0A\\bin\\NETFX 4.0 Tools\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.0A\\bin\\NETFX 4.0 Tools\\ildasm.exe",
                                 "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.1\\bin\\NETFX 4.0 Tools\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.1\\bin\\NETFX 4.0 Tools\\ildasm.exe" }}
        };

        static Dictionary<string, string[]> ildasm64Paths = new Dictionary<string, string[]>()
        {
            { "2.0", new string [] { "C:\\Program Files\\Microsoft SDKs\\Windows\\v6.0A\\Bin\\x64\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v6.0A\\Bin\\x64\\ildasm.exe",
                                 "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\x64\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\x64\\ildasm.exe",
                                 "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.1\\Bin\\x64\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.1\\Bin\\x64\\ildasm.exe" }},
            { "3.5", new string [] { "C:\\Program Files\\Microsoft SDKs\\Windows\\v6.0A\\Bin\\x64\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v6.0A\\Bin\\x64\\ildasm.exe",
                                 "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\x64\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\x64\\ildasm.exe",
                                 "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.1\\Bin\\x64\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.1\\Bin\\\x64\\ildasm.exe" }},
            { "4.0", new string [] { "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.0A\\bin\\NETFX 4.0 Tools\\x64\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.0A\\bin\\NETFX 4.0 Tools\\x64\\ildasm.exe",
                                 "C:\\Program Files\\Microsoft SDKs\\Windows\\v7.1\\bin\\NETFX 4.0 Tools\\x64\\ildasm.exe",
                                 "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.1\\bin\\NETFX 4.0 Tools\\x64\\ildasm.exe" }}
        };

        //TODO: Allow for custom key and infer public key token from it
        static string _keyFile = "maestroapi.key";
        static string _publicKeyToken = "F5 26 C4 89 29 FD A8 56";

        static bool Is64BitProcess()
        {
            return IntPtr.Size == 8;
        }

        static string getILDASMEXE(string frameworkVersion)
        {
            if (Is64BitProcess())
            {
                if (!ildasm64Paths.ContainsKey(frameworkVersion))
                    throw new Exception("No registered ildasm.exe search paths for framework version: " + frameworkVersion);

                foreach (string f in ildasm64Paths[frameworkVersion])
                    if (File.Exists(f))
                        return f;
            }
            else
            {
                if (!ildasm32Paths.ContainsKey(frameworkVersion))
                    throw new Exception("No registered ildasm.exe search paths for framework version: " + frameworkVersion);

                foreach (string f in ildasm32Paths[frameworkVersion])
                    if (File.Exists(f))
                        return f;
            }
            return null;
        }

        static string getILASMEXE(string frameworkVersion)
        {
            if (Is64BitProcess())
            {
                if (!ilasm64Paths.ContainsKey(frameworkVersion))
                    throw new Exception("No registered ilasm.exe search paths for framework version: " + frameworkVersion);

                foreach (string f in ilasm64Paths[frameworkVersion])
                    if (File.Exists(f))
                        return f;
            }
            else
            {
                if (!ilasm32Paths.ContainsKey(frameworkVersion))
                    throw new Exception("No registered ilasm.exe search paths for framework version: " + frameworkVersion);

                foreach (string f in ilasm32Paths[frameworkVersion])
                    if (File.Exists(f))
                        return f;
            }
            return null;
        }

        static void backup()
        {
            Console.Write("Creating Backup folder...");
            Directory.CreateDirectory("Backup");
            Console.WriteLine("Done");
            Console.WriteLine("Backing up original DLL files.");
            foreach (string f in files)
            {
                string file = f + ".dll";
                if (!File.Exists(file))
                {
                    Console.WriteLine("[WARNING]: Could not find file to backup (" + file + ")");
                    continue;
                }
                Console.Write("Copying " + file + "...");
                File.Copy(file, "Backup\\" + file, true);
                Console.WriteLine("Done");
            }
        }

        static void decompile(string decompiler)
        {
            foreach (string f in files)
            {
                string file = f + ".dll";
                if (!File.Exists(file))
                {
                    Console.WriteLine("[WARNING]: Could not find file to decompilation (" + file + ")");
                    continue;
                }
                Console.Write("Decompiling: " + file + "...");
                Process p = Process.Start(decompiler, "/all /out=" + f + ".il " + file);
                p.WaitForExit();
                p.Dispose();
                Console.WriteLine("Done");
            }
        }

        static void cleandll()
        {
            foreach (string f in files)
            {
                string file = f + ".dll";
                Console.Write("Deleting " + file + "...");
                File.Delete(file);
                Console.WriteLine("done");
            }
        }

        static void compile(string compiler, string file)
        {
            if (!File.Exists(file + ".il"))
            {
                Console.WriteLine("[WARNING]: No IL file to compile (" + file + ".il)");
                return;
            }

            Console.Write("Compiling: " + file + ".il...");
            Process p = Process.Start(compiler, "/dll /key=" + _keyFile + " " + file + ".il");
            p.WaitForExit();
            p.Dispose();
            Console.WriteLine("Done");
        }

        static bool assemblyLine(string line)
        {
            string l = line.ToLower();
            if (l.StartsWith(".assembly extern"))
            {
                bool ends = false;
                foreach (string f in files)
                {
                    if (l.EndsWith(f.ToLower()))
                    {
                        ends = true;
                        Console.WriteLine("  Added key to section: " + l);
                        break;
                    }
                }
                return ends;
            }
            return false;
        }

        static void fixKey(string file)
        {
            string ilfile = file + ".il";
            if (!File.Exists(ilfile))
            {
                Console.WriteLine("[WARNING]: Cannot find IL file to fix (" + ilfile + ")");
                return;
            }

            string tempfile = file + ".temp";
            File.Move(ilfile, tempfile); // rename file to .temp
            System.IO.StreamReader fin = new System.IO.StreamReader(tempfile); // open file for read
            System.IO.StreamWriter fout = new System.IO.StreamWriter(ilfile, false); // open file for write
            string line;
            Console.WriteLine("Fixing: " + ilfile);
            int count = 0;
            while ((line = fin.ReadLine()) != null) // read line by line
            {
                fout.WriteLine(line); // copy line to new file
                if (assemblyLine(line)) // if line starts with .assembly extern and ends with one in file names
                {
                    line = fin.ReadLine(); // Copy next line containing: {
                    fout.WriteLine(line);
                    fout.WriteLine("  .publickeytoken = (" + _publicKeyToken + " )"); // Write .publickeytoken line
                }
                count++;
            }
            fout.Close();
            fin.Close();
        }

        static void cleanup()
        {
            Console.Write("Cleanup...");
            foreach (string f in files)
            {
                if (File.Exists(f + ".temp"))
                    File.Delete(f + ".temp");

                if (File.Exists(f + ".il"))
                    File.Delete(f + ".il");

                if (File.Exists(f + ".res"))
                    File.Delete(f + ".res");
            }
            Console.WriteLine("Done");
        }

        static void Main(string[] args)
        {
            string fxVersion = "2.0"; //default version
            foreach (var arg in args)
            {
                if (arg.StartsWith("/framework="))
                {
                    fxVersion = arg.Substring("/framework=".Length);
                    break;
                }
            }
            Console.WriteLine("Target Framework: " + fxVersion);
            try
            {
                if (File.Exists(_keyFile))
                {
                    Console.Write("Detecting tools...");
                    string ildasmexe = getILDASMEXE(fxVersion); // Detect 32 or 64 bit versions of decompiler and compiler
                    string ilasmexe = getILASMEXE(fxVersion);
                    Console.WriteLine("Done");
                    Console.WriteLine("Compiler: " + ilasmexe);
                    Console.WriteLine("Decompiler: " + ildasmexe);
                    backup(); // make backup copy of dll
                    decompile(ildasmexe); // decompile dll files
                    cleandll();
                    for (int i = files.Length - 1; i >= 0; i--) // Compile the il files back to dll, fixing any missing key tokens in the references
                    {
                        fixKey(files[i]);
                        compile(ilasmexe, files[i]);
                    }
                    cleanup();
                    Console.WriteLine("All OSGeo files has been signed.");
                }
                else
                {
                    Console.WriteLine("Error: The file '" + _keyFile + "' is missing. This is required to sign the DLL files.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\r\n" + ex.ToString());
            }
        }
    }
}
