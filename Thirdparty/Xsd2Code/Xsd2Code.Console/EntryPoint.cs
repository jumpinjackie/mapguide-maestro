using System;
using System.IO;
using System.Reflection;
using Xsd2Code.Library;
using Xsd2Code.Library.Helpers;
using Xsd2Code.Properties;

namespace Xsd2Code
{
    /// <summary>
    /// Entry point class 
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Modified 2009-02-20 by Ruslan Urban
    ///     Changed parsing of command-line arguments
    /// 
    ///     Modified 2009-03-12 By Ruslan Urban
    ///     Enabled GenerateDataContracts option
    /// 
    /// </remarks>
    internal class EntryPoint
    {
        [STAThread]
        static private void Main(string[] args)
        {
            // Display hekp when no parameters have been specified
            if (args.Length < 1)
            {
                DisplayApplicationInfo();
                DisplayHelp();
                return;
            }

            DisplayApplicationInfo();

            // Create new instance of GeneratorParams, which contains all generation parameters
            var xsdFilePath = args[0];
            var generatorParams = new GeneratorParams {InputFilePath = xsdFilePath};

            // When only the XSD file name is specified, 
            // try to locate generated Designer file and load output parameters from the file header
            if (args.Length == 1)
            {
                string outputFilePath;
                var tempGeneratorParams = GeneratorParams.LoadFromFile(xsdFilePath, out outputFilePath);
                if (tempGeneratorParams != null)
                {
                    generatorParams = tempGeneratorParams;
                    generatorParams.InputFilePath = xsdFilePath;

                    if (!string.IsNullOrEmpty(outputFilePath))
                        generatorParams.OutputFilePath = outputFilePath;
                }
            }

            // Second command-line parameter that is not a switch  is the generated code namespace
            if (args.Length > 1 && !args[1].StartsWith("/"))
            {
                generatorParams.NameSpace = args[1];

                // Third command-line parameter that is not a switch is the generated code namespace
                if (args.Length > 2 && !args[2].StartsWith("/"))
                    generatorParams.OutputFilePath = args[2];
            }


            // Process command-line parameter switches
            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i].Trim().ToLower())
                {
                    
                    case "/n":
                    case "/ns":
                    case "/namespace":
                        if (i < args.Length - 1)
                        {
                            generatorParams.NameSpace = args[i + 1];
                            i++;
                        }
                        break;

                    case "/o":
                    case "/out":
                    case "/output":
                        if (i < args.Length - 1)
                        {
                            generatorParams.OutputFilePath = args[i + 1];
                            i++;
                        }
                        break;

                    case "/l":
                    case "/lang":
                    case "/language":
                        if (i < args.Length - 1)
                        {
                            generatorParams.Language = Utility.GetGenerationLanguage(args[i + 1]);
                            i++;
                        }
                        break;

                    case "/c":
                    case "/col":
                    case "/collection":
                    case "/collectionbase":
                        if (i < args.Length - 1)
                        {
                            generatorParams.CollectionObjectType = Utility.ToEnum<CollectionType>(args[i + 1]);
                            i++;
                        }
                        break;

                    //Duplicate with /pl
                    case "/cb":
                    case "/codebase":
                        if (i < args.Length - 1)
                        {
                            Console.WriteLine("Warning: /cb /codebase is obsolete please use /pl[atform] <Platform> - Generated code target platform (Net20|Net30|Net35|Silverlight20). Default: Net20");
                            generatorParams.TargetFramework = Utility.ToEnum<TargetFramework>(args[i + 1]);
                            i++;
                        }
                        break;

                    case "/cu":
                    case "/customusings":
                        if (i < args.Length - 1)
                        {
                            //generatorParams.CustomUsingsString = args[i + 1];

                            foreach(string item in args[i+1].Split(','))
                            {
                                generatorParams.CustomUsings.Add(new NamespaceParam { NameSpace = item });
                            }                            

                            i++;
                        }
                        break;

                    case "/lf":
                    case "/lfm":
                        if (i < args.Length - 1)
                        {
                            generatorParams.LoadFromFileMethodName = args[i + 1];
                            i++;
                        }
                        break;

                    case "/sm":
                        if (i < args.Length - 1)
                        {
                            generatorParams.SerializeMethodName = args[i + 1];
                            i++;
                        }
                        break;

                    case "/sf":
                    case "/sfm":
                        if (i < args.Length - 1)
                        {
                            generatorParams.SaveToFileMethodName = args[i + 1];
                            i++;
                        }
                        break;

                    case "/p":
                    case "/pl":
                    case "/tp":
                    case "/tpl":
                    case "/platform":
                        if (i < args.Length - 1)
                        {
                            generatorParams.TargetFramework = Utility.GetTargetPlatform(args[i + 1]);
                            i++;
                        }
                        break;

                    case "/u":
                    case "/us":
                    case "/using":
                        if (i < args.Length - 1)
                        {
                            var nsList = args[i + 1].Split(',');
                            foreach (var ns in nsList)
                                generatorParams.CustomUsings.Add(new NamespaceParam {NameSpace = ns});
                            i++;
                        }
                        break;

                    case "/dbg":
                    case "/dbg+":
                    case "/debug":
                    case "/debug+":
                        generatorParams.DisableDebug = false;
                        break;
                    case "/dbg-":
                    case "/debug-":
                        generatorParams.DisableDebug = true;
                        break;

                    case "/db":
                    case "/db+":
                        generatorParams.EnableDataBinding = true;
                        break;
                    case "/db-":
                        generatorParams.EnableDataBinding = false;
                        break;

                    case "/dc":
                    case "/dc+":
                        generatorParams.GenerateDataContracts = true;
                        break;
                    case "/dc-":
                        generatorParams.GenerateDataContracts = false;
                        break;

                    case "/ap":
                    case "/ap+":
                        generatorParams.AutomaticProperties = true;
                        break;
                    case "/ap-":
                        generatorParams.AutomaticProperties = false;
                        break;

                    case "/if":
                    case "/if+":
                        generatorParams.EnableInitializeFields = true;
                        break;
                    case "/if-":
                        generatorParams.EnableInitializeFields = false;
                        break;

                    case "/eit":
                    case "/eit+":
                        generatorParams.ExcludeIncludedTypes = true;
                        break;
                    case "/eit-":
                        generatorParams.ExcludeIncludedTypes = false;
                        break;

                    case "/gbc":
                    case "/gbc+":
                        generatorParams.UseGenericBaseClass = true;
                        break;
                    case "/gbc-":
                        generatorParams.UseGenericBaseClass = false;
                        break;

                    case "/sc":
                    case "/sc+":
                        generatorParams.EnableSummaryComment = true;
                        break;
                    case "/sc-":
                    case "/sum-":
                        generatorParams.EnableSummaryComment = false;
                        break;

                    case "/xa":
                    case "/xa+":
                        generatorParams.GenerateXMLAttributes = true;
                        break;
                    case "/xa-":
                        generatorParams.GenerateXMLAttributes = false;
                        break;

                    case "/cl":
                    case "/cl+":
                        generatorParams.GenerateCloneMethod = true;
                        break;
                    case "/cl-":
                        generatorParams.GenerateCloneMethod = false;
                        break;

                    case "/hp":
                    case "/hp+":
                        generatorParams.HidePrivateFieldInIde = true;
                        break;
                    case "/hp-":
                        generatorParams.HidePrivateFieldInIde = false;
                        break;

                    case "/ll":
                    case "/ll+":
                        generatorParams.EnableLasyLoading = true;
                        break;
                    case "/ll-":
                        generatorParams.EnableLasyLoading = false;
                        break;

                    case "/s":
                    case "/s+":
                    case "/is":
                    case "/is+":
                        generatorParams.IncludeSerializeMethod = true;
                        break;
                    case "/s-":
                    case "/is-":
                        generatorParams.IncludeSerializeMethod = false;
                        break;

                    case "/lic":
                    case "/license":
                        DisplayLicense();
                        return;

                    case "/?":
                    case "/h":
                    case "/hlp":
                    case "/help":
                        DisplayHelp();
                        return;
                }
            }

            // Auto-generate missing output file path
            if (string.IsNullOrEmpty(generatorParams.OutputFilePath))
            {
                generatorParams.OutputFilePath =
                    Utility.GetOutputFilePath(generatorParams.InputFilePath,
                                              generatorParams.Language);
            }

            // Auto-generate missing generated code namespace
            if (string.IsNullOrEmpty(generatorParams.NameSpace))
                generatorParams.NameSpace = Path.GetFileNameWithoutExtension(generatorParams.InputFilePath);


            // Create an instance of Generator
            var generator = new GeneratorFacade(generatorParams);

            // Generate code
            var result = generator.Generate();
            if (!result.Success)
            {
                // Display the error and wait for user confirmation
                Console.WriteLine();
                Console.WriteLine(result.Messages.ToString());
                Console.WriteLine();
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();

                return;
            }

            Console.WriteLine("Generated code has been saved into the file {0}.", result.Entity);

            Console.WriteLine();
            Console.WriteLine("Finished");
            Console.WriteLine();
        }

        static private void DisplayApplicationInfo()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var currentAssemblyName = currentAssembly.GetName();

            Console.WriteLine();
            Console.WriteLine("Xsd2Code Version {0}", currentAssemblyName.Version);
            Console.WriteLine("Code generation utility from XML schema files.");
            Console.WriteLine();
        }

        /// <summary>
        /// Display contents of the help file ~/Resources/Help.txt
        /// </summary>
        static private void DisplayHelp()
        {
            Console.WriteLine();
            Console.WriteLine(Resources.Help);
            Console.WriteLine();
        }

        /// <summary>
        /// Display contents of the help file ~/Resources/Help.txt
        /// </summary>
        static private void DisplayLicense()
        {
            Console.WriteLine();
            Console.WriteLine(Resources.License);
            Console.WriteLine();
        }
    }
}