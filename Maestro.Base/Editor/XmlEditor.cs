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
using Maestro.Base.Services;
using Maestro.Editors.Common;
using Maestro.Editors.Preview;

#pragma warning disable 1591

namespace Maestro.Base.Editor
{
    /// <summary>
    /// A generic XML editor for any resource.
    /// </summary>
    /// <remarks>
    /// Although public, this class is undocumented and reserved for internal use by built-in Maestro AddIns
    /// </remarks>
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
            XmlValidator.ValidateResourceXmlContent(editor.XmlContent, this.XsdPath, out errors, out warnings);
        }

        private IEditorService _edSvc;

        protected override void Bind(IEditorService service)
        {
            //NOTE: This is exempt from #1656 requirements because this will never be called when returing
            //from an XML editor because IT IS the xml editor!

            _edSvc = service;
            _edSvc.RegisterCustomNotifier(editor);
            var path = Path.Combine(this.XsdPath, _edSvc.GetEditedResource().ValidatingSchema);
            editor.LoadAutoCompletionData(path);
            editor.Bind(_edSvc);
            editor.ReadyForEditing(); //This turns on event broadcasting
            this.Title = Strings.XmlEditor + " " + ResourceIdentifier.GetName(this.EditorService.ResourceID); //NOXLATE
        }

        protected override ICollection<ValidationIssue> ValidateEditedResource()
        {
            string[] warnings;
            string[] errors;

            Func<ValidationIssue[]> validator = () =>
            {
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

                try
                {
                    var res = ResourceTypeRegistry.Deserialize(editor.XmlContent);
                    var context = new ResourceValidationContext(_edSvc.ResourceService, _edSvc.FeatureService);
                    //We don't care about dependents, we just want to validate *this* resource
                    var resIssues = ResourceValidatorSet.Validate(context, res, false);
                    set.AddIssues(resIssues);
                }
                catch
                {
                    //This can fail because the XML may be for something that Maestro does not offer a strongly-typed class for yet.
                    //So the XML may be legit, just not for this version of Maestro that is doing the validating
                }

                //Only care about errors. Warnings and other types should not derail us from saving
                return set.GetAllIssues(ValidationStatus.Error);
            };

            if (this.InvokeRequired)
                return (ValidationIssue[])this.Invoke(validator);
            else
                return validator();
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

        public override void Preview()
        {
            //Save the current resource to another session copy
            string resId = "Session:" + this.EditorService.SessionID + "//" + Guid.NewGuid() + "." + this.Resource.ResourceType.ToString(); //NOXLATE
            string xml = this.XmlContent;
            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    this.EditorService.ResourceService.SetResourceXmlData(resId, ms);
                }
            }
            catch (Exception ex)
            {
                XmlContentErrorDialog.CheckAndHandle(ex, xml, false);
            }

            //Copy any resource data
            var previewCopy = this.EditorService.ResourceService.GetResource(resId);
            this.Resource.CopyResourceDataTo(previewCopy);

            var conn = previewCopy.CurrentConnection;
            var previewer = ResourcePreviewerFactory.GetPreviewer(conn.ProviderName);
            if (previewer != null)
                previewer.Preview(previewCopy, this.EditorService);
        }
        
        public override void SyncSessionCopy()
        {
            //Write our XML changes back into the edited resource copy and re-read
            string xml = this.XmlContent;
            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    _edSvc.ResourceService.SetResourceXmlData(_edSvc.EditedResourceID, ms);
                }
            }
            catch (Exception ex)
            {
                XmlContentErrorDialog.CheckAndHandle(ex, xml, false);
            }
            //base.SyncSessionCopy();
        }

        internal void FindAndReplace(string find, string replace)
        {
            editor.FindAndReplace(find, replace);
        }

        public override Icon ViewIcon
        {
            get
            {
                return Properties.Resources.icon_xmleditor;
            }
        }

        private void editor_RequestReloadFromSource(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion(Strings.ConfirmReloadXmlFromSource))
            {
                var resId = this.EditorService.ResourceID;
                using (var stream = this.EditorService.ResourceService.GetResourceXmlData(resId))
                {
                    using (var sr = new StreamReader(stream))
                    {
                        var xml = sr.ReadToEnd();
                        editor.XmlContent = xml;
                        MessageService.ShowMessage(Strings.XmlReloadedFromSource);
                    }
                }
            }
        }
    }
}
