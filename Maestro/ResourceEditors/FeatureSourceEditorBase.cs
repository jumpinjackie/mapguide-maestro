#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using OSGeo.MapGuide.Maestro;
using OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourcePreview;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// This usercontrol is a placeholder for the specialized version of the resource editor
	/// </summary>
	public class FeatureSourceEditorBase : System.Windows.Forms.UserControl, IResourceEditorControl 
	{
		private IResourceEditorControl m_child;
		private FeatureSourceEditorGeneric m_childGeneric = null;
		private ResourceEditors.EditorInterface m_editor = null;

		private bool m_isUpdating = false;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private System.Windows.Forms.TabControl EditorTab;
		private System.Windows.Forms.TabPage CustomEditorPage;
		private System.Windows.Forms.TabPage GenericEditorPage;
		private ResourceEditors.CoordinateSystemOverride CoordinateSystemOverride;
		private System.Windows.Forms.Panel TestConnectionPanel;
		private System.Windows.Forms.TextBox TestConnectionResult;
		private System.Windows.Forms.Button btnTest;

		private Hashtable m_providerMap = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox ProviderName;
		private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel EditJoinPanel;
		private System.Windows.Forms.Button EditExtensions;
        private Panel panel1;
        private Button EditConfigDocButton;
        private TabPage LocalPreviewPage;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private void CreateLayout(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
		{
			ProviderName.Text = feature.Provider;
			System.Type ClassDef = ResolveSpecificResourceEditor(editor, feature.Provider);
			if (ClassDef != null)
			{
				UserControl uc = (UserControl)Activator.CreateInstance(ClassDef, new object[]{editor, feature} );
				if (uc as IResourceEditorControl == null)
					throw new Exception(Strings.FeatureSourceEditorBase.FeatureSourceCreationError);

				m_child = (IResourceEditorControl)uc;
				if (uc.GetType() == typeof(FeatureSourceEditorGeneric))
				{
					Panel editorPanel = new Panel();
					editorPanel.Controls.Clear();
					editorPanel.Controls.Add(uc);
					editorPanel.Anchor = EditorTab.Anchor;

					this.Controls.Remove(EditorTab);
					this.Controls.Add(editorPanel);
                    editorPanel.Dock = DockStyle.Fill;
                    editorPanel.BringToFront();
					
					m_childGeneric = (FeatureSourceEditorGeneric)m_child;
				}
				else
				{
					CustomEditorPage.Controls.Clear();
					CustomEditorPage.Controls.Add(uc);
					
					m_childGeneric = new FeatureSourceEditorGeneric(editor, feature);
					GenericEditorPage.Controls.Clear();
					GenericEditorPage.Controls.Add(m_childGeneric);
					m_childGeneric.Dock = DockStyle.Fill;
				}
				uc.Dock = DockStyle.Fill;
				m_child.Resource = m_feature;

				CoordinateSystemOverride.SetItem(m_editor, feature);
			}

            //This stuff is not applicable to raster providers
            string provider = feature.Provider.ToUpper();
            if (!provider.StartsWith("OSGEO.GDAL") &&
                !provider.StartsWith("OSGEO.WMS") &&
                !provider.StartsWith("AUTODESK.RASTER"))
            {
                FeatureSourcePreviewCtrl ctl = new FeatureSourcePreviewCtrl(m_editor, ProviderName.Text, m_editor.ResourceId);
                ctl.Dock = DockStyle.Fill;

                LocalPreviewPage.Controls.Clear();
                LocalPreviewPage.Controls.Add(ctl);

                //This feature is broken for any MG release < 2.2 so disable it.
                Version ver = editor.CurrentConnection.SiteVersion;
                Version supported = new Version(2, 2);
                if (ver < supported)
                    EditorTab.Controls.Remove(LocalPreviewPage);
            }
            else
            {
                EditorTab.Controls.Remove(LocalPreviewPage);
            }
		}

		public FeatureSourceEditorBase(EditorInterface editor)
			: this()
		{
			m_editor = editor;
			SelectDataProvider sdp = new SelectDataProvider(editor.CurrentConnection);
            DialogResult dlgres = sdp.ShowDialog(this);

			if (dlgres != DialogResult.Cancel && sdp.SelectedProvider != null)
			{
			    OSGeo.MapGuide.MaestroAPI.ResourceIdentifier nid = new OSGeo.MapGuide.MaestroAPI.ResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.FeatureSource, m_editor.CurrentConnection.SessionID);
				m_feature = new OSGeo.MapGuide.MaestroAPI.FeatureSource();
				m_feature.ResourceId = nid;
				m_feature.Provider = sdp.SelectedProvider.Name;
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();
				foreach(OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProviderConnectionProperty property in sdp.SelectedProvider.ConnectionProperties)
					m_feature.Parameter[property.Name] = property.DefaultValue;

				m_editor.CurrentConnection.SaveResource(m_feature);
				CreateLayout(editor, m_feature);
			}

            if (m_child == null)
                if (dlgres == DialogResult.Cancel)
                    throw new CancelException();
                else
				    throw new Exception(Strings.FeatureSourceEditorBase.FeatureSourceCreationError);


		}

		public FeatureSourceEditorBase(EditorInterface editor, string resourceID)
			: this()
		{
			m_editor = editor;
			m_feature = editor.CurrentConnection.GetFeatureSource(resourceID);

            if (string.IsNullOrEmpty(m_feature.Provider))
            {
                SelectDataProvider sdp = new SelectDataProvider(editor.CurrentConnection);
                if (sdp.ShowDialog(this) != DialogResult.Cancel && sdp.SelectedProvider != null)
                {
                    m_feature.Provider = sdp.SelectedProvider.Name;
                    m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();
                    foreach (OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProviderConnectionProperty property in sdp.SelectedProvider.ConnectionProperties)
                        m_feature.Parameter[property.Name] = property.DefaultValue;
                }
                else
                    throw new CancelException();
            }

			CreateLayout(editor, m_feature);

            if (m_child == null)
                throw new Exception(Strings.FeatureSourceEditorBase.FeatureSourceCreationError);
        }

		private System.Type ResolveSpecificResourceEditor(EditorInterface editor, string providerName)
		{
			if (m_providerMap == null)
			{
				Hashtable ht = new Hashtable();
				string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "ProviderMap.xml");
				if (!System.IO.File.Exists(path))
					throw new Exception(string.Format(Strings.FeatureSourceEditorBase.FeatureSourceEditorMapMissingError, path));

				ProviderEditorMap pvm = new ProviderEditorMap();

				try
				{
					System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(ProviderEditorMap));
					using(System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
						pvm = (ProviderEditorMap)sr.Deserialize(fs);
				}
				catch
				{
				}

				if (pvm.Mappings == null || pvm.Mappings.Length == 0)
					throw new Exception(Strings.FeatureSourceEditorBase.FeatureSourceEditorMapInvalidError);

				string oldDir = System.IO.Directory.GetCurrentDirectory();
				try
				{
					System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
					foreach(ProviderEditorMap.ProviderItem pv in pvm.Mappings)
					{
						System.Reflection.Assembly asm = null;
						try
						{
							asm = System.Reflection.Assembly.LoadFile(System.IO.Path.GetFullPath(pv.AssemblyPath));
						}
						catch (Exception ex)
						{
							throw new Exception(string.Format(Strings.FeatureSourceEditorBase.AssemblyLoadError, pv.Provider, System.IO.Path.GetFullPath(pv.AssemblyPath), ex.Message), ex);
						}
						Type t = asm.GetType(pv.Control);
						if (t == null)
							throw new Exception(string.Format(Strings.FeatureSourceEditorBase.ControlMissingError,  pv.Control, pv.AssemblyPath));
						ht.Add(pv.Provider, t);
					}
				} 
				finally
				{
					try { System.IO.Directory.SetCurrentDirectory(oldDir); }
					catch { }
				}
				m_providerMap = ht;
			}

			string prov = editor.CurrentConnection.RemoveVersionFromProviderName(providerName);
			if (m_providerMap.ContainsKey(prov))
				return (Type)m_providerMap[prov];
			else
				return typeof(FeatureSourceEditorGeneric);
		}

		public void UpdateDisplay()
		{
			m_child.UpdateDisplay();
			if (m_childGeneric != null)
				m_childGeneric.UpdateDisplay();
			CoordinateSystemOverride.UpdateDisplay();
		}

		public object Resource
		{
			get { return m_feature; }
			set 
			{
                m_feature = (MaestroAPI.FeatureSource)value;
				m_child.Resource = m_feature;
				if (m_childGeneric != null)
					m_childGeneric.Resource = m_feature;
				CoordinateSystemOverride.SetItem(m_editor, m_feature);
				UpdateDisplay();
                m_child.UpdateDisplay();
                if (m_childGeneric != null)
                    m_childGeneric.UpdateDisplay();
            }
		}

		public string ResourceId
		{
			get { return m_child.ResourceId; }
			set { m_child.ResourceId = value; }
		}

		protected FeatureSourceEditorBase()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureSourceEditorBase));
            this.EditorTab = new System.Windows.Forms.TabControl();
            this.CustomEditorPage = new System.Windows.Forms.TabPage();
            this.GenericEditorPage = new System.Windows.Forms.TabPage();
            this.TestConnectionPanel = new System.Windows.Forms.Panel();
            this.TestConnectionResult = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ProviderName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EditJoinPanel = new System.Windows.Forms.Panel();
            this.EditConfigDocButton = new System.Windows.Forms.Button();
            this.EditExtensions = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CoordinateSystemOverride = new OSGeo.MapGuide.Maestro.ResourceEditors.CoordinateSystemOverride();
            this.LocalPreviewPage = new System.Windows.Forms.TabPage();
            this.EditorTab.SuspendLayout();
            this.TestConnectionPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.EditJoinPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // EditorTab
            // 
            this.EditorTab.Controls.Add(this.CustomEditorPage);
            this.EditorTab.Controls.Add(this.GenericEditorPage);
            this.EditorTab.Controls.Add(this.LocalPreviewPage);
            resources.ApplyResources(this.EditorTab, "EditorTab");
            this.EditorTab.Name = "EditorTab";
            this.EditorTab.SelectedIndex = 0;
            this.EditorTab.SelectedIndexChanged += new System.EventHandler(this.EditorTab_SelectedIndexChanged);
            // 
            // CustomEditorPage
            // 
            resources.ApplyResources(this.CustomEditorPage, "CustomEditorPage");
            this.CustomEditorPage.Name = "CustomEditorPage";
            this.CustomEditorPage.UseVisualStyleBackColor = true;
            // 
            // GenericEditorPage
            // 
            resources.ApplyResources(this.GenericEditorPage, "GenericEditorPage");
            this.GenericEditorPage.Name = "GenericEditorPage";
            this.GenericEditorPage.UseVisualStyleBackColor = true;
            // 
            // TestConnectionPanel
            // 
            this.TestConnectionPanel.Controls.Add(this.TestConnectionResult);
            this.TestConnectionPanel.Controls.Add(this.btnTest);
            resources.ApplyResources(this.TestConnectionPanel, "TestConnectionPanel");
            this.TestConnectionPanel.Name = "TestConnectionPanel";
            // 
            // TestConnectionResult
            // 
            resources.ApplyResources(this.TestConnectionResult, "TestConnectionResult");
            this.TestConnectionResult.Name = "TestConnectionResult";
            this.TestConnectionResult.ReadOnly = true;
            // 
            // btnTest
            // 
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.Name = "btnTest";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ProviderName
            // 
            resources.ApplyResources(this.ProviderName, "ProviderName");
            this.ProviderName.Name = "ProviderName";
            this.ProviderName.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CoordinateSystemOverride);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // EditJoinPanel
            // 
            this.EditJoinPanel.Controls.Add(this.EditConfigDocButton);
            this.EditJoinPanel.Controls.Add(this.EditExtensions);
            resources.ApplyResources(this.EditJoinPanel, "EditJoinPanel");
            this.EditJoinPanel.Name = "EditJoinPanel";
            // 
            // EditConfigDocButton
            // 
            resources.ApplyResources(this.EditConfigDocButton, "EditConfigDocButton");
            this.EditConfigDocButton.Name = "EditConfigDocButton";
            this.EditConfigDocButton.Click += new System.EventHandler(this.EditConfigDocButton_Click);
            // 
            // EditExtensions
            // 
            resources.ApplyResources(this.EditExtensions, "EditExtensions");
            this.EditExtensions.Name = "EditExtensions";
            this.EditExtensions.Click += new System.EventHandler(this.EditExtensions_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ProviderName);
            this.panel1.Controls.Add(this.label1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // CoordinateSystemOverride
            // 
            resources.ApplyResources(this.CoordinateSystemOverride, "CoordinateSystemOverride");
            this.CoordinateSystemOverride.Name = "CoordinateSystemOverride";
            // 
            // LocalPreviewPage
            // 
            resources.ApplyResources(this.LocalPreviewPage, "LocalPreviewPage");
            this.LocalPreviewPage.Name = "LocalPreviewPage";
            this.LocalPreviewPage.UseVisualStyleBackColor = true;
            // 
            // FeatureSourceEditorBase
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.EditorTab);
            this.Controls.Add(this.TestConnectionPanel);
            this.Controls.Add(this.EditJoinPanel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "FeatureSourceEditorBase";
            this.EditorTab.ResumeLayout(false);
            this.TestConnectionPanel.ResumeLayout(false);
            this.TestConnectionPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.EditJoinPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		public bool Preview()
		{
            if (!m_child.Preview())
            {
                //It is using a temp id, so its safe to save it
                m_editor.CurrentConnection.SaveResource(m_feature);

                string url = ((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)m_editor.CurrentConnection).BaseURL;

                url += "schemareport/describeschema.php" +
                    "?viewer=" + (m_editor.UseFusionPreview ? "flexible" : "basic") + "&resId=" + System.Web.HttpUtility.UrlEncode(m_feature.ResourceId) +
 "&sessionId=" + System.Web.HttpUtility.UrlEncode(m_editor.CurrentConnection.SessionID)  + "&schemaName=&className=";

                m_editor.OpenUrl(url);

            }
        
            return true;
        }

		public bool Save(string savename)
		{
			if (m_child != m_childGeneric)
				m_childGeneric.Save(savename);


			if (!m_child.Save(savename))
				m_editor.CurrentConnection.SaveResourceAs(m_feature, savename);

            m_feature.ResourceId = savename;
			return true;
		}

		private void EditorTab_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (EditorTab.SelectedTab == null || EditorTab.SelectedTab.Controls[0] as IResourceEditorControl == null)
				return;

			(EditorTab.SelectedTab.Controls[0] as IResourceEditorControl).Resource = m_feature;
		}

		private void btnTest_Click(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			try
			{
				//We always work on a temp, so it is now safe to save it
				m_editor.CurrentConnection.SaveResource(m_feature);

				string x = m_editor.CurrentConnection.TestConnection(m_feature.ResourceId);
				if (x.Length == 0)
					TestConnectionResult.Text = Strings.FeatureSourceEditorBase.NoErrorsFound;
				else
					TestConnectionResult.Text = x;
			}
			catch(Exception ex)
			{
				TestConnectionResult.Text = ex.Message;
			}
		
		}

		private void EditExtensions_Click(object sender, System.EventArgs e)
		{
			FeatureSourceExtensions.EditExtensions dlg = new FeatureSourceExtensions.EditExtensions
				(m_editor, m_feature); 
			dlg.ShowDialog(this);
		}

        public bool Profile() { return m_child.Profile(); }
        public bool ValidateResource(bool recurse) 
        {
            try
            {
                //We always work on a temp, so it is now safe to save it
                m_editor.CurrentConnection.SaveResource(m_feature);
            }
            catch (Exception ex)
            {
                m_editor.SetLastException(ex);
                MessageBox.Show(string.Format(Strings.FeatureSourceEditorBase.ResourceSaveError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return m_child.ValidateResource(recurse); 
        }
        public bool SupportsPreview { get { return m_child.SupportsPreview; } }
        public bool SupportsValidate { get { return m_child.SupportsValidate; } }
        public bool SupportsProfiling { get { return m_child.SupportsProfiling; } }

        private void EditConfigDocButton_Click(object sender, EventArgs e)
        {
            if (m_feature == null)
                return;

            try
            {
                XmlEditor dlg;

                if (string.IsNullOrEmpty(m_feature.ConfigurationDocument))
                    dlg = new XmlEditor("", m_editor.CurrentConnection);
                else
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(m_editor.CurrentConnection.GetResourceData(m_feature.ResourceId, m_feature.ConfigurationDocument), System.Text.Encoding.UTF8, true))
                        dlg = new XmlEditor(sr.ReadToEnd(), m_editor.CurrentConnection);

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(m_feature.ConfigurationDocument))
                    {
                        m_editor.CurrentConnection.DeleteResourceData(m_feature.ResourceId, m_feature.ConfigurationDocument);
                        m_editor.HasChanged();
                    }

                    if (string.IsNullOrEmpty(dlg.EditorText))
                        m_feature.ConfigurationDocument = null;
                    else
                    {
                        if (string.IsNullOrEmpty(m_feature.ConfigurationDocument))
                            m_feature.ConfigurationDocument = "config.xml";

                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(new System.Text.UTF8Encoding(false).GetBytes(dlg.EditorText)))
                            m_editor.CurrentConnection.SetResourceData(m_feature.ResourceId, m_feature.ConfigurationDocument, OSGeo.MapGuide.MaestroAPI.ResourceDataType.Stream, ms);
                    }

                    m_childGeneric.UpdateDisplay();
                    m_editor.HasChanged();
                }
            }
            catch (Exception ex)
            {
                m_editor.SetLastException(ex);
                MessageBox.Show(this, string.Format(Strings.FeatureSourceEditorBase.XmlUpdateError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
