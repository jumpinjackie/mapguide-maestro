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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Maestro.Editors.Generic;
using System.IO;
using OSGeo.MapGuide.MaestroAPI.Resource;
using ICSharpCode.Core;
using OSGeo.MapGuide.MaestroAPI;
using System.Xml;
using Maestro.Editors;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using System.Xml.Schema;
using Maestro.Base.UI.Preferences;

namespace Maestro.Base.Editor
{
    public partial class XmlEditor : EditorContentBase
    {
        public XmlEditor()
        {
            InitializeComponent();
            editor.Validator = new XmlValidationCallback(ValidateXml);
            this.XsdPath = PropertyService.Get(ConfigProperties.XsdSchemaPath, ConfigProperties.DefaultXsdSchemaPath);
        }

        public string XsdPath
        {
            get;
            set;
        }

        private void ValidateXml(out string[] errors, out string[] warnings)
        {
            errors = new string[0];
            warnings = new string[0];

            List<string> err = new List<string>();
            List<string> warn = new List<string>();

            var res = this.Resource;

            //Test for well-formedness
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(editor.XmlContent);
            }
            catch (XmlException ex)
            {
                err.Add(ex.Message);
            }

            //If strongly-typed, test that this is serializable
            if (res.IsStronglyTyped)
            {
                try
                {
                    //Test by simply attempting to deserialize the current xml content
                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(editor.XmlContent)))
                    {
                        //Use original resource type to determine how to deserialize
                        var obj = ResourceTypeRegistry.Deserialize(res.ResourceType, ms);
                    }
                }
                catch (Exception ex)
                {
                    err.Add(ex.Message);
                }
            }

            //Finally verify the content itself
            var xml = this.XmlContent;
            var xsd = GetXsd(res.ValidatingSchema);
            var validator = new XmlValidator();
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                validator.Validate(ms, xsd);
            }

            err.AddRange(validator.ValidationErrors);
            warn.AddRange(validator.ValidationWarnings);

            /*
            var xml = this.XmlContent;
            var config = new XmlReaderSettings();
            
            config.ValidationType = ValidationType.Schema;
            config.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            config.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            config.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            //This will trap all the errors and warnings that are raised
            config.ValidationEventHandler += (s, e) =>
            {
                if (e.Severity == XmlSeverityType.Warning)
                {
                    warn.Add(e.Message);
                }
                else
                {
                    err.Add(e.Message);
                }
            };

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                using (var reader = XmlReader.Create(ms, config))
                {
                    while (reader.Read()) { } //Trigger the validation
                }
            }*/

            errors = err.ToArray();
            warnings = warn.ToArray();
        }

        private XmlSchema GetXsd(string xsdFile)
        {
            string path = xsdFile;

            if (!string.IsNullOrEmpty(this.XsdPath))
                path = Path.Combine(this.XsdPath, xsdFile);

            if (File.Exists(path))
            {
                ValidationEventHandler handler = (s, e) =>
                {
                };
                return XmlSchema.Read(File.OpenRead(path), handler);
            }
            return null;
        }

        private IEditorService _edSvc;

        protected override void Bind(IEditorService service)
        {
            //NOTE: This is exempt from #1656 requirements because this will never be called when returing
            //from an XML editor because IT IS the xml editor!

            _edSvc = service;
            _edSvc.RegisterCustomNotifier(editor);
            editor.Bind(_edSvc);
            editor.ReadyForEditing(); //This turns on event broadcasting
            this.Title = Properties.Resources.XmlEditor + " " + ResourceIdentifier.GetName(this.EditorService.ResourceID);
        }

        protected override ICollection<ValidationIssue> ValidateEditedResource()
        {
            string[] warnings;
            string[] errors;

            ValidateXml(out errors, out warnings);
            var issues = new List<ValidationIssue>();
            foreach (string err in errors)
            {
                issues.Add(new ValidationIssue(this.Resource, ValidationStatus.Error, ValidationStatusCode.Error_General_ValidationError, err));
            }
            foreach (string warn in warnings)
            {
                issues.Add(new ValidationIssue(this.Resource, ValidationStatus.Warning, ValidationStatusCode.Warning_General_ValidationWarning, warn));
            }

            //Put through ValidationResultSet to weed out redundant messages
            var set = new ValidationResultSet(issues);

            //Only care about errors. Warnings and other types should not derail us from saving
            return set.GetAllIssues(ValidationStatus.Error);
        }

        public override string GetXmlContent()
        {
            return this.XmlContent;
        }

        public string XmlContent
        {
            get { return editor.XmlContent; }
            set { editor.XmlContent = value; }
        }

        public override bool CanEditAsXml
        {
            get
            {
                return false; //We're already in the XML editor!
            }
        }

        public override void SyncSessionCopy()
        {
            //Write our XML changes back into the edited resource copy and re-read
            _edSvc.ResourceService.SetResourceXmlData(_edSvc.EditedResourceID, new MemoryStream(Encoding.UTF8.GetBytes(this.XmlContent)));
            //base.SyncSessionCopy();
        }
    }
}
