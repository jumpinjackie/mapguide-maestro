using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Xsd2Code.Library.Helpers;

namespace Xsd2Code.Library
{
    /// <summary>
    /// Represents all generation parameters
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Modified 2009-02-20 by Ruslan Urban
    ///     Added TargetFramework and GenerateCloneMethod properties
    ///     Modified 2009-05-18 by Pascal Cabanel.
    ///     Added NET 2.0 serialization attributes as an option
    /// </remarks>
    public class GeneratorParams
    {
        #region Private

        /// <summary>
        /// Type of collection
        /// </summary>
        private CollectionType collectionObjectTypeField = CollectionType.List;

        /// <summary>
        /// List of custom usings
        /// </summary>
        private List<NamespaceParam> customUsingsField = new List<NamespaceParam>();

        /// <summary>
        /// Indicate if use automatic properties
        /// </summary>
        private bool automaticPropertiesField = false;
        #endregion

        /// <summary>
        /// Indicate the target framework
        /// </summary>
        private TargetFramework targetFrameworkField = default(TargetFramework);

        /// <summary>
        /// Indicate id databinding is enable
        /// </summary>
        private bool enableDataBindingField = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorParams"/> class.
        /// </summary>
        public GeneratorParams()
        {
            this.LoadFromFileMethodName = "LoadFromFile";
            this.SaveToFileMethodName = "SaveToFile";
            this.DeserializeMethodName = "Deserialize";
            this.SerializeMethodName = "Serialize";
            this.BaseClassName = "EntityBase";
            this.UseGenericBaseClass = false;
            this.EnableInitializeFields = true;
            this.ExcludeIncludedTypes = false;
        }

        /// <summary>
        /// Gets or sets the name space.
        /// </summary>
        /// <value>The name space.</value>
        [Category("Code")]
        [Description("namespace of generated file")]
        public string NameSpace { get; set; }

        /// <summary>
        /// Gets or sets generation language
        /// </summary>
        [Category("Code")]
        [Description("Language")]
        public GenerationLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets the output file path.
        /// </summary>
        /// <value>The output file path.</value>
        [Browsable(false)]
        public string OutputFilePath { get; set; }

        /// <summary>
        /// Gets or sets the input file path.
        /// </summary>
        /// <value>The input file path.</value>
        [Browsable(false)]
        public string InputFilePath { get; set; }

        /// <summary>
        /// Gets or sets collection type to use for code generation
        /// </summary>
        [Category("Collection")]
        [Description("Set type of collection for unbounded elements")]
        public CollectionType CollectionObjectType
        {
            get { return this.collectionObjectTypeField; }
            set { this.collectionObjectTypeField = value; }
        }

        /// <summary>
        /// Gets or sets collection base
        /// </summary>
        [Category("Collection")]
        [Description("Set the collection base if using CustomCollection")]
        public string CollectionBase { get; set; }

        /// <summary>
        /// Gets or sets custom usings
        /// </summary>
        [Category("Collection")]
        [Description("list of custom using for CustomCollection")]
        public List<NamespaceParam> CustomUsings
        {
            get { return this.customUsingsField; }
            set { this.customUsingsField = value; }
        }

        /// <summary>
        /// Gets or sets the custom usings string.
        /// </summary>
        /// <value>The custom usings string.</value>
        [Browsable(false)]
        public string CustomUsingsString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if implement INotifyPropertyChanged
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether if implement INotifyPropertyChanged.")]
        public bool EnableDataBinding
        {
            get
            {
                return this.enableDataBindingField;
            }

            set
            {
                this.enableDataBindingField = value;
                if (this.enableDataBindingField)
                {
                    this.AutomaticProperties = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if implement INotifyPropertyChanged
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Use lasy pattern when possible.")]
        public bool EnableLasyLoading { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether serialize/deserialize method support
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether serialize/deserialize method nust be generate.")]
        public bool IncludeSerializeMethod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether serialize/deserialize method support
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Generate automatic properties when possible. (Work only for csharp with target framework 3.0 or 3.5 and EnableDataBinding disable)")]
        public bool AutomaticProperties
        {
            get
            {
                return this.automaticPropertiesField;
            }

            set
            {
                if (value)
                {
                    if (this.targetFrameworkField != TargetFramework.Net20)
                    {
                        this.automaticPropertiesField = value;
                        this.EnableDataBinding = false;
                    }
                }
                else
                {
                    this.automaticPropertiesField = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether serialize/deserialize method support
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether serialize/deserialize method nust be generate.")]
        public bool GenerateCloneMethod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether serialize/deserialize method support
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(Library.TargetFramework.Net20)]
        [Description("Generated code base")]
        public TargetFramework TargetFramework
        {
            get
            {
                return this.targetFrameworkField;
            }

            set
            {
                this.targetFrameworkField = value;
                if (this.targetFrameworkField == TargetFramework.Net20)
                {
                    this.AutomaticProperties = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if generate EditorBrowsableState.Never attribute
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether if generate EditorBrowsableState.Never attribute.")]
        public bool HidePrivateFieldInIde { get; set; }

        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating to exclude class generation types includes/imported into schema.")]
        public bool ExcludeIncludedTypes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if generate EditorBrowsableState.Never attribute
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether if generate .NET 2.0 serialization attributes.")]
        public bool GenerateXMLAttributes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [disable debug].
        /// </summary>
        /// <value><c>true</c> if [disable debug]; otherwise, <c>false</c>.</value>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether if generate attribute for debug into generated code.")]
        public bool DisableDebug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if generate summary documentation
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether if generate summary documentation from xsd annotation.")]
        public bool EnableSummaryComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if generate summary documentation
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Generate WCF data contract attributes")]
        public bool GenerateDataContracts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of Serialize method.
        /// </summary>
        [Category("Behavior"), DefaultValue(false), Description("Use generic patial base class for all methods")]
        public bool UseGenericBaseClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of Serialize method.
        /// </summary>
        [Category("Serialize"), DefaultValue("EntityBase"), Description("Name of generic patial base class")]
        public string BaseClassName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of Serialize method.
        /// </summary>
        [Category("Serialize"), DefaultValue("Serialize"), Description("The name of Serialize method.")]
        public string SerializeMethodName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of Deserialize method.
        /// </summary>
        [Category("Serialize"), DefaultValue("Deserialize"), Description("The name of deserialize method.")]
        public string DeserializeMethodName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of Serialize method.
        /// </summary>
        [Category("Serialize"), DefaultValue("SaveToFile"), Description("The name of save to xml file method.")]
        public string SaveToFileMethodName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of SaveToFile method.
        /// </summary>
        [Category("Serialize"), DefaultValue("LoadFromFile"), Description("The name of load from xml file method.")]
        public string LoadFromFileMethodName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether accessing a property will initialize it
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Enable/Disable Global initialisation of the fields in both Constructors, Lazy Properties. Maximum override")]
        public bool EnableInitializeFields { get; set; }
       
        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <param name="xsdFilePath">The XSD file path.</param>
        /// <returns>GeneratorParams instance</returns>
        public static GeneratorParams LoadFromFile(string xsdFilePath)
        {
            string outFile;
            return LoadFromFile(xsdFilePath, out outFile);
        }

        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <param name="xsdFilePath">The XSD file path.</param>
        /// <param name="outputFile">The output file.</param>
        /// <returns>GeneratorParams instance</returns>
        public static GeneratorParams LoadFromFile(string xsdFilePath, out string outputFile)
        {
            var parameters = new GeneratorParams();


            #region Search generationFile
            outputFile = string.Empty;

            foreach (GenerationLanguage language in Enum.GetValues(typeof(GenerationLanguage)))
            {
                string fileName = Utility.GetOutputFilePath(xsdFilePath, language);
                if (File.Exists(fileName))
                {
                    outputFile = fileName;
                    break;
                }
            }


            if (outputFile.Length == 0)
                return null;

            #endregion

            #region Try to get Last options

            using (TextReader streamReader = new StreamReader(outputFile))
            {
                streamReader.ReadLine();
                streamReader.ReadLine();
                streamReader.ReadLine();
                string optionLine = streamReader.ReadLine();
                if (optionLine != null)
                {
                    parameters.NameSpace = XmlHelper.ExtractStrFromXML(optionLine, GeneratorContext.NAMESPACETAG);
                    parameters.CollectionObjectType =
                            Utility.ToEnum<CollectionType>(
                                    XmlHelper.ExtractStrFromXML(optionLine, GeneratorContext.COLLECTIONTAG));
                    parameters.Language =
                            Utility.ToEnum<GenerationLanguage>(XmlHelper.ExtractStrFromXML(optionLine,
                                                                                           GeneratorContext.CODETYPETAG));
                    parameters.EnableDataBinding =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ENABLEDATABINDINGTAG));
                    parameters.EnableLasyLoading =
                             Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ENABLELASYLOADINGTAG));
                    parameters.HidePrivateFieldInIde =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.HIDEPRIVATEFIELDTAG));
                    parameters.EnableSummaryComment =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ENABLESUMMARYCOMMENTTAG));
                    parameters.IncludeSerializeMethod =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.INCLUDESERIALIZEMETHODTAG));
                    parameters.GenerateCloneMethod =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.GENERATECLONEMETHODTAG));
                    parameters.GenerateDataContracts =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.GENERATEDATACONTRACTSTAG));
                    parameters.TargetFramework =
                            Utility.ToEnum<TargetFramework>(optionLine.ExtractStrFromXML(GeneratorContext.CODEBASETAG));
                    parameters.DisableDebug =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.DISABLEDEBUGTAG));

                    parameters.GenerateXMLAttributes = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.GENERATEXMLATTRIBUTESTAG));

                    parameters.AutomaticProperties = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.AUTOMATICPROPERTIESTAG));

                    parameters.UseGenericBaseClass = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.USEGENERICBASECLASSTAG));

                    parameters.EnableInitializeFields =
                        Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.ENABLEINITIALIZEFIELDSTAG),true);

                    parameters.ExcludeIncludedTypes = Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.EXCLUDEINCLUDEDTYPESTAG));

                    string str = optionLine.ExtractStrFromXML(GeneratorContext.SERIALIZEMETHODNAMETAG);
                    parameters.SerializeMethodName = str.Length > 0 ? str : "Serialize";

                    str = optionLine.ExtractStrFromXML(GeneratorContext.DESERIALIZEMETHODNAMETAG);
                    parameters.DeserializeMethodName = str.Length > 0 ? str : "Deserialize";

                    str = optionLine.ExtractStrFromXML(GeneratorContext.SAVETOFILEMETHODNAMETAG);
                    parameters.SaveToFileMethodName = str.Length > 0 ? str : "SaveToFile";

                    str = optionLine.ExtractStrFromXML(GeneratorContext.LOADFROMFILEMETHODNAMETAG);
                    parameters.LoadFromFileMethodName = str.Length > 0 ? str : "LoadFromFile";

                    str = optionLine.ExtractStrFromXML(GeneratorContext.BASECLASSNAMETAG);
                    parameters.BaseClassName = str.Length > 0 ? str : "EntityBase";

                    // TODO:get custom using
                    string customUsingString = optionLine.ExtractStrFromXML(GeneratorContext.CUSTOMUSINGSTAG);
                    if (!string.IsNullOrEmpty(customUsingString))
                    {
                        string[] usings = customUsingString.Split(';');
                        foreach (string item in usings)
                            parameters.CustomUsings.Add(new NamespaceParam { NameSpace = item });
                    }
                    parameters.CollectionBase = optionLine.ExtractStrFromXML(GeneratorContext.COLLECTIONBASETAG);
                }
            }

            return parameters;

            #endregion
        }

        /// <summary>
        /// Gets the params.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>GeneratorParams instance</returns>
        public static GeneratorParams GetParams(string parameters)
        {
            var newparams = new GeneratorParams();

            return newparams;
        }

        /// <summary>
        /// Save values into xml string
        /// </summary>
        /// <returns>xml string value</returns>
        public string ToXmlTag()
        {
            var optionsLine = new StringBuilder();

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.NAMESPACETAG, this.NameSpace));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.COLLECTIONTAG,
                                                          this.CollectionObjectType.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.CODETYPETAG, this.Language.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.ENABLEDATABINDINGTAG,
                                                          this.EnableDataBinding.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.ENABLELASYLOADINGTAG,
                                                          this.EnableLasyLoading.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.HIDEPRIVATEFIELDTAG,
                                                          this.HidePrivateFieldInIde.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.ENABLESUMMARYCOMMENTTAG,
                                                          this.EnableSummaryComment.ToString()));

            if (!string.IsNullOrEmpty(this.CollectionBase))
                optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.COLLECTIONBASETAG, this.CollectionBase));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.INCLUDESERIALIZEMETHODTAG,
                                                          this.IncludeSerializeMethod.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.USEGENERICBASECLASSTAG,
                                                          this.UseGenericBaseClass.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.GENERATECLONEMETHODTAG,
                                                          this.GenerateCloneMethod.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.GENERATEDATACONTRACTSTAG,
                                                          this.GenerateDataContracts.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.CODEBASETAG,
                                                          this.TargetFramework.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.SERIALIZEMETHODNAMETAG,
                                                          this.SerializeMethodName));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.DESERIALIZEMETHODNAMETAG,
                                                          this.DeserializeMethodName));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.SAVETOFILEMETHODNAMETAG,
                                                          this.SaveToFileMethodName));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.LOADFROMFILEMETHODNAMETAG,
                                                          this.LoadFromFileMethodName));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.GENERATEXMLATTRIBUTESTAG,
                                                          this.GenerateXMLAttributes.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                          GeneratorContext.AUTOMATICPROPERTIESTAG,
                                                          this.AutomaticProperties.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.DISABLEDEBUGTAG, this.DisableDebug.ToString()));

            var customUsingsStr = new StringBuilder();
            if (this.CustomUsings != null)
            {
                foreach (NamespaceParam usingStr in this.CustomUsings)
                {
                    if (usingStr.NameSpace.Length > 0)
                        customUsingsStr.Append(usingStr.NameSpace + ";");
                }

                // remove last ";"
                if (customUsingsStr.Length > 0)
                {
                    if (customUsingsStr[customUsingsStr.Length - 1] == ';')
                        customUsingsStr = customUsingsStr.Remove(customUsingsStr.Length - 1, 1);
                }

                optionsLine.Append(XmlHelper.InsertXMLFromStr(
                                                              GeneratorContext.CUSTOMUSINGSTAG,
                                                              customUsingsStr.ToString()));
            }

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.EXCLUDEINCLUDEDTYPESTAG,
                                                          this.ExcludeIncludedTypes.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.ENABLEINITIALIZEFIELDSTAG,
                                                          this.EnableInitializeFields.ToString()));


            return optionsLine.ToString();
        }

        /// <summary>
        /// Shallow clone
        /// </summary>
        /// <returns></returns>
        public GeneratorParams Clone()
        {
            return MemberwiseClone() as GeneratorParams;
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns></returns>
        public Result Validate()
        {
            var result = new Result(true);

            #region Validate input

            if (string.IsNullOrEmpty(this.NameSpace))
            {
                result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the NameSpace");
            }

            if (this.CollectionObjectType.ToString() == CollectionType.DefinedType.ToString())
            {
                if (string.IsNullOrEmpty(this.CollectionBase))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the custom collection base type");
                }
            }

            if (this.IncludeSerializeMethod)
            {
                if (string.IsNullOrEmpty(this.SerializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the Serialize method name.");
                }

                if (!IsValidMethodName(this.SerializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Serialize method name {0} is invalid.",
                                                  this.SerializeMethodName));
                }

                if (string.IsNullOrEmpty(this.DeserializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the Deserialize method name.");
                }

                if (!IsValidMethodName(this.DeserializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Deserialize method name {0} is invalid.",
                                                  this.DeserializeMethodName));
                }

                if (string.IsNullOrEmpty(this.SaveToFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the save to xml file method name.");
                }

                if (!IsValidMethodName(this.SaveToFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Save to file method name {0} is invalid.",
                                                  this.SaveToFileMethodName));
                }

                if (string.IsNullOrEmpty(this.LoadFromFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the load from xml file method name.");
                }

                if (!IsValidMethodName(this.LoadFromFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Load from file method name {0} is invalid.",
                                                  this.LoadFromFileMethodName));
                }
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static bool IsValidMethodName(string value)
        {
            foreach (char item in value)
            {
                int ascii = Convert.ToInt16(item);
                if ((ascii < 65 || ascii > 90) && (ascii < 97 || ascii > 122) && (ascii != 8))
                    return false;
            }
            return true;
        }

    }
}