using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xsd2Code.Library;
using Xsd2Code.Library.Helpers;
using Xsd2Code.TestUnit.Properties;

namespace Xsd2Code.TestUnit
{
    /// <summary>
    /// Xsd2Code unit tests
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Modified 2009-02-25 by Ruslan Urban 
    ///     Performed code review
    ///     Changed output folder to the TestResults folder to preserve files in the testing history
    ///     TODO: Add tests that compile generated code
    /// 
    /// </remarks>
    [TestClass]
    public class UnitTest
    {
        private readonly object testLock = new object();
        static readonly object fileLock = new object();

        /// <summary>
        /// Output folder: TestResults folder relative to the solution root folder
        /// </summary>
        private static string OutputFolder
        {
            get { return @"c:\temp\"; } // Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\"; }
        }

        /// <summary>
        /// Code generation namespace  
        /// </summary>
        private const string CodeGenerationNamespace = "Xsd2Code.TestUnit";

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        /// <summary>
        /// Circulars this instance.
        /// </summary>
        [TestMethod]
        public void Circular()
        {
            lock (testLock)
            {
                // Copy resource file to the run-time directory
                string inputFilePath = GetInputFilePath("Circular.xsd", Resources.Circular);

                var xsdGen = new GeneratorFacade(GetGeneratorParams(inputFilePath));
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());
            }
        }

        /// <summary>
        /// Circulars this instance.
        /// </summary>
        [TestMethod]
        public void CircularClassReference()
        {
            lock (testLock)
            {

                // Copy resource file to the run-time directory
                string inputFilePath = GetInputFilePath("CircularClassReference.xsd", Resources.CircularClassReference);
                var generatorParams = new GeneratorParams
                                          {
                                              InputFilePath = inputFilePath,
                                              TargetFramework = TargetFramework.Net20,
                                              AutomaticProperties = true,
                                              IncludeSerializeMethod = false,
                                              UseGenericBaseClass = false,
                                              OutputFilePath = GetOutputFilePath(inputFilePath)

                                          };

                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                try
                {
                    var cs = new Circular();

#pragma warning disable 168
                    int count = cs.circular.count;
#pragma warning restore 168

                    var compileResult = CompileCSFile(generatorParams.OutputFilePath);
                    Assert.IsTrue(compileResult.Success, compileResult.Messages.ToString());
                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                }
            }
        }

        /// <summary>
        /// Arrays the of array.
        /// </summary>
        [TestMethod]
        public void ArrayOfArray()
        {
            lock (testLock)
            {

                // Copy resource file to the run-time directory
                var inputFilePath = GetInputFilePath("ArrayOfArray.xsd", Resources.ArrayOfArray);

                var generatorParams = new GeneratorParams
                                          {
                                              GenerateCloneMethod = true,
                                              IncludeSerializeMethod = true,
                                              AutomaticProperties = true,
                                              InputFilePath = inputFilePath,
                                              NameSpace = "MyNameSpace",
                                              CollectionObjectType = CollectionType.Array,
                                              EnableDataBinding = true,
                                              Language = GenerationLanguage.CSharp,
                                              OutputFilePath = Path.ChangeExtension(inputFilePath, ".TestGenerated.cs")
                                          };
                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                var compileResult = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(compileResult.Success, compileResult.Messages.ToString());
            }
        }

        /// <summary>
        /// Stacks the over flow.
        /// </summary>
        [TestMethod]
        public void StackOverFlow()
        {
            lock (testLock)
            {

                // Copy resource file to the run-time directory
                string inputFilePath = GetInputFilePath("StackOverFlow.xsd", Resources.StackOverFlow);

                var generatorParams = GetGeneratorParams(inputFilePath);
                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                var compileResult = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(compileResult.Success, compileResult.Messages.ToString());
            }
        }

        [TestMethod]
        public void Deserialize_ArrayOfMyElement()
        {
            lock (testLock)
            {

                var e = new ArrayOfMyElement();
                var myE = new MyElement {Name = "Name"};
                myE.AttributeLists.Add(new NameValuePair {Name = "Name", Value = "Value"});
                e.MyElement.Add(myE);
                Exception ex;

                var serialized = e.Serialize();
                e.SaveToFile(Path.Combine(OutputFolder, "ReproSampleFile.xml"), out ex);
                if (ex != null) throw ex;

                //try to deserialize

                //generate doc conformant to schema

                ArrayOfMyElement toDeserialize;
                if (!ArrayOfMyElement.LoadFromFile("ReproSampleFile.xml", out toDeserialize, out ex))
                {
                    Console.WriteLine("Unable to deserialize, will exit");
                    return;
                }

                var serialized2 = toDeserialize.Serialize();

                Console.WriteLine("Still missing the <NameValuePairElement>");
                Console.WriteLine(serialized);

                Console.WriteLine("Name value pairs elements missing");
                Console.WriteLine(serialized2);
            }
        }

        /// <summary>
        /// DVDs this instance.
        /// </summary>
        [TestMethod]
        public void Dvd()
        {
            lock (testLock)
            {

                // Copy resource file to the run-time directory
                GetInputFilePath("Actor.xsd", Resources.Actor);

                // Copy resource file to the run-time directory
                string inputFilePath = GetInputFilePath("Dvd.xsd", Resources.dvd);
                var generatorParams = GetGeneratorParams(inputFilePath);
                generatorParams.CollectionObjectType = CollectionType.List;
                generatorParams.TargetFramework = TargetFramework.Net35;
                generatorParams.EnableDataBinding = true;
                generatorParams.EnableSummaryComment = true;
                generatorParams.GenerateDataContracts = false;
                generatorParams.UseGenericBaseClass = false;
                generatorParams.GenerateXMLAttributes = true;
                

                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                // Create new dvd collection and save it to file
                var dvd = new DvdCollection();
                dvd.Dvds.Add(new dvd {Title = "Matrix"});
                var newitem = new dvd();
                newitem.Actor.Add(new Actor {firstname = "James", nationality = "Us"});
                dvd.Dvds.Add(newitem);
                var originalXml = dvd.Serialize();
                dvd.SaveToFile("dvd.xml");

                // Load data fom file and serialize it again.
                var loadedDvdCollection = DvdCollection.LoadFromFile("dvd.xml");
                var finalXml = loadedDvdCollection.Serialize();

                // Then comprate two xml string
                if (!originalXml.Equals(finalXml))
                {
                    Assert.Fail("Xml value are not equals");
                }

                var compileResult = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(compileResult.Success, compileResult.Messages.ToString());

            }
        }

        /// <summary>
        /// Genders this instance.
        /// </summary>
        [TestMethod]
        public void Gender()
        {
            lock (testLock)
            {

                // Get the code namespace for the schema.
                string inputFilePath = GetInputFilePath("Gender.xsd", Resources.Gender);

                var generatorParams = GetGeneratorParams(inputFilePath);
                generatorParams.TargetFramework = TargetFramework.Net30;
                generatorParams.AutomaticProperties = true;
                generatorParams.GenerateDataContracts = true;
                generatorParams.GenerateXMLAttributes = true;
                generatorParams.OutputFilePath = GetOutputFilePath(inputFilePath);

                var xsdGen = new GeneratorFacade(generatorParams);

                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                var genderRoot = new Root
                                     {
                                         GenderAttribute = ksgender.female,
                                         GenderAttributeSpecified = true,
                                         GenderElement = ksgender.female,
                                         GenderIntAttribute = "toto"
                                     };
                Exception ex;
                genderRoot.SaveToFile(Path.Combine(OutputFolder, "gender.xml"), out ex);
                if(ex!=null) throw ex;

                var canCompile = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(canCompile.Success, canCompile.Messages.ToString());
            }
        }

        [TestMethod]
        public void GenarateVBCS()
        {
            lock (testLock)
            {
                // Get the code namespace for the schema.
                string inputFilePath = GetInputFilePath("Actor.xsd", Resources.Actor);

                var generatorParams = GetGeneratorParams(inputFilePath);
                generatorParams.TargetFramework = TargetFramework.Net30;
                generatorParams.AutomaticProperties = true;
                generatorParams.GenerateDataContracts = true;
                generatorParams.GenerateXMLAttributes = true;
                generatorParams.OutputFilePath = GetOutputFilePath(inputFilePath);
                generatorParams.EnableDataBinding = true;
                generatorParams.EnableSummaryComment = true;
                generatorParams.Language = GenerationLanguage.VisualBasic;
                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();
                Assert.IsTrue(result.Success, result.Messages.ToString());

                generatorParams.Language = GenerationLanguage.CSharp;
                xsdGen = new GeneratorFacade(generatorParams);
                result = xsdGen.Generate();

                var canCompile = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(canCompile.Success, canCompile.Messages.ToString());
            }
        }
        /// <summary>
        /// Alows the debug.
        /// </summary>
        [TestMethod]
        public void AlowDebug()
        {
            lock (testLock)
            {

                // Copy resource file to the run-time directory
                GetInputFilePath("Actor.xsd", Resources.Actor);
                string inputFilePath = GetInputFilePath("Dvd.xsd", Resources.dvd);

                var generatorParams = GetGeneratorParams(inputFilePath);
                generatorParams.DisableDebug = false;
                generatorParams.OutputFilePath = Path.ChangeExtension(generatorParams.InputFilePath, ".DebugEnabled.cs");

                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                var compileResult = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(compileResult.Success, compileResult.Messages.ToString());
            }
        }

        [TestMethod]
        public void Hierarchical()
        {
            lock (testLock)
            {

                // Copy resource file to the run-time directory
                string inputFilePath = GetInputFilePath("Hierarchical.xsd", Resources.Hierarchical);

                var generatorParams = GetGeneratorParams(inputFilePath);
                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                var compileResult = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(compileResult.Success, compileResult.Messages.ToString());
            }
        }

        //[TestMethod]
        //public void Serialize()
        //{
        //    DvdCollection dvdCol = GetDvd();
        //    string dvdColStr1 = dvdCol.Serialize();

        //    DvdCollection dvdColFromXml;
        //    Exception exception;
        //    bool sucess = DvdCollection.Deserialize(dvdColStr1, out dvdColFromXml, out exception);
        //    if (sucess)
        //    {
        //        string dvdColStr2 = dvdColFromXml.Serialize();
        //        if (!dvdColStr1.Equals(dvdColStr2))
        //            Assert.Fail("dvdColFromXml is not equal after Deserialize");
        //    }
        //    else
        //        Assert.Fail(exception.Message);
        //}

        [TestMethod]
        public void Silverlight()
        {
            lock (testLock)
            {

                // Get the code namespace for the schema.
                GetInputFilePath("Actor.xsd", Resources.Actor);
                string inputFilePath = GetInputFilePath("dvd.xsd", Resources.dvd);

                var generatorParams = GetGeneratorParams(inputFilePath);
                generatorParams.TargetFramework = TargetFramework.Silverlight;
                generatorParams.OutputFilePath = Path.ChangeExtension(generatorParams.InputFilePath,
                                                                      ".Silverlight20_01.cs");

                var xsdGen = new GeneratorFacade(generatorParams);

                var result = xsdGen.Generate();
                Assert.IsTrue(result.Success, result.Messages.ToString());

            }
        }


        [TestMethod]
        public void XMLAttributes()
        {
            lock (testLock)
            {

                // Get the code namespace for the schema.
                GetInputFilePath("Actor.xsd", Resources.Actor);
                string inputFilePath = GetInputFilePath("dvd.xsd", Resources.dvd);

                var generatorParams = GetGeneratorParams(inputFilePath);
                generatorParams.GenerateXMLAttributes = true;

                generatorParams.TargetFramework = TargetFramework.Net20;
                generatorParams.OutputFilePath = Path.ChangeExtension(generatorParams.InputFilePath, ".xml.cs");

                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                var compileResult = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(compileResult.Success, compileResult.Messages.ToString());
            }
        }

        [TestMethod]
        public void AutomaticProperties()
        {
            lock (testLock)
            {

                // Get the code namespace for the schema.
                GetInputFilePath("Actor.xsd", Resources.Actor);
                string inputFilePath = GetInputFilePath("dvd.xsd", Resources.dvd);

                var generatorParams = new GeneratorParams {InputFilePath = inputFilePath};
                GetGeneratorParams(inputFilePath);
                generatorParams.EnableSummaryComment = true;
                generatorParams.GenerateDataContracts = false;
                generatorParams.AutomaticProperties = true;

                generatorParams.TargetFramework = TargetFramework.Net30;
                generatorParams.OutputFilePath = Path.ChangeExtension(generatorParams.InputFilePath, ".autoProp.cs");

                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                var compileResult = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(compileResult.Success, compileResult.Messages.ToString());
            }
        }

        [TestMethod]
        public void UseBaseClass()
        {
            lock (testLock)
            {

                // Get the code namespace for the schema.
                GetInputFilePath("Actor.xsd", Resources.Actor);
                string inputFilePath = GetInputFilePath("dvd.xsd", Resources.dvd);

                string outputFilePath = Path.ChangeExtension(inputFilePath, ".baseClass.cs");
                var generatorParams = new GeneratorParams
                                          {
                                              InputFilePath = inputFilePath,
                                              TargetFramework = TargetFramework.Net30,
                                              EnableSummaryComment = true,
                                              GenerateDataContracts = true,
                                              AutomaticProperties = false,
                                              EnableDataBinding = true,
                                              UseGenericBaseClass = true,
                                              BaseClassName = "EntityObject",
                                              OutputFilePath = outputFilePath
                                          };

                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                var compileResult = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(compileResult.Success, compileResult.Messages.ToString());
            }
        }

        [TestMethod]
        public void TestAnnotations()
        {
            lock (testLock)
            {

                // Get the code namespace for the schema.
                string inputFilePath = GetInputFilePath("TestAnnotations.xsd", Resources.TestAnnotations);

                var generatorParams = new GeneratorParams {InputFilePath = inputFilePath};
                GetGeneratorParams(inputFilePath);

                generatorParams.EnableSummaryComment = true;
                generatorParams.TargetFramework = TargetFramework.Net35;
                generatorParams.AutomaticProperties = true;
                generatorParams.OutputFilePath = Path.ChangeExtension(generatorParams.InputFilePath,
                                                                      ".TestAnnotations.cs");

                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                var compileResult = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(compileResult.Success, compileResult.Messages.ToString());
            }
        }

        [TestMethod]
        public void WcfAttributes()
        {
            lock (testLock)
            {

                // Get the code namespace for the schema.
                GetInputFilePath("Actor.xsd", Resources.Actor);
                string inputFilePath = GetInputFilePath("dvd.xsd", Resources.dvd);

                var generatorParams = GetGeneratorParams(inputFilePath);
                generatorParams.GenerateDataContracts = true;
                generatorParams.TargetFramework = TargetFramework.Net30;
                generatorParams.OutputFilePath = Path.ChangeExtension(generatorParams.InputFilePath, ".wcf.cs");

                var xsdGen = new GeneratorFacade(generatorParams);
                var result = xsdGen.Generate();

                Assert.IsTrue(result.Success, result.Messages.ToString());

                var compileResult = CompileCSFile(generatorParams.OutputFilePath);
                Assert.IsTrue(compileResult.Success, compileResult.Messages.ToString());
            }
        }

        //[TestMethod]
        //public void Persistent()
        //{
        //    DvdCollection dvdCol = GetDvd();
        //    Exception exception;
        //    if (!dvdCol.SaveToFile(OutputFolder + @"savedvd.xml", out exception))
        //        Assert.Fail(string.Format("Failed to save file. {0}", exception.Message));

        //    DvdCollection loadedDvdCollection;
        //    Exception e;
        //    if (!DvdCollection.LoadFromFile(OutputFolder + @"savedvd.xml", out loadedDvdCollection, out e))
        //        Assert.Fail(string.Format("Failed to load file. {0}", e.Message));

        //    string xmlBegin = dvdCol.Serialize();
        //    string xmlEnd = loadedDvdCollection.Serialize();

        //    if (!xmlBegin.Equals(xmlEnd))
        //        Assert.Fail(string.Format("xmlBegin and xmlEnd are not equal after LoadFromFile"));
        //}

        //[TestMethod]
        //public void InvalidLoadFromFile()
        //{
        //    DvdCollection loadedDvdCollection;
        //    Exception e;
        //    DvdCollection.LoadFromFile(OutputFolder + @"savedvd.error.xml", out loadedDvdCollection, out e);
        //}

        //private static DvdCollection GetDvd()
        //{
        //    var dvdCol = new DvdCollection();
        //    var newdvd = new dvd {Title = "Matrix", Style = Styles.Action};
        //    newdvd.Actor.Add(new Actor {firstname = "Thomas", lastname = "Anderson"});
        //    dvdCol.Dvds.Add(newdvd);
        //    return dvdCol;
        //}


        private static string GetInputFilePath(string resourceFileName, string fileContent)
        {
            lock (fileLock)
            {
                using (var sw = new StreamWriter(OutputFolder + resourceFileName, false))
                {
                    sw.Write(fileContent);
                }

                return OutputFolder + resourceFileName;
            }
        }

        private static GeneratorParams GetGeneratorParams(string inputFilePath)
        {
            return new GeneratorParams
                       {
                           InputFilePath = inputFilePath,
                           NameSpace = CodeGenerationNamespace,
                           TargetFramework = TargetFramework.Net20,
                           CollectionObjectType = CollectionType.ObservableCollection,
                           DisableDebug = true,
                           EnableDataBinding = true,
                           GenerateDataContracts = true,
                           GenerateCloneMethod = true,
                           IncludeSerializeMethod = true,
                           HidePrivateFieldInIde = true,
                           OutputFilePath = GetOutputFilePath(inputFilePath)
                       };
        }

        /// <summary>
        /// Get output file path
        /// </summary>
        /// <param name="inputFilePath">input file path</param>
        /// <returns></returns>
        static private string GetOutputFilePath(string inputFilePath)
        {
            return Path.ChangeExtension(inputFilePath, ".TestGenerated.cs");
        }

        /// <summary>
        /// Compile file
        /// </summary>
        /// <param name="filePath">CS file path</param>
        /// <returns></returns>
        static private Result<string> CompileCSFile(string filePath)
        {
            var result = new Result<string>(null, true);
            var file = new FileInfo(filePath);
            if (!file.Exists)
            {
                result.Success = false;
                result.Messages.Add(MessageType.Error, "Input file \"{0}\" does not exist", filePath);
            }
            if (result.Success)
            {
                try
                {
                    var outputPath = Path.ChangeExtension(file.FullName, ".dll");
                    result.Entity = outputPath;

                    var args = new StringBuilder();
                    args.Append(" /target:module /nologo /debug");
                    args.Append(" /out:\"" + outputPath + "\"");
                    args.Append(" \"" + filePath + "\"");

                    var compilerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System),
                                                    @"..\Microsoft.NET\Framework\v2.0.50727\csc.exe");

                    var compilerFile = new FileInfo(compilerPath);

                    Debug.WriteLine(string.Format("Executing:\r\n{0} {1}\r\n", compilerFile.FullName, args));

                    var info = new ProcessStartInfo
                                   {
                                       ErrorDialog = false,
                                       FileName = compilerFile.FullName,
                                       Arguments = args.ToString(),
                                       CreateNoWindow = true,
                                       WindowStyle = ProcessWindowStyle.Minimized
                                   };

                    using (var process = new Process {StartInfo = info})
                    {
                        process.ErrorDataReceived += (s, e) =>
                                                         {
                                                             result.Success = false;
                                                             result.Messages.Add(MessageType.Error, "Error data received", e.Data);
                                                         };

                        process.Exited += (s, e) => { result.Success = process.ExitCode == 1 && File.Exists(outputPath); };

                        process.OutputDataReceived += (s, e) => result.Messages.Add(MessageType.Debug, "Output data received", e.Data);

                        if (!process.Start())
                            throw new ApplicationException("Unablle to start process");

                        var exited = process.WaitForExit((int) TimeSpan.FromSeconds(15).TotalMilliseconds);
                        if (!exited)
                        {
                            result.Success = false;
                            result.Messages.Add(MessageType.Error, "Timeout", "Compile timeout occurred {0}", DateTime.Now - process.StartTime);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Messages.Add(MessageType.Error, "Exception", ex.ToString());
                }
            }

            if (result.Messages.Count > 0)
                Debug.WriteLine(result.Messages.ToString());

            return result;
        }

    }
}