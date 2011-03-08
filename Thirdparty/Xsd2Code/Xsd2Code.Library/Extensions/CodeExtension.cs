// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeExtension.cs" company="Xsd2Code">
//   N/A
// </copyright>
// <summary>
//   Base class for code generation extension
// </summary>
// <remarks>
//  Revision history:
//  Created 2009-03-16 by Ruslan Urban
//  based on GeneratorExtension.cs
//  Updated 2009-05-18 move wcf CodeDom generation into Net35Extention.cs by Pascal Cabanel
//  Updated 2009-05-18 Remove .Net 2.0 XML attributes by Pascal Cabanel
//  Updated 2009-06-16 Add EntityBase class.
//                     Add new serialize/deserialize methods.
//                     Dispose object in serialize/deserialize methods.
//  Updated 2010-01-07 Deerwood McCord Jr. (DCM) applied patch from Rob van der Veer
//  Updated 2010-01-20 Deerwood McCord Jr. Cleaned CodeSnippetStatements by replacing with specific CodeDom Expressions
//                     Refactored OnPropertyChanged to use more CodeDom Specific version found in CodeDomHelper.CreateOnPropertyChangeMethod()
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

namespace Xsd2Code.Library.Extensions
{
    using System;
    using System.CodeDom;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Helpers;

    /// <summary>
    /// Base class for code generation extension
    /// </summary>
    public abstract class CodeExtension : ICodeExtension
    {
        #region private fields
        /// <summary>
        /// Sorted list for custom collection
        /// </summary>
        private static readonly SortedList<string, string> CollectionTypes = new SortedList<string, string>();

        /// <summary>
        /// Contains all enum.
        /// </summary>
        private static List<string> enumListField;

        /// <summary>
        /// Contains all collection fields.
        /// </summary>
        private static List<string> lasyLoadingFields = new List<string>();

        /// <summary>
        /// Contains all collection fields.
        /// </summary>
        protected static List<string> collectionTypesFields = new List<string>();

        #endregion

        #region public method
        /// <summary>
        /// Process method for cs or vb CodeDom generation
        /// </summary>
        /// <param name="code">CodeNamespace generated</param>
        /// <param name="schema">XmlSchema to generate</param>
        public virtual void Process(CodeNamespace code, XmlSchema schema)
        {
            this.ImportNamespaces(code);
            CollectionTypes.Clear();
            lasyLoadingFields.Clear();
            collectionTypesFields.Clear();

            var types = new CodeTypeDeclaration[code.Types.Count];
            code.Types.CopyTo(types, 0);

            // Generate generic base class
            if (GeneratorContext.GeneratorParams.UseGenericBaseClass)
            {
                code.Types.Insert(0, this.GenerateBaseClass());
            }

            enumListField = (from p in types
                             where p.IsEnum
                             select p.Name).ToList();

            foreach (var type in types)
            {
                
                //Fixes http://xsd2code.codeplex.com/WorkItem/View.aspx?WorkItemId=8781
                //  and http://xsd2code.codeplex.com/WorkItem/View.aspx?WorkItemId=6944
                if (GeneratorContext.GeneratorParams.ExcludeIncludedTypes)
                {
                    //if the typeName is NOT defined in the current schema, skip it.
                    if (!ContainsTypeName(schema, type))
                    {
                        code.Types.Remove(type);
                        continue;
                    }
                }


                // Remove default remarks attribute
                type.Comments.Clear();

                // Remove default .Net 2.0 XML attributes if disabled.
                if (!GeneratorContext.GeneratorParams.GenerateXMLAttributes)
                {
                    this.RemoveDefaultXmlAttributes(type.CustomAttributes);
                }

                if (!type.IsClass && !type.IsStruct) continue;

                this.ProcessClass(code, schema, type);
            }

            foreach (string collName in CollectionTypes.Keys)
                this.CreateCollectionClass(code, collName);
        }

        /// <summary>
        /// Determines whether the specified schema contains the type.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if the specified schema contains the type; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>Used to Exclude Included Types from Schema</remarks>
        private bool ContainsTypeName(XmlSchema schema, CodeTypeDeclaration type)
        {
            foreach (var item in schema.Items)
            {
                var complexItem = item as XmlSchemaComplexType;
                if(complexItem != null)
                {
                    if (complexItem.Name == type.Name)
                    {
                        return true;
                    }
                }
                var elementItem = item as XmlSchemaElement;
                if (elementItem != null)
                {
                    if (elementItem.Name == type.Name)
                    {
                        return true;
                    }
                }

            }

            //TODO: Does not work for combined anonymous types 
            //fallback: Check if the namespace attribute of the type equals the namespace of the file.
            //first, find the XmlType attribute.
            foreach(CodeAttributeDeclaration attribute in type.CustomAttributes)
            {
                if(attribute.Name == "System.Xml.Serialization.XmlTypeAttribute")
                {
                    foreach(CodeAttributeArgument argument in attribute.Arguments)
                    {
                        if(argument.Name == "Namespace")
                        {
                            if(((CodePrimitiveExpression)argument.Value).Value == schema.TargetNamespace)
                            {
                                return true;
                            }
                        }
                    }
                }
                 
            }

            return false;
        }
        #endregion

        #region protedted methods
        /// <summary>
        /// Generate defenition of the Clone() method
        /// </summary>
        /// <param name="type">Represents a type declaration for a class, structure, interface, or enumeration</param>
        /// <returns>return CodeDom clone method</returns>
        protected static CodeTypeMember GetCloneMethod(CodeTypeDeclaration type)
        {
            string typeName = GeneratorContext.GeneratorParams.UseGenericBaseClass ? "T" : type.Name;

            // ----------------------------------------------------------------------
            // /// <summary>
            // /// Create clone of this TClass object
            // /// </summary>
            // public TClass Clone()
            // {
            //    return ((TClass)this.MemberwiseClone());
            // }
            // ----------------------------------------------------------------------
            var cloneMethod = new CodeMemberMethod
                                  {
                                      Attributes = MemberAttributes.Public,
                                      Name = "Clone",
                                      ReturnType = new CodeTypeReference(typeName)
                                  };

            CodeDomHelper.CreateSummaryComment(
                cloneMethod.Comments,
                string.Format("Create a clone of this {0} object", typeName));

            var memberwiseCloneMethod = new CodeMethodInvokeExpression(
                new CodeThisReferenceExpression(),
                "MemberwiseClone");

            var statement = new CodeMethodReturnStatement(new CodeCastExpression(typeName, memberwiseCloneMethod));
            cloneMethod.Statements.Add(statement);
            return cloneMethod;
        }

        /// <summary>
        /// Processes the class.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="schema">The input xsd schema.</param>
        /// <param name="type">Represents a type declaration for a class, structure, interface, or enumeration</param>
        protected virtual void ProcessClass(CodeNamespace codeNamespace, XmlSchema schema, CodeTypeDeclaration type)
        {
            var addedToConstructor = false;
            var newCTor = false;

            var ctor = this.GetConstructor(type, ref newCTor);

            // Inherits from EntityBase
            if (GeneratorContext.GeneratorParams.UseGenericBaseClass)
            {
                var ctr = new CodeTypeReference(GeneratorContext.GeneratorParams.BaseClassName);
                ctr.TypeArguments.Add(new CodeTypeReference(type.Name));
                type.BaseTypes.Add(ctr);
            }
            else
            {
                if (GeneratorContext.GeneratorParams.EnableDataBinding)
                    type.BaseTypes.Add(typeof(INotifyPropertyChanged));
            }

            // Generate WCF DataContract
            this.CreateDataContractAttribute(type, schema);

            XmlSchemaElement currentElement = null;
            if (GeneratorContext.GeneratorParams.EnableSummaryComment)
                currentElement = this.CreateSummaryCommentFromSchema(type, schema, currentElement);

            foreach (CodeTypeMember member in type.Members)
            {
                // Remove default remarks attribute
                member.Comments.Clear();

                // Remove default .Net 2.0 XML attributes if disabled.
                if (!GeneratorContext.GeneratorParams.GenerateXMLAttributes)
                {
                    this.RemoveDefaultXmlAttributes(member.CustomAttributes);
                }

                var codeMember = member as CodeMemberField;
                if (codeMember != null)
                    this.ProcessFields(codeMember, ctor, codeNamespace, ref addedToConstructor);

                var codeMemberProperty = member as CodeMemberProperty;
                if (codeMemberProperty != null)
                    this.ProcessProperty(type, codeNamespace, codeMemberProperty, currentElement, schema);
            }

            //DCM: Moved From GeneraterFacade File based removal to CodeDom Style Attribute-based removal
            if (GeneratorContext.GeneratorParams.DisableDebug)
            {
                this.RemoveDebugAttributes(type.CustomAttributes);
            }

            // Add new ctor if required
            if (addedToConstructor && newCTor)
                type.Members.Add(ctor);

            // If don't use base class, generate all methods inside class
            if (!GeneratorContext.GeneratorParams.UseGenericBaseClass)
            {
                if (GeneratorContext.GeneratorParams.EnableDataBinding)
                    this.CreateDataBinding(type);

                if (GeneratorContext.GeneratorParams.IncludeSerializeMethod)
                {
                    CreateStaticSerializer(type);
                    this.CreateSerializeMethods(type);
                }

                if (GeneratorContext.GeneratorParams.GenerateCloneMethod)
                    this.CreateCloneMethod(type);
            }
        }

        /// <summary>
        /// Create data binding
        /// </summary>
        /// <param name="type">Code type declaration</param>
        protected virtual void CreateDataBinding(CodeTypeDeclaration type)
        {
            // -------------------------------------------------------------------------------
            // public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
            // -------------------------------------------------------------------------------
            var propertyChangedEvent =
                new CodeMemberEvent
                    {
                        Attributes = MemberAttributes.Final | MemberAttributes.Public,
                        Name = "PropertyChanged",
                        Type =
                            new CodeTypeReference(typeof(PropertyChangedEventHandler))
                    };
            propertyChangedEvent.ImplementationTypes.Add(new CodeTypeReference("INotifyPropertyChanged"));
            type.Members.Add(propertyChangedEvent);

            // -----------------------------------------------------------
            //  protected virtual  void OnPropertyChanged(string info) {
            //      PropertyChangedEventHandler handler = PropertyChanged;
            //      if (handler != null) {
            //          handler(this, new PropertyChangedEventArgs(info));
            //      }
            //  }
            // -----------------------------------------------------------

            /* DCM REMOVED Uses CodeSnippetExpressions
            var propertyChangedMethod = CreatePropertyChangedMethod();
             */
 
            var propertyChangedMethod = CodeDomHelper.CreatePropertyChangedMethod();
            
            type.Members.Add(propertyChangedMethod);
        }


        /// <summary>
        /// Creates the property changed method.
        /// </summary>
        /// <returns>CodeMemberMethod on Property Change handler</returns>
        /// <remarks>
        /// DCM: Plan on deleting after Beta Period.
        /// Use Switch and Language specific CodeSnippetExpressions produce method
        /// VB version
        /// <code>
        /// public sub OnPropertyChanged(byval info as string)
        ///     RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
        /// end sub
        /// 
        /// </code>
        /// C# & CPP Version
        /// <code>
        ///  protected virtual  void OnPropertyChanged(string info) {
        ///      PropertyChangedEventHandler handler = PropertyChanged;
        ///      if (handler != null) {
        ///          handler(this, new PropertyChangedEventArgs(info));
        ///      }
        ///  }
        /// </code>
        /// </remarks>
        private static CodeMemberMethod CreatePropertyChangedMethod()
        {
            var propertyChangedMethod = new CodeMemberMethod
                                                         {
                                                             Name = "OnPropertyChanged",
                                                             Attributes = MemberAttributes.Public
                                                         };
            propertyChangedMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "info"));

            switch (GeneratorContext.GeneratorParams.Language)
            {
                case GenerationLanguage.VisualBasic:

                    propertyChangedMethod.Statements.Add(
                        new CodeExpressionStatement(
                            new CodeSnippetExpression(
                                "RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))")));

                    break;

                case GenerationLanguage.CSharp:
                case GenerationLanguage.VisualCpp:

                    propertyChangedMethod.Statements.Add(
                        new CodeExpressionStatement(
                            new CodeSnippetExpression("PropertyChangedEventHandler handler = PropertyChanged")));

                    var codeExpressionStatement =
                        new CodeExpressionStatement(
                            new CodeSnippetExpression("handler(this, new PropertyChangedEventArgs(info))"));

                    CodeStatement[] statements = new[] { codeExpressionStatement };

                    propertyChangedMethod.Statements.Add(
                        new CodeConditionStatement(new CodeSnippetExpression("handler != null"), statements));
                    break;
            }
            return propertyChangedMethod;
        }

        /// <summary>
        /// Creates the summary comment from schema.
        /// </summary>
        /// <param name="codeTypeDeclaration">The code type declaration.</param>
        /// <param name="schema">The input XML schema.</param>
        /// <param name="currentElement">The current element.</param>
        /// <returns>returns the element found otherwise null</returns>
        protected virtual XmlSchemaElement CreateSummaryCommentFromSchema(CodeTypeDeclaration codeTypeDeclaration, XmlSchema schema, XmlSchemaElement currentElement)
        {
            var xmlSchemaElement = this.SearchElementInSchema(codeTypeDeclaration, schema, new List<XmlSchema>());
            if (xmlSchemaElement != null)
            {
                currentElement = xmlSchemaElement;
                if (xmlSchemaElement.Annotation != null)
                {
                    foreach (var item in xmlSchemaElement.Annotation.Items)
                    {
                        var xmlDoc = item as XmlSchemaDocumentation;
                        if (xmlDoc == null) continue;
                        this.CreateCommentStatment(codeTypeDeclaration.Comments, xmlDoc);
                    }
                }
            }

            return currentElement;
        }

        /// <summary>
        /// Creates the collection class.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="collName">Name of the coll.</param>
        protected virtual void CreateCollectionClass(CodeNamespace codeNamespace, string collName)
        {
            var ctd = new CodeTypeDeclaration(collName) { IsClass = true };

            /* DCM REMOVED NOT LANGUAGE INDEPENDENT
            ctd.BaseTypes.Add(string.Format(
                                            "{0}<{1}>",
                                            GeneratorContext.GeneratorParams.CollectionBase,
                                            CollectionTypes[collName]));
            DCM REMOVED */

            //DCM Changed to Languaged independent CodeDOM
            ctd.BaseTypes.Add(new CodeTypeReference(GeneratorContext.GeneratorParams.CollectionBase, new[] { new CodeTypeReference(CollectionTypes[collName]) }));

            ctd.IsPartial = true;

            bool newCTor = false;
            var ctor = this.GetConstructor(ctd, ref newCTor);

            ctd.Members.Add(ctor);
            codeNamespace.Types.Add(ctd);
        }

        /// <summary>
        /// Creates the clone method.
        /// </summary>
        /// <param name="codeTypeDeclaration">Represents a type declaration for a class, structure, interface, or enumeration.</param>
        protected virtual void CreateCloneMethod(CodeTypeDeclaration codeTypeDeclaration)
        {
            var cloneMethod = GetCloneMethod(codeTypeDeclaration);
            cloneMethod.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Clone method"));
            cloneMethod.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Clone method"));
            codeTypeDeclaration.Members.Add(cloneMethod);
        }

        /// <summary>
        /// Creates the serialize methods.
        /// </summary>
        /// <param name="type">Represents a type declaration for a class, structure, interface, or enumeration.</param>
        protected virtual void CreateSerializeMethods(CodeTypeDeclaration type)
        {
            // Serialize
            var ser = this.CreateSerializeMethod(type);
            ser.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Serialize/Deserialize"));
            type.Members.Add(ser);

            // Deserialize
            type.Members.AddRange(this.GetOverrideDeserializeMethods(type));
            type.Members.Add(this.GetDeserializeMethod(type));

            // SaveToFile
            type.Members.AddRange(this.GetOverrideSaveToFileMethods(type));
            type.Members.Add(this.GetSaveToFileMethod());

            // LoadFromFile
            type.Members.AddRange(this.GetOverrideLoadFromFileMethods(type));
            var lff = this.GetLoadFromFileMethod(type);
            lff.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Serialize/Deserialize"));
            type.Members.Add(lff);
        }

        /// <summary>
        /// Gets the serialize CodeDOM method.
        /// </summary>
        /// <param name="type">The type object to serilize.</param>
        /// <returns>return the CodeDOM serialize method</returns>
        protected virtual CodeMemberMethod CreateSerializeMethod(CodeTypeDeclaration type)
        {
            var serializeMethod = new CodeMemberMethod
                                      {
                                          Attributes = MemberAttributes.Public,
                                          Name = GeneratorContext.GeneratorParams.SerializeMethodName
                                      };

            var tryStatmanentsCol = new CodeStatementCollection();
            var finallyStatmanentsCol = new CodeStatementCollection();

            // ------------------------------------------------------------
            // System.IO.StreamReader streamReader = null;
            // System.IO.MemoryStream memoryStream = null;
            // ------------------------------------------------------------
            serializeMethod.Statements.Add(
                    new CodeVariableDeclarationStatement(
                        new CodeTypeReference(typeof(StreamReader)),
                        "streamReader",
                        new CodePrimitiveExpression(null)));

            serializeMethod.Statements.Add(
                    new CodeVariableDeclarationStatement(
                        new CodeTypeReference(typeof(MemoryStream)),
                        "memoryStream",
                        new CodePrimitiveExpression(null)));

            tryStatmanentsCol.Add(new CodeAssignStatement(
                new CodeVariableReferenceExpression("memoryStream"),
                CodeDomHelper.CreateInstance(typeof(MemoryStream))));

            // --------------------------------------------------------------------------
            // Serializer.Serialize(memoryStream, this);
            // --------------------------------------------------------------------------
            tryStatmanentsCol.Add(
                CodeDomHelper.GetInvokeMethod(
                    "Serializer",
                    "Serialize",
                    new CodeExpression[]
                        {
                            new CodeTypeReferenceExpression("memoryStream"),
                            new CodeThisReferenceExpression()
                        }));

            // ---------------------------------------------------------------------------
            // memoryStream.Seek(0, SeekOrigin.Begin);
            // System.IO.StreamReader streamReader = new System.IO.StreamReader(memoryStream);
            // ---------------------------------------------------------------------------
            tryStatmanentsCol.Add(
                CodeDomHelper.GetInvokeMethod(
                                              "memoryStream",
                                              "Seek",
                                              new CodeExpression[]
                                                  {
                                                      new CodePrimitiveExpression(0),
                                                      new CodeTypeReferenceExpression("System.IO.SeekOrigin.Begin")
                                                  }));

            tryStatmanentsCol.Add(new CodeAssignStatement(
                new CodeVariableReferenceExpression("streamReader"),
                CodeDomHelper.CreateInstance(typeof(StreamReader), new[] { "memoryStream" })));

            var readToEnd = CodeDomHelper.GetInvokeMethod("streamReader", "ReadToEnd");
            tryStatmanentsCol.Add(new CodeMethodReturnStatement(readToEnd));

            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("streamReader"));
            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("memoryStream"));

            var tryfinallyStmt = new CodeTryCatchFinallyStatement(tryStatmanentsCol.ToArray(), new CodeCatchClause[0], finallyStatmanentsCol.ToArray());
            serializeMethod.Statements.Add(tryfinallyStmt);

            serializeMethod.ReturnType = new CodeTypeReference(typeof(string));

            // --------
            // Comments
            // --------
            serializeMethod.Comments.AddRange(
                CodeDomHelper.GetSummaryComment(string.Format("Serializes current {0} object into an XML document", type.Name)));

            serializeMethod.Comments.Add(CodeDomHelper.GetReturnComment("string XML value"));
            return serializeMethod;
        }

        /// <summary>
        /// Get Deserialize method
        /// </summary>
        /// <param name="type">represent a type declaration of class</param>
        /// <returns>Deserialize CodeMemberMethod</returns>
        protected virtual CodeMemberMethod GetDeserializeMethod(CodeTypeDeclaration type)
        {
            string deserializeTypeName = GeneratorContext.GeneratorParams.UseGenericBaseClass ? "T" : type.Name;

            // ---------------------------------------
            // public static T Deserialize(string xml)
            // ---------------------------------------
            var deserializeMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = GeneratorContext.GeneratorParams.DeserializeMethodName
            };

            deserializeMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "xml"));
            deserializeMethod.ReturnType = new CodeTypeReference(deserializeTypeName);

            deserializeMethod.Statements.Add(
                new CodeVariableDeclarationStatement(
                    new CodeTypeReference(typeof(StringReader)),
                    "stringReader",
                    new CodePrimitiveExpression(null)));

            var tryStatmanentsCol = new CodeStatementCollection();
            var finallyStatmanentsCol = new CodeStatementCollection();

            // ------------------------------------------------------
            // stringReader = new StringReader(xml);
            // ------------------------------------------------------
            var deserializeStatmanents = new CodeStatementCollection();

            tryStatmanentsCol.Add(new CodeAssignStatement(
                          new CodeVariableReferenceExpression("stringReader"),
                          new CodeObjectCreateExpression(
                              new CodeTypeReference(typeof(StringReader)),
                              new CodeExpression[] { new CodeArgumentReferenceExpression("xml") })));

            // ----------------------------------------------------------
            // obj = (ClassName)serializer.Deserialize(xmlReader);
            // return true;
            // ----------------------------------------------------------
            var deserialize = CodeDomHelper.GetInvokeMethod(
                                                            "Serializer",
                                                            "Deserialize",
                                                            new CodeExpression[]
                                                            {
                                                                CodeDomHelper.GetInvokeMethod(
                                                                "System.Xml.XmlReader", 
                                                                "Create", 
                                                                new CodeExpression[] { new CodeVariableReferenceExpression("stringReader") })
                                                            });

            var castExpr = new CodeCastExpression(deserializeTypeName, deserialize);
            var returnStmt = new CodeMethodReturnStatement(castExpr);

            tryStatmanentsCol.Add(returnStmt);
            tryStatmanentsCol.AddRange(deserializeStatmanents);

            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("stringReader"));

            var tryfinallyStmt = new CodeTryCatchFinallyStatement(tryStatmanentsCol.ToArray(), new CodeCatchClause[0], finallyStatmanentsCol.ToArray());
            deserializeMethod.Statements.Add(tryfinallyStmt);

            return deserializeMethod;
        }

        /// <summary>
        /// Get Deserialize method
        /// </summary>
        /// <param name="type">represent a type declaration of class</param>
        /// <returns>Deserialize CodeMemberMethod</returns>
        protected virtual CodeMemberMethod[] GetOverrideDeserializeMethods(CodeTypeDeclaration type)
        {
            var deserializeMethodList = new List<CodeMemberMethod>();
            string deserializeTypeName = GeneratorContext.GeneratorParams.UseGenericBaseClass ? "T" : type.Name;

            // -------------------------------------------------------------------------------------
            // public static bool Deserialize(string xml, out T obj, out System.Exception exception)
            // -------------------------------------------------------------------------------------
            var deserializeMethod = new CodeMemberMethod
                                        {
                                            Attributes = MemberAttributes.Public | MemberAttributes.Static,
                                            Name = GeneratorContext.GeneratorParams.DeserializeMethodName
                                        };

            deserializeMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "xml"));

            var param = new CodeParameterDeclarationExpression(deserializeTypeName, "obj") { Direction = FieldDirection.Out };
            deserializeMethod.Parameters.Add(param);

            param = new CodeParameterDeclarationExpression(typeof(Exception), "exception") { Direction = FieldDirection.Out };

            deserializeMethod.Parameters.Add(param);

            deserializeMethod.ReturnType = new CodeTypeReference(typeof(bool));

            // -----------------
            // exception = null;
            // -----------------
            deserializeMethod.Statements.Add(
                new CodeAssignStatement(
                    new CodeArgumentReferenceExpression("exception"),
                    new CodePrimitiveExpression(null)));

            // -----------------
            // obj = default(T);
            // -----------------
            deserializeMethod.Statements.Add(
                        new CodeAssignStatement(
                          new CodeArgumentReferenceExpression("obj"),
                            new CodeDefaultValueExpression(new CodeTypeReference(deserializeTypeName))
                            ));

            /* DCM REMOVE Switch Statement Dependent Code
            switch (GeneratorContext.GeneratorParams.Language)
            {
                case GenerationLanguage.CSharp:
                    {
                        deserializeMethod.Statements.Add(
                            new CodeAssignStatement(
                              new CodeArgumentReferenceExpression("obj"),
                                new CodeSnippetExpression(string.Format("default({0})", deserializeTypeName))));
                    }

                    break;
                case GenerationLanguage.VisualBasic:
                    {
                        deserializeMethod.Statements.Add(
                             new CodeAssignStatement(
                                new CodeArgumentReferenceExpression("obj"),
                                    new CodePrimitiveExpression(null)));
                    }

                    break;
            }
             */

            // ---------------------
            // try {...} catch {...}
            // ---------------------
            var tryStatmanentsCol = new CodeStatementCollection();

            // Call Desrialize method
            var deserializeInvoke =
                new CodeMethodInvokeExpression(
                      new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.DeserializeMethodName),
                        new CodeExpression[] { new CodeArgumentReferenceExpression("xml") });

            tryStatmanentsCol.Add(
                new CodeAssignStatement(
                    new CodeArgumentReferenceExpression("obj"),
                        deserializeInvoke));

            tryStatmanentsCol.Add(CodeDomHelper.GetReturnTrue());

            // catch
            var catchClauses = CodeDomHelper.GetCatchClause();

            var trycatch = new CodeTryCatchFinallyStatement(tryStatmanentsCol.ToArray(), catchClauses);
            deserializeMethod.Statements.Add(trycatch);

            // --------
            // Comments
            // --------
            deserializeMethod.Comments.AddRange(
                CodeDomHelper.GetSummaryComment(string.Format("Deserializes workflow markup into an {0} object", type.Name)));

            deserializeMethod.Comments.Add(CodeDomHelper.GetParamComment("xml", "string workflow markup to deserialize"));
            deserializeMethod.Comments.Add(CodeDomHelper.GetParamComment("obj", string.Format("Output {0} object", type.Name)));
            deserializeMethod.Comments.Add(CodeDomHelper.GetParamComment("exception", "output Exception value if deserialize failed"));

            deserializeMethod.Comments.Add(
                CodeDomHelper.GetReturnComment("true if this XmlSerializer can deserialize the object; otherwise, false"));
            deserializeMethodList.Add(deserializeMethod);

            // -----------------------------------------------------
            // public static bool Deserialize(string xml, out T obj)
            // -----------------------------------------------------
            deserializeMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = GeneratorContext.GeneratorParams.DeserializeMethodName
            };
            deserializeMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "xml"));
            deserializeMethod.ReturnType = new CodeTypeReference(typeof(bool));

            param = new CodeParameterDeclarationExpression(deserializeTypeName, "obj") { Direction = FieldDirection.Out };
            deserializeMethod.Parameters.Add(param);

            // ---------------------------
            // Exception exception = null;
            // ---------------------------
            deserializeMethod.Statements.Add(
            new CodeVariableDeclarationStatement(typeof(Exception), "exception", new CodePrimitiveExpression(null)));

            // ------------------------------------------------
            // return Deserialize(xml, out obj, out exception);
            // ------------------------------------------------
            var xmlStringParam = new CodeArgumentReferenceExpression("xml");
            var objParam = new CodeDirectionExpression(
                FieldDirection.Out, new CodeFieldReferenceExpression(null, "obj"));

            var expParam = new CodeDirectionExpression(
                FieldDirection.Out, new CodeFieldReferenceExpression(null, "exception"));

            deserializeInvoke =
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.DeserializeMethodName),
                    new CodeExpression[] { xmlStringParam, objParam, expParam });

            var returnStmt = new CodeMethodReturnStatement(deserializeInvoke);
            deserializeMethod.Statements.Add(returnStmt);
            deserializeMethodList.Add(deserializeMethod);
            return deserializeMethodList.ToArray();
        }

        /// <summary>
        /// Gets the save to file code DOM method.
        /// </summary>
        /// <returns>
        /// return the save to file code DOM method statment
        /// </returns>
        protected virtual CodeMemberMethod GetSaveToFileMethod()
        {
            // -----------------------------------------------
            // public virtual void SaveToFile(string fileName)
            // -----------------------------------------------
            var saveToFileMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public,
                Name = GeneratorContext.GeneratorParams.SaveToFileMethodName
            };

            saveToFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));

            saveToFileMethod.Statements.Add(
                new CodeVariableDeclarationStatement(
                    new CodeTypeReference(typeof(StreamWriter)),
                    "streamWriter",
                    new CodePrimitiveExpression(null)));

            // ------------------------
            // try {...} finally {...}
            // -----------------------
            var tryExpression = new CodeStatementCollection();

            // ------------------------------
            // string xmlString = Serialize();
            // -------------------------------
            var serializeMethodInvoke = new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.SerializeMethodName));

            var xmlString = new CodeVariableDeclarationStatement(
                new CodeTypeReference(typeof(string)), "xmlString", serializeMethodInvoke);

            tryExpression.Add(xmlString);

            // --------------------------------------------------------------
            // System.IO.FileInfo xmlFile = new System.IO.FileInfo(fileName);
            // --------------------------------------------------------------
            tryExpression.Add(CodeDomHelper.CreateObject(typeof(FileInfo), "xmlFile", new[] { "fileName" }));

            // ----------------------------------------
            // StreamWriter Tex = xmlFile.CreateText();
            // ----------------------------------------
            var createTextMethodInvoke = CodeDomHelper.GetInvokeMethod("xmlFile", "CreateText");

            tryExpression.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression("streamWriter"),
                    createTextMethodInvoke));

            // ----------------------------------
            // streamWriter.WriteLine(xmlString);
            // ----------------------------------
            var writeLineMethodInvoke =
                CodeDomHelper.GetInvokeMethod(
                                                "streamWriter",
                                                "WriteLine",
                                                new CodeExpression[]
                                                  {
                                                      new CodeVariableReferenceExpression("xmlString")
                                                  });

            tryExpression.Add(writeLineMethodInvoke);
            var closeMethodInvoke = CodeDomHelper.GetInvokeMethod("streamWriter", "Close");

            tryExpression.Add(closeMethodInvoke);

            var finallyStatmanentsCol = new CodeStatementCollection();
            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("streamWriter"));

            var trycatch = new CodeTryCatchFinallyStatement(tryExpression.ToArray(), new CodeCatchClause[0], finallyStatmanentsCol.ToArray());
            saveToFileMethod.Statements.Add(trycatch);

            return saveToFileMethod;
        }

        /// <summary>
        /// Gets the save to file code DOM method.
        /// </summary>
        /// <param name="type">CodeTypeDeclaration type.</param>
        /// <returns>
        /// return the save to file code DOM method statment
        /// </returns>
        protected virtual CodeMemberMethod[] GetOverrideSaveToFileMethods(CodeTypeDeclaration type)
        {
            var saveToFileMethodList = new List<CodeMemberMethod>();
            var saveToFileMethod = new CodeMemberMethod
                                       {
                                           Attributes = MemberAttributes.Public,
                                           Name = GeneratorContext.GeneratorParams.SaveToFileMethodName
                                       };

            saveToFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));

            var paramException = new CodeParameterDeclarationExpression(
                typeof(Exception), "exception")
                                     {
                                         Direction = FieldDirection.Out
                                     };

            saveToFileMethod.Parameters.Add(paramException);

            saveToFileMethod.ReturnType = new CodeTypeReference(typeof(bool));

            saveToFileMethod.Statements.Add(
                new CodeAssignStatement(new CodeArgumentReferenceExpression("exception"), new CodePrimitiveExpression(null)));

            // ---------------------
            // try {...} catch {...}
            // ---------------------
            var tryExpression = new CodeStatementCollection();

            // ---------------------
            // SaveToFile(fileName);
            // ---------------------
            var xmlStringParam = new CodeArgumentReferenceExpression("fileName");

            var saveToFileInvoke =
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.SaveToFileMethodName),
                    new CodeExpression[] { xmlStringParam });

            tryExpression.Add(saveToFileInvoke);
            tryExpression.Add(CodeDomHelper.GetReturnTrue());

            // -----------
            // Catch {...}
            // -----------
            var catchstmts = new CodeStatementCollection();
            catchstmts.Add(new CodeAssignStatement(new CodeArgumentReferenceExpression("exception"), new CodeVariableReferenceExpression("e")));

            catchstmts.Add(CodeDomHelper.GetReturnFalse());
            var codeCatchClause = new CodeCatchClause("e", new CodeTypeReference(typeof(Exception)), catchstmts.ToArray());

            var codeCatchClauses = new[] { codeCatchClause };

            var trycatch = new CodeTryCatchFinallyStatement(tryExpression.ToArray(), codeCatchClauses);
            saveToFileMethod.Statements.Add(trycatch);

            saveToFileMethod.Comments.AddRange(
                CodeDomHelper.GetSummaryComment(string.Format("Serializes current {0} object into file", type.Name)));

            saveToFileMethod.Comments.Add(CodeDomHelper.GetParamComment("fileName", "full path of outupt xml file"));
            saveToFileMethod.Comments.Add(CodeDomHelper.GetParamComment("exception", "output Exception value if failed"));
            saveToFileMethod.Comments.Add(CodeDomHelper.GetReturnComment("true if can serialize and save into file; otherwise, false"));

            saveToFileMethodList.Add(saveToFileMethod);
            return saveToFileMethodList.ToArray();
        }

        /// <summary>
        /// Gets the load from file CodeDOM method.
        /// </summary>
        /// <param name="type">The type CodeTypeDeclaration.</param>
        /// <returns>return the codeDom LoadFromFile method</returns>
        protected virtual CodeMemberMethod GetLoadFromFileMethod(CodeTypeDeclaration type)
        {
            string typeName = GeneratorContext.GeneratorParams.UseGenericBaseClass ? "T" : type.Name;

            // ---------------------------------------------
            // public static T LoadFromFile(string fileName)
            // ---------------------------------------------
            var loadFromFileMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = GeneratorContext.GeneratorParams.LoadFromFileMethodName
            };

            loadFromFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));
            loadFromFileMethod.ReturnType = new CodeTypeReference(typeName);

            loadFromFileMethod.Statements.Add(
                new CodeVariableDeclarationStatement(
                        new CodeTypeReference(typeof(FileStream)),
                        "file",
                        new CodePrimitiveExpression(null)));

            loadFromFileMethod.Statements.Add(
                new CodeVariableDeclarationStatement(
                        new CodeTypeReference(typeof(StreamReader)),
                        "sr",
                        new CodePrimitiveExpression(null)));

            var tryStatmanentsCol = new CodeStatementCollection();
            var finallyStatmanentsCol = new CodeStatementCollection();

            // ---------------------------------------------------------------------------
            // file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            // sr = new StreamReader(file);
            // ---------------------------------------------------------------------------
            tryStatmanentsCol.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression("file"),
                    new CodeObjectCreateExpression(
                        typeof(FileStream),
                        new CodeExpression[]
                        {
                            new CodeArgumentReferenceExpression("fileName"),
                            CodeDomHelper.GetEnum("FileMode","Open"),
                            CodeDomHelper.GetEnum("FileAccess","Read")
                        })));

            tryStatmanentsCol.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression("sr"),
                    new CodeObjectCreateExpression(
                        typeof(StreamReader),
                        new CodeExpression[]
                        {
                            new CodeVariableReferenceExpression("file"),
                        })));

            // ----------------------------------
            // string xmlString = sr.ReadToEnd();
            // ----------------------------------
            var readToEndInvoke = CodeDomHelper.GetInvokeMethod("sr", "ReadToEnd");

            var xmlString = new CodeVariableDeclarationStatement(
                new CodeTypeReference(typeof(string)), "xmlString", readToEndInvoke);

            tryStatmanentsCol.Add(xmlString);
            tryStatmanentsCol.Add(CodeDomHelper.GetInvokeMethod("sr", "Close"));
            tryStatmanentsCol.Add(CodeDomHelper.GetInvokeMethod("file", "Close"));

            // ------------------------------------------------------
            // return Deserialize(xmlString, out obj, out exception);
            // ------------------------------------------------------            
            var fileName = new CodeVariableReferenceExpression("xmlString");

            var deserializeInvoke =
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.DeserializeMethodName),
                    new CodeExpression[] { fileName });

            var rstmts = new CodeMethodReturnStatement(deserializeInvoke);
            tryStatmanentsCol.Add(rstmts);

            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("file"));
            finallyStatmanentsCol.Add(CodeDomHelper.GetDispose("sr"));

            var tryfinally = new CodeTryCatchFinallyStatement(
                CodeDomHelper.CodeStmtColToArray(tryStatmanentsCol), new CodeCatchClause[0], CodeDomHelper.CodeStmtColToArray(finallyStatmanentsCol));

            loadFromFileMethod.Statements.Add(tryfinally);

            return loadFromFileMethod;
        }

        /// <summary>
        /// Gets the load from file CodeDOM method.
        /// </summary>
        /// <param name="type">The type CodeTypeDeclaration.</param>
        /// <returns>return the codeDom LoadFromFile method</returns>
        protected virtual CodeMemberMethod[] GetOverrideLoadFromFileMethods(CodeTypeDeclaration type)
        {
            string typeName = GeneratorContext.GeneratorParams.UseGenericBaseClass ? "T" : type.Name;

            CodeTypeReference teeType = new CodeTypeReference(typeName);
            
            var saveToFileMethodList = new List<CodeMemberMethod>();
            var loadFromFileMethod = new CodeMemberMethod
                                         {
                                             Attributes = MemberAttributes.Public | MemberAttributes.Static,
                                             Name = GeneratorContext.GeneratorParams.LoadFromFileMethodName
                                         };

            loadFromFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));

            var param = new CodeParameterDeclarationExpression(typeName, "obj") { Direction = FieldDirection.Out };
            loadFromFileMethod.Parameters.Add(param);

            param = new CodeParameterDeclarationExpression(typeof(Exception), "exception") { Direction = FieldDirection.Out };

            loadFromFileMethod.Parameters.Add(param);
            loadFromFileMethod.ReturnType = new CodeTypeReference(typeof(bool));

            // -----------------
            // exception = null;
            // obj = null;
            // -----------------
            loadFromFileMethod.Statements.Add(
                new CodeAssignStatement(new CodeArgumentReferenceExpression("exception"), new CodePrimitiveExpression(null)));

            loadFromFileMethod.Statements.Add(
                new CodeAssignStatement(new CodeArgumentReferenceExpression("obj"), new CodeDefaultValueExpression(teeType)));

            var tryStatmanentsCol = new CodeStatementCollection();

            // Call LoadFromFile method
            var loadFromFileInvoke =
                new CodeMethodInvokeExpression(
                      new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.LoadFromFileMethodName),
                        new CodeExpression[] { new CodeArgumentReferenceExpression("fileName") });

            tryStatmanentsCol.Add(
                new CodeAssignStatement(
                    new CodeArgumentReferenceExpression("obj"),
                        loadFromFileInvoke));

            tryStatmanentsCol.Add(CodeDomHelper.GetReturnTrue());

            var trycatch = new CodeTryCatchFinallyStatement(
                CodeDomHelper.CodeStmtColToArray(tryStatmanentsCol), CodeDomHelper.GetCatchClause());

            loadFromFileMethod.Statements.Add(trycatch);

            loadFromFileMethod.Comments.AddRange(
                CodeDomHelper.GetSummaryComment(
                    string.Format("Deserializes xml markup from file into an {0} object", type.Name)));

            loadFromFileMethod.Comments.Add(CodeDomHelper.GetParamComment("fileName", "string xml file to load and deserialize"));
            loadFromFileMethod.Comments.Add(CodeDomHelper.GetParamComment("obj", string.Format("Output {0} object", type.Name)));
            loadFromFileMethod.Comments.Add(CodeDomHelper.GetParamComment("exception", "output Exception value if deserialize failed"));

            loadFromFileMethod.Comments.Add(
                CodeDomHelper.GetReturnComment("true if this XmlSerializer can deserialize the object; otherwise, false"));

            saveToFileMethodList.Add(loadFromFileMethod);

            // ------------------------------------------------------
            // public static bool LoadFromFile(string xml, out T obj)
            // ------------------------------------------------------
            loadFromFileMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = GeneratorContext.GeneratorParams.LoadFromFileMethodName
            };
            loadFromFileMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));
            loadFromFileMethod.ReturnType = new CodeTypeReference(typeof(bool));

            param = new CodeParameterDeclarationExpression(typeName, "obj") { Direction = FieldDirection.Out };
            loadFromFileMethod.Parameters.Add(param);

            // ---------------------------
            // Exception exception = null;
            // ---------------------------
            loadFromFileMethod.Statements.Add(
            new CodeVariableDeclarationStatement(typeof(Exception), "exception", new CodePrimitiveExpression(null)));

            // ------------------------------------------------------
            // return LoadFromFile(fileName, out obj, out exception);
            // ------------------------------------------------------
            var fileName = new CodeArgumentReferenceExpression("fileName");
            var objParam = new CodeDirectionExpression(
                FieldDirection.Out, new CodeFieldReferenceExpression(null, "obj"));

            var expParam = new CodeDirectionExpression(
                FieldDirection.Out, new CodeFieldReferenceExpression(null, "exception"));

            var loadFromFileMethodInvok =
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(null, GeneratorContext.GeneratorParams.LoadFromFileMethodName),
                    new CodeExpression[] { fileName, objParam, expParam });

            var returnStmt = new CodeMethodReturnStatement(loadFromFileMethodInvok);
            loadFromFileMethod.Statements.Add(returnStmt);
            saveToFileMethodList.Add(loadFromFileMethod);
            return saveToFileMethodList.ToArray();
        }

        /// <summary>
        /// Import namespaces
        /// </summary>
        /// <param name="code">Code namespace</param>
        protected virtual void ImportNamespaces(CodeNamespace code)
        {
            code.Imports.Add(new CodeNamespaceImport("System"));
            code.Imports.Add(new CodeNamespaceImport("System.Diagnostics"));
            code.Imports.Add(new CodeNamespaceImport("System.Xml.Serialization"));
            code.Imports.Add(new CodeNamespaceImport("System.Collections"));
            code.Imports.Add(new CodeNamespaceImport("System.Xml.Schema"));
            code.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));

            if (GeneratorContext.GeneratorParams.CustomUsings != null)
            {
                foreach (var item in GeneratorContext.GeneratorParams.CustomUsings)
                    code.Imports.Add(new CodeNamespaceImport(item.NameSpace));
            }

            if (GeneratorContext.GeneratorParams.IncludeSerializeMethod)
                code.Imports.Add(new CodeNamespaceImport("System.IO"));

            switch (GeneratorContext.GeneratorParams.CollectionObjectType)
            {
                case CollectionType.List:
                    code.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
                    break;
                case CollectionType.ObservableCollection:
                    code.Imports.Add(new CodeNamespaceImport("System.Collections.ObjectModel"));
                    break;
                default:
                    break;
            }

            code.Name = GeneratorContext.GeneratorParams.NameSpace;
        }

        /// <summary>
        /// Create data contract attribute
        /// </summary>
        /// <param name="type">Code type declaration</param>
        /// <param name="schema">XML schema</param>
        protected virtual void CreateDataContractAttribute(CodeTypeDeclaration type, XmlSchema schema)
        {
            // abstract
        }

        /// <summary>
        /// Creates the data member attribute.
        /// </summary>
        /// <param name="prop">Represents a declaration for a property of a type.</param>
        protected virtual void CreateDataMemberAttribute(CodeMemberProperty prop)
        {
            // abstract
        }

        /// <summary>
        /// Recursive search of elemement.
        /// </summary>
        /// <param name="type">Element to search</param>
        /// <param name="xmlElement">Current element</param>
        /// <param name="currentElementName">Name of the current element.</param>
        /// <param name="hierarchicalElmtName">The hierarchical Elmt Name.</param>
        /// <returns>
        /// return found XmlSchemaElement or null value
        /// </returns>
        protected virtual XmlSchemaElement SearchElement(CodeTypeDeclaration type, XmlSchemaElement xmlElement, string currentElementName, string hierarchicalElmtName)
        {
            var found = false;
            if (type.IsClass)
            {
                if (xmlElement.Name == null)
                    return null;

                if (type.Name.Equals(hierarchicalElmtName + xmlElement.Name) ||
                    type.Name.Equals(xmlElement.Name))
                    found = true;
            }
            else
            {
                if (type.Name.Equals(xmlElement.QualifiedName.Name))
                    found = true;
            }

            if (found)
                return xmlElement;

            var xmlComplexType = xmlElement.ElementSchemaType as XmlSchemaComplexType;
            if (xmlComplexType != null)
            {
                var xmlSequence = xmlComplexType.ContentTypeParticle as XmlSchemaSequence;
                if (xmlSequence != null)
                {
                    foreach (XmlSchemaObject item in xmlSequence.Items)
                    {
                        var currentXmlSchemaElement = item as XmlSchemaElement;
                        if (currentXmlSchemaElement == null)
                            continue;

                        if (hierarchicalElmtName == xmlElement.QualifiedName.Name ||
                            currentElementName == xmlElement.QualifiedName.Name)
                            return null;

                        XmlSchemaElement subItem = this.SearchElement(
                                                                      type,
                                                                      currentXmlSchemaElement,
                                                                      xmlElement.QualifiedName.Name,
                                                                      hierarchicalElmtName + xmlElement.QualifiedName.Name);
                        if (subItem != null)
                            return subItem;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Create CodeCommentStatement from schema documentation.
        /// </summary>
        /// <param name="codeStatmentColl">CodeCommentStatementCollection collection</param>
        /// <param name="xmlDoc">Schema documentation</param>
        protected virtual void CreateCommentStatment(
                                                     CodeCommentStatementCollection codeStatmentColl,
                                                     XmlSchemaDocumentation xmlDoc)
        {
            codeStatmentColl.Clear();
            foreach (XmlNode itemDoc in xmlDoc.Markup)
            {
                var textLine = itemDoc.InnerText.Trim();
                if (textLine.Length > 0)
                    CodeDomHelper.CreateSummaryComment(codeStatmentColl, textLine);
            }
        }

        /// <summary>
        /// Field process.
        /// </summary>
        /// <param name="member">CodeTypeMember member</param>
        /// <param name="ctor">CodeMemberMethod constructor</param>
        /// <param name="ns">CodeNamespace XSD</param>
        /// <param name="addedToConstructor">Indicates if create a new constructor</param>
        protected virtual void ProcessFields(
                                            CodeTypeMember member,
                                            CodeMemberMethod ctor,
                                            CodeNamespace ns,
                                            ref bool addedToConstructor)
        {
            var field = (CodeMemberField)member;

            // ---------------------------------------------
            // [EditorBrowsable(EditorBrowsableState.Never)]
            // ---------------------------------------------
            if (member.Attributes == MemberAttributes.Private)
            {
                if (GeneratorContext.GeneratorParams.HidePrivateFieldInIde)
                {
                    var attributeType = new CodeTypeReference(
                        typeof(EditorBrowsableAttribute).Name.Replace("Attribute", string.Empty));

                    var argument = new CodeAttributeArgument
                                       {
                                           //Value = new CodePropertyReferenceExpression(
                                               //new CodeSnippetExpression(typeof(EditorBrowsableState).Name), "Never")
                                           Value = CodeDomHelper.GetEnum(typeof(EditorBrowsableState).Name,"Never")
                                       };

                    field.CustomAttributes.Add(new CodeAttributeDeclaration(attributeType, new[] { argument }));
                }
            }

            // ------------------------------------------
            // protected virtual  List <Actor> nameField;
            // ------------------------------------------
            var thisIsCollectionType = field.Type.ArrayElementType != null;
            if (thisIsCollectionType)
            {
                field.Type = this.GetCollectionType(field.Type);
            }

            // ---------------------------------------
            // if ((this.nameField == null))
            // {
            //    this.nameField = new List<Name>();
            // }
            // ---------------------------------------
            if (GeneratorContext.GeneratorParams.EnableInitializeFields && GeneratorContext.GeneratorParams.CollectionObjectType != CollectionType.Array)
            {
                CodeTypeDeclaration declaration = this.FindTypeInNamespace(field.Type.BaseType, ns);
                if (thisIsCollectionType ||
                     (((declaration != null) && declaration.IsClass)
                      && ((declaration.TypeAttributes & TypeAttributes.Abstract) != TypeAttributes.Abstract)))
                {
                    if (GeneratorContext.GeneratorParams.EnableLasyLoading)
                    {
                        lasyLoadingFields.Add(field.Name);
                    }
                    else
                    {
                        ctor.Statements.Insert(0, this.CreateInstance(field.Name, field.Type));
                        addedToConstructor = true;
                    }
                }
            }
        }

        /// <summary>
        /// Create a Class Constructor
        /// </summary>
        /// <param name="type">type of declaration</param>
        /// <returns>return CodeConstructor</returns>
        protected virtual CodeConstructor CreateClassConstructor(CodeTypeDeclaration type)
        {
            var ctor = new CodeConstructor { Attributes = MemberAttributes.Public, Name = type.Name };
            return ctor;
        }

        /// <summary>
        /// Create new instance of object
        /// </summary>
        /// <param name="name">Name of object</param>
        /// <param name="type">CodeTypeReference Type</param>
        /// <returns>return instance CodeConditionStatement</returns>
        protected virtual CodeConditionStatement CreateInstanceIfNotNull(string name, CodeTypeReference type)
        {

            CodeAssignStatement statement;
            if (type.BaseType.Equals("System.String") && type.ArrayRank == 0)
            {
                statement =
                    new CodeAssignStatement(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name),
                        //new CodeSnippetExpression("String.Empty"));
                        CodeDomHelper.GetStaticField(typeof(String), "Empty"));
            }
            else{
            statement =
                new CodeAssignStatement(
                                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name),
                                        new CodeObjectCreateExpression(type, new CodeExpression[0]));
            }

            return
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name),
                        CodeBinaryOperatorType.IdentityEquality,
                        new CodePrimitiveExpression(null)),
                        new CodeStatement[] { statement });
        }

        /// <summary>
        /// Create new instance of object
        /// </summary>
        /// <param name="name">Name of object</param>
        /// <param name="type">CodeTypeReference Type</param>
        /// <returns>return instance CodeConditionStatement</returns>
        protected virtual CodeAssignStatement CreateInstance(string name, CodeTypeReference type)
        {
            //return new CodeAssignStatement(
            //                            new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name),
            //                            new CodeObjectCreateExpression(type, new CodeExpression[0]));


            CodeAssignStatement statement;
            if (type.BaseType.Equals("System.String") && type.ArrayRank == 0)
            {
                statement =
                new CodeAssignStatement(
                                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name),
                                        //new CodeSnippetExpression("String.Empty"));
                                        CodeDomHelper.GetStaticField(typeof(String), "Empty"));
            }
            else
            {
                statement =
                    new CodeAssignStatement(
                                            new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name),
                                            new CodeObjectCreateExpression(type, new CodeExpression[0]));
            }
            return statement;
        }

        /// <summary>
        /// Recherche le CodeTypeDeclaration d'un objet en fonction de son type de base (nom de classe)
        /// </summary>
        /// <param name="typeName">Search name</param>
        /// <param name="ns">Seach into</param>
        /// <returns>CodeTypeDeclaration found</returns>
        protected virtual CodeTypeDeclaration FindTypeInNamespace(string typeName, CodeNamespace ns)
        {
            foreach (CodeTypeDeclaration declaration in ns.Types)
            {
                if (declaration.Name == typeName)
                    return declaration;
            }

            return null;
        }

        /// <summary>
        /// Property process
        /// </summary>
        /// <param name="type">Represents a type declaration for a class, structure, interface, or enumeration</param>
        /// <param name="ns">The ns.</param>
        /// <param name="member">Type members include fields, methods, properties, constructors and nested types</param>
        /// <param name="xmlElement">Represent the root element in schema</param>
        /// <param name="schema">XML Schema</param>
        protected virtual void ProcessProperty(CodeTypeDeclaration type, CodeNamespace ns, CodeTypeMember member, XmlSchemaElement xmlElement, XmlSchema schema)
        {
            if (GeneratorContext.GeneratorParams.EnableSummaryComment)
            {
                if (xmlElement != null)
                {
                    var xmlComplexType = xmlElement.ElementSchemaType as XmlSchemaComplexType;
                    bool foundInAttributes = false;
                    if (xmlComplexType != null)
                    {
                        // Search property in attributes for summary comment generation
                        foreach (XmlSchemaObject attribute in xmlComplexType.Attributes)
                        {
                            var xmlAttrib = attribute as XmlSchemaAttribute;
                            if (xmlAttrib != null)
                            {
                                if (member.Name.Equals(xmlAttrib.QualifiedName.Name))
                                {
                                    this.CreateCommentFromAnnotation(xmlAttrib.Annotation, member.Comments);
                                    foundInAttributes = true;
                                }
                            }
                        }

                        // Search property in XmlSchemaElement for summary comment generation
                        if (!foundInAttributes)
                        {
                            var xmlSequence = xmlComplexType.ContentTypeParticle as XmlSchemaSequence;
                            if (xmlSequence != null)
                            {
                                foreach (XmlSchemaObject item in xmlSequence.Items)
                                {
                                    var currentItem = item as XmlSchemaElement;
                                    if (currentItem != null)
                                    {
                                        if (member.Name.Equals(currentItem.QualifiedName.Name))
                                            this.CreateCommentFromAnnotation(currentItem.Annotation, member.Comments);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var prop = (CodeMemberProperty)member;

            if (prop.Type.ArrayElementType != null)
            {
                prop.Type = this.GetCollectionType(prop.Type);
                collectionTypesFields.Add(prop.Name);
            }

            if (GeneratorContext.GeneratorParams.GenerateDataContracts)
            {
                this.CreateDataMemberAttribute(prop);
            }

            // Lasy loading
            if (GeneratorContext.GeneratorParams.EnableInitializeFields)
            {
                var propReturnStatment = prop.GetStatements[0] as CodeMethodReturnStatement;
                if (propReturnStatment != null)
                {
                    var field = propReturnStatment.Expression as CodeFieldReferenceExpression;
                    if (field != null)
                    {
                        if (lasyLoadingFields.IndexOf(field.FieldName) != -1)
                        {
                            prop.GetStatements.Insert(0, this.CreateInstanceIfNotNull(field.FieldName, prop.Type));
                        }
                    }
                }
            }

            // Add OnPropertyChanged in setter
            if (GeneratorContext.GeneratorParams.EnableDataBinding)
            {
                if (type.BaseTypes.IndexOf(new CodeTypeReference(typeof(CollectionBase))) == -1)
                {
                    // -----------------------------
                    // if (handler != null) {
                    //    OnPropertyChanged("Name");
                    // -----------------------------
                    var propChange =
                        new CodeMethodInvokeExpression(
                            new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "OnPropertyChanged"),
                            new CodeExpression[] { new CodePrimitiveExpression( prop.Name ) });
                    
                    var propAssignStatment = prop.SetStatements[0] as CodeAssignStatement;
                    if (propAssignStatment != null)
                    {
                        var cfreL = propAssignStatment.Left as CodeFieldReferenceExpression;
                        var cfreR = propAssignStatment.Right as CodePropertySetValueReferenceExpression;

                        if (cfreL != null)
                        {
                            var setValueCondition = new CodeStatementCollection { propAssignStatment, propChange };

                            // ---------------------------------------------
                            // if ((xxxField.Equals(value) != true)) { ... }
                            // ---------------------------------------------
                            var condStatmentCondEquals = new CodeConditionStatement(
                                new CodeBinaryOperatorExpression(
                                    new CodeMethodInvokeExpression(
                                        new CodeFieldReferenceExpression(
                                            null,
                                            cfreL.FieldName),
                                        "Equals",
                                        cfreR),
                                    CodeBinaryOperatorType.IdentityInequality,
                                    new CodePrimitiveExpression(true)),
                                CodeDomHelper.CodeStmtColToArray(setValueCondition));

                            // ---------------------------------------------
                            // if ((xxxField != null)) { ... }
                            // ---------------------------------------------
                            var condStatmentCondNotNull = new CodeConditionStatement(
                                new CodeBinaryOperatorExpression(
                                    new CodeFieldReferenceExpression(
                                        new CodeThisReferenceExpression(), cfreL.FieldName),
                                        CodeBinaryOperatorType.IdentityInequality,
                                        new CodePrimitiveExpression(null)),
                                        new CodeStatement[] { condStatmentCondEquals },
                                        CodeDomHelper.CodeStmtColToArray(setValueCondition));

                            var property = member as CodeMemberProperty;
                            if (property != null)
                            {
                                if (property.Type.BaseType != new CodeTypeReference(typeof(long)).BaseType &&
                                    property.Type.BaseType != new CodeTypeReference(typeof(DateTime)).BaseType &&
                                    property.Type.BaseType != new CodeTypeReference(typeof(float)).BaseType &&
                                    property.Type.BaseType != new CodeTypeReference(typeof(double)).BaseType &&
                                    property.Type.BaseType != new CodeTypeReference(typeof(int)).BaseType &&
                                    property.Type.BaseType != new CodeTypeReference(typeof(bool)).BaseType &&
                                    enumListField.IndexOf(property.Type.BaseType) == -1)
                                    prop.SetStatements[0] = condStatmentCondNotNull;
                                else
                                    prop.SetStatements[0] = condStatmentCondEquals;
                            }
                        }
                        else
                            prop.SetStatements.Add(propChange);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether [is complex type] [the specified code type reference].
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="codeTypeReference">The code type reference.</param>
        /// <param name="ns">The ns.</param>
        /// <returns>
        /// true if type is complex type (class, List, etc.)"/&gt;
        /// </returns>
        protected bool IsComplexType(CodeTypeReference codeTypeReference, CodeNamespace ns)
        {
            CodeTypeDeclaration declaration = this.FindTypeInNamespace(codeTypeReference.BaseType, ns);
            return ((declaration != null) && declaration.IsClass) &&
                   ((declaration.TypeAttributes & TypeAttributes.Abstract) != TypeAttributes.Abstract);
        }

        /// <summary>
        /// Removes the default XML attributes.
        /// </summary>
        /// <param name="customAttributes">
        /// The custom Attributes.
        /// </param>
        protected virtual void RemoveDefaultXmlAttributes(CodeAttributeDeclarationCollection customAttributes)
        {
            var codeAttributes = new List<CodeAttributeDeclaration>();
            foreach (var attribute in customAttributes)
            {
                var attrib = attribute as CodeAttributeDeclaration;
                if (attrib == null)
                {
                    continue;
                }

                if (attrib.Name == "System.Xml.Serialization.XmlAttributeAttribute" ||
                    attrib.Name == "System.Xml.Serialization.XmlIgnoreAttribute" ||
                    attrib.Name == "System.Xml.Serialization.XmlTypeAttribute" ||
                    attrib.Name == "System.Xml.Serialization.XmlElementAttribute" ||
                    attrib.Name == "System.CodeDom.Compiler.GeneratedCodeAttribute" ||
                    attrib.Name == "System.Xml.Serialization.XmlRootAttribute")
                {
                    codeAttributes.Add(attrib);
                }
            }

            foreach (var item in codeAttributes)
            {
                customAttributes.Remove(item);
            }
        }

        /// <summary>
        /// Removes the debug attributes.
        /// </summary>
        /// <param name="customAttributes">The custom attributes Collection.</param>
        protected virtual void RemoveDebugAttributes(CodeAttributeDeclarationCollection customAttributes)
        {
            var codeAttributes = new List<CodeAttributeDeclaration>();
            foreach (var attribute in customAttributes)
            {
                var attrib = attribute as CodeAttributeDeclaration;
                if (attrib == null)
                {
                    continue;
                }

                if (attrib.Name == "System.Diagnostics.DebuggerStepThroughAttribute")
                {
                    codeAttributes.Add(attrib);
                }
            }
            //DCM: OK not sure why it in this loop other than its like a transaction.
            //Not going to touch it now.
            foreach (var item in codeAttributes)
            {
                customAttributes.Remove(item);
            }
        }

        /// <summary>
        /// Generate summary comment from XmlSchemaAnnotation 
        /// </summary>
        /// <param name="xmlSchemaAnnotation">XmlSchemaAnnotation from XmlSchemaElement or XmlSchemaAttribute</param>
        /// <param name="codeCommentStatementCollection">codeCommentStatementCollection from member</param>
        protected virtual void CreateCommentFromAnnotation(XmlSchemaAnnotation xmlSchemaAnnotation, CodeCommentStatementCollection codeCommentStatementCollection)
        {
            if (xmlSchemaAnnotation != null)
            {
                foreach (XmlSchemaObject annotation in xmlSchemaAnnotation.Items)
                {
                    var xmlDoc = annotation as XmlSchemaDocumentation;
                    if (xmlDoc != null)
                    {
                        this.CreateCommentStatment(codeCommentStatementCollection, xmlDoc);
                    }
                }
            }
        }

        /// <summary>
        /// Get CodeTypeReference for collection
        /// </summary>
        /// <param name="codeType">The code Type.</param>
        /// <returns>return array of or genereric collection</returns>
        protected virtual CodeTypeReference GetCollectionType(CodeTypeReference codeType)
        {
            CodeTypeReference collectionType = codeType;
            if (codeType.BaseType == typeof(byte).FullName)
            {
                // Never change byte[] to List<byte> etc.
                // Fix : when translating hexBinary and base64Binary 
                return codeType;
            }

            switch (GeneratorContext.GeneratorParams.CollectionObjectType)
            {
                case CollectionType.List:
                    collectionType = new CodeTypeReference("List", new[] { new CodeTypeReference(codeType.BaseType) });
                    break;

                case CollectionType.BindingList:
                    collectionType = new CodeTypeReference("BindingList", new[] { new CodeTypeReference(codeType.BaseType) });
                    break;

                case CollectionType.ObservableCollection:
                    collectionType = new CodeTypeReference("ObservableCollection", new[] { new CodeTypeReference(codeType.BaseType) });
                    break;

                case CollectionType.DefinedType:
                    string typname = codeType.BaseType.Replace(".", string.Empty) + "Collection";

                    if (!CollectionTypes.Keys.Contains(typname))
                        CollectionTypes.Add(typname, codeType.BaseType);

                    collectionType = new CodeTypeReference(typname);
                    break;
                default:
                    {
                        // If not use generics, remove multiple array Ex. string[][] => string[]
                        // Fix : http://xsd2code.codeplex.com/WorkItem/View.aspx?WorkItemId=7269
                        if (codeType.ArrayElementType.ArrayRank > 0)
                            collectionType.ArrayElementType.ArrayRank = 0;
                    }

                    break;
            }

            return collectionType;
        }

        /// <summary>
        /// Search defaut constructor. If not exist, create a new ctor.
        /// </summary>
        /// <param name="type">CodeTypeDeclaration type</param>
        /// <param name="newCTor">Indicates if new constructor</param>
        /// <returns>return current or new CodeConstructor</returns>
        protected virtual CodeConstructor GetConstructor(CodeTypeDeclaration type, ref bool newCTor)
        {
            CodeConstructor ctor = null;
            foreach (CodeTypeMember member in type.Members)
            {
                if (member is CodeConstructor)
                    ctor = member as CodeConstructor;
            }

            if (ctor == null)
            {
                newCTor = true;
                ctor = this.CreateClassConstructor(type);
            }

            if (GeneratorContext.GeneratorParams.EnableSummaryComment)
                CodeDomHelper.CreateSummaryComment(ctor.Comments, string.Format("{0} class constructor", ctor.Name));

            return ctor;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Creates the static serializer.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        private static void CreateStaticSerializer(CodeTypeDeclaration classType)
        {
            string typeName = GeneratorContext.GeneratorParams.UseGenericBaseClass ? "T" : classType.Name;


            //VB is not Case Sensitive
            string fieldName = GeneratorContext.GeneratorParams.Language == GenerationLanguage.VisualBasic ?  "sSerializer" : "serializer" ;

            // -----------------------------------------------------------------
            // private static System.Xml.Serialization.XmlSerializer serializer;
            // -----------------------------------------------------------------
            var serializerfield = new CodeMemberField(typeof(XmlSerializer), fieldName);
            serializerfield.Attributes = MemberAttributes.Static | MemberAttributes.Private;
            classType.Members.Add(serializerfield);

            var typeRef = new CodeTypeReference(typeName);
            var typeofValue = new CodeTypeOfExpression(typeRef);

            // private static System.Xml.Serialization.XmlSerializer Serializer { get {...} }
            var serializerProperty = new CodeMemberProperty();
            serializerProperty.Type = new CodeTypeReference(typeof(XmlSerializer));
            serializerProperty.Name = "Serializer";

            serializerProperty.HasSet = false;
            serializerProperty.HasGet = true;
            serializerProperty.Attributes = MemberAttributes.Static | MemberAttributes.Private;

            var statments = new CodeStatementCollection();

            statments.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression(fieldName),
                    new CodeObjectCreateExpression(
                        new CodeTypeReference(typeof(XmlSerializer)), new CodeExpression[] { typeofValue })));


            serializerProperty.GetStatements.Add(
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeVariableReferenceExpression(fieldName),
                        CodeBinaryOperatorType.IdentityEquality,
                        new CodePrimitiveExpression(null)),
                        statments.ToArray()));


            serializerProperty.GetStatements.Add(
                new CodeMethodReturnStatement(new CodeVariableReferenceExpression(fieldName)));

            classType.Members.Add(serializerProperty);
        }

        /// <summary>
        /// Generates the base class.
        /// </summary>
        /// <returns>Return base class codetype declaration</returns>
        private CodeTypeDeclaration GenerateBaseClass()
        {
            var baseClass = new CodeTypeDeclaration(GeneratorContext.GeneratorParams.BaseClassName)
                                {
                                    IsClass = true,
                                    IsPartial = true,
                                    TypeAttributes = TypeAttributes.Public
                                };

            baseClass.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Base entity class"));
            baseClass.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Base entity class"));

            if (GeneratorContext.GeneratorParams.EnableDataBinding)
                baseClass.BaseTypes.Add(typeof(INotifyPropertyChanged));

            baseClass.TypeParameters.Add(new CodeTypeParameter("T"));

            if (GeneratorContext.GeneratorParams.EnableDataBinding)
                this.CreateDataBinding(baseClass);

            if (GeneratorContext.GeneratorParams.IncludeSerializeMethod)
            {
                CreateStaticSerializer(baseClass);
                this.CreateSerializeMethods(baseClass);
            }

            if (GeneratorContext.GeneratorParams.GenerateCloneMethod)
                this.CreateCloneMethod(baseClass);

            return baseClass;
        }

        /// <summary>
        /// Search XmlElement in schema.
        /// </summary>
        /// <param name="codeTypeDeclaration">Represents a type declaration for a class, structure, interface, or enumeration.</param>
        /// <param name="schema">schema object</param>
        /// <param name="visitedSchemas">The visited schemas.</param>
        /// <returns>
        /// return found XmlSchemaElement or null value
        /// </returns>
        private XmlSchemaElement SearchElementInSchema(CodeTypeDeclaration codeTypeDeclaration, XmlSchema schema, List<XmlSchema> visitedSchemas)
        {
            foreach (var item in schema.Items)
            {
                var xmlElement = item as XmlSchemaElement;
                if (xmlElement == null)
                {
                    continue;
                }

                var xmlSubElement = this.SearchElement(codeTypeDeclaration, xmlElement, string.Empty, string.Empty);
                if (xmlSubElement != null) return xmlSubElement;
            }

            // If not found search in schema inclusion
            foreach (var item in schema.Includes)
            {
                var schemaInc = item as XmlSchemaInclude;

                // avoid to follow cyclic refrence
                if ((schemaInc == null) || visitedSchemas.Exists(loc => schemaInc.Schema == loc))
                    continue;

                visitedSchemas.Add(schemaInc.Schema);
                var includeElmts = this.SearchElementInSchema(codeTypeDeclaration, schemaInc.Schema, visitedSchemas);
                visitedSchemas.Remove(schemaInc.Schema);

                if (includeElmts != null) return includeElmts;
            }

            return null;
        }
        #endregion
    }
}
