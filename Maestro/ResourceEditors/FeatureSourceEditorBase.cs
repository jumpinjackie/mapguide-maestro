#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// This usercontrol is a placeholder for the specialized version of the resource editor
	/// </summary>
	public class FeatureSourceEditorBase : System.Windows.Forms.UserControl, ResourceEditor 
	{
		private ResourceEditor m_child;
		private FeatureSourceEditorGeneric m_childGeneric = null;
		private ResourceEditors.EditorInterface m_editor = null;

		private bool m_isUpdating = false;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private System.Windows.Forms.TabControl EditorTab;
		private System.Windows.Forms.TabPage CustomEditorPage;
		private System.Windows.Forms.TabPage GenericEditorPage;
		private string m_realid = null;
		private ResourceEditors.CoordinateSystemOverride CoordinateSystemOverride;
		private System.Windows.Forms.Panel TestConnectionPanel;
		private System.Windows.Forms.TextBox TestConnectionResult;
		private System.Windows.Forms.Button btnTest;
		private Globalizator.Globalizator m_globalizor;

		private Hashtable m_providerMap = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox ProviderName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button EditExtensions;

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
				if (uc as ResourceEditor == null)
					throw new Exception("Failed to create new datasource");

				m_child = (ResourceEditor)uc;
				if (uc.GetType() == typeof(FeatureSourceEditorGeneric))
				{
					Panel editorPanel = new Panel();
					editorPanel.Top = EditorTab.Top;
					editorPanel.Left = EditorTab.Left;
					editorPanel.Width = EditorTab.Width;
					editorPanel.Height = EditorTab.Height;
					editorPanel.Controls.Clear();
					editorPanel.Controls.Add(uc);
					editorPanel.Anchor = EditorTab.Anchor;

					this.Controls.Remove(EditorTab);
					this.Controls.Add(editorPanel);
					
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
                    //TODO: Should probably have a special class rather than this ugly way...
                    throw new Exception("CANCEL");
                else
				    throw new Exception("Failed to create new datasource");


		}

		public FeatureSourceEditorBase(EditorInterface editor, string resourceID)
			: this()
		{
			m_editor = editor;
			m_realid = resourceID;
            string nid = new MaestroAPI.ResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.FeatureSource, m_editor.CurrentConnection.SessionID);
			editor.CurrentConnection.CopyResource(resourceID, nid, true);
			m_feature = editor.CurrentConnection.GetFeatureSource(nid);
			CreateLayout(editor, m_feature);

			if (m_child == null)
				throw new Exception("Failed to create new datasource");

		}

		private System.Type ResolveSpecificResourceEditor(EditorInterface editor, string providerName)
		{
			if (m_providerMap == null)
			{
				Hashtable ht = new Hashtable();
				string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "ProviderMap.xml");
				if (!System.IO.File.Exists(path))
					throw new Exception(string.Format("The setup file for the FeatureSource Editors is missing. Please place it at: {0}", path));

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
					throw new Exception("The setup file for the FeatureSource Editors is invalid.");

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
							throw new Exception(string.Format("Failed to load assembly {0} from file {1}.\nError message: {2}", pv.Provider, System.IO.Path.GetFullPath(pv.AssemblyPath), ex.Message), ex);
						}
						Type t = asm.GetType(pv.Control);
						if (t == null)
							throw new Exception(string.Format("Failed to find the Control type: {0} in assembly: {1}",  pv.Control, pv.AssemblyPath));
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
				//TODO: If feature is edited by Xml, it won't update here
				if (m_realid != null)
				{
					//Recopy
					//m_editor.CurrentConnection.CopyResource(((OSGeo.MapGuide.MaestroAPI.FeatureSource)value).ResourceId, m_feature.ResourceId, true);
					m_editor.CurrentConnection.SaveResourceAs(value, m_feature.ResourceId);
					m_feature = m_editor.CurrentConnection.GetFeatureSource(m_feature.ResourceId);
					m_child.Resource = m_feature;
					if (m_childGeneric != null)
						m_childGeneric.Resource = m_feature;
					CoordinateSystemOverride.SetItem(m_editor, m_feature);
					UpdateDisplay();
				}
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
			m_globalizor = new Globalizator.Globalizator(this);
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
			this.EditorTab = new System.Windows.Forms.TabControl();
			this.CustomEditorPage = new System.Windows.Forms.TabPage();
			this.GenericEditorPage = new System.Windows.Forms.TabPage();
			this.CoordinateSystemOverride = new OSGeo.MapGuide.Maestro.ResourceEditors.CoordinateSystemOverride();
			this.TestConnectionPanel = new System.Windows.Forms.Panel();
			this.TestConnectionResult = new System.Windows.Forms.TextBox();
			this.btnTest = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.ProviderName = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.EditExtensions = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.EditorTab.SuspendLayout();
			this.TestConnectionPanel.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// EditorTab
			// 
			this.EditorTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.EditorTab.Controls.Add(this.CustomEditorPage);
			this.EditorTab.Controls.Add(this.GenericEditorPage);
			this.EditorTab.Location = new System.Drawing.Point(0, 24);
			this.EditorTab.Name = "EditorTab";
			this.EditorTab.SelectedIndex = 0;
			this.EditorTab.Size = new System.Drawing.Size(592, 288);
			this.EditorTab.TabIndex = 0;
			this.EditorTab.SelectedIndexChanged += new System.EventHandler(this.EditorTab_SelectedIndexChanged);
			// 
			// CustomEditorPage
			// 
			this.CustomEditorPage.Location = new System.Drawing.Point(4, 22);
			this.CustomEditorPage.Name = "CustomEditorPage";
			this.CustomEditorPage.Size = new System.Drawing.Size(584, 262);
			this.CustomEditorPage.TabIndex = 0;
			this.CustomEditorPage.Text = "Custom editor";
			// 
			// GenericEditorPage
			// 
			this.GenericEditorPage.Location = new System.Drawing.Point(4, 22);
			this.GenericEditorPage.Name = "GenericEditorPage";
			this.GenericEditorPage.Size = new System.Drawing.Size(584, 246);
			this.GenericEditorPage.TabIndex = 1;
			this.GenericEditorPage.Text = "Generic Editor";
			// 
			// CoordinateSystemOverride
			// 
			this.CoordinateSystemOverride.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.CoordinateSystemOverride.Location = new System.Drawing.Point(8, 16);
			this.CoordinateSystemOverride.Name = "CoordinateSystemOverride";
			this.CoordinateSystemOverride.Size = new System.Drawing.Size(576, 112);
			this.CoordinateSystemOverride.TabIndex = 1;
			// 
			// TestConnectionPanel
			// 
			this.TestConnectionPanel.Controls.Add(this.TestConnectionResult);
			this.TestConnectionPanel.Controls.Add(this.btnTest);
			this.TestConnectionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.TestConnectionPanel.Location = new System.Drawing.Point(0, 320);
			this.TestConnectionPanel.Name = "TestConnectionPanel";
			this.TestConnectionPanel.Size = new System.Drawing.Size(592, 48);
			this.TestConnectionPanel.TabIndex = 2;
			// 
			// TestConnectionResult
			// 
			this.TestConnectionResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.TestConnectionResult.Location = new System.Drawing.Point(0, 0);
			this.TestConnectionResult.Multiline = true;
			this.TestConnectionResult.Name = "TestConnectionResult";
			this.TestConnectionResult.ReadOnly = true;
			this.TestConnectionResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.TestConnectionResult.Size = new System.Drawing.Size(416, 40);
			this.TestConnectionResult.TabIndex = 7;
			this.TestConnectionResult.Text = "Click on \"Test connection\" to test with the current parameters";
			// 
			// btnTest
			// 
			this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnTest.Location = new System.Drawing.Point(424, 8);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(160, 32);
			this.btnTest.TabIndex = 6;
			this.btnTest.Text = "Test connection";
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Provider";
			// 
			// ProviderName
			// 
			this.ProviderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ProviderName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ProviderName.Location = new System.Drawing.Point(112, 0);
			this.ProviderName.Name = "ProviderName";
			this.ProviderName.ReadOnly = true;
			this.ProviderName.Size = new System.Drawing.Size(472, 20);
			this.ProviderName.TabIndex = 4;
			this.ProviderName.Text = "";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.CoordinateSystemOverride);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox1.Location = new System.Drawing.Point(0, 408);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(592, 136);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Coordinate system override";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.EditExtensions);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 368);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(592, 40);
			this.panel1.TabIndex = 6;
			// 
			// EditExtensions
			// 
			this.EditExtensions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.EditExtensions.Location = new System.Drawing.Point(424, 8);
			this.EditExtensions.Name = "EditExtensions";
			this.EditExtensions.Size = new System.Drawing.Size(160, 24);
			this.EditExtensions.TabIndex = 1;
			this.EditExtensions.Text = "Edit extensions";
			this.EditExtensions.Click += new System.EventHandler(this.EditExtensions_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(408, 24);
			this.label2.TabIndex = 0;
			this.label2.Text = "The feature source can be joined with other properties and contain values that ar" +
				"e computed on the fly.";
			// 
			// FeatureSourceEditorBase
			// 
			this.AutoScroll = true;
			this.Controls.Add(this.TestConnectionPanel);
			this.Controls.Add(this.ProviderName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.EditorTab);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.groupBox1);
			this.Name = "FeatureSourceEditorBase";
			this.Size = new System.Drawing.Size(592, 544);
			this.EditorTab.ResumeLayout(false);
			this.TestConnectionPanel.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
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
			{
				m_editor.CurrentConnection.SaveResource(m_feature);
				m_editor.CurrentConnection.CopyResource(m_feature.ResourceId, savename, true);
				m_realid = savename;

			}
			return true;
		}

		private void EditorTab_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (EditorTab.SelectedTab == null || EditorTab.SelectedTab.Controls[0] as ResourceEditor == null)
				return;

			(EditorTab.SelectedTab.Controls[0] as ResourceEditor).Resource = m_feature;
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
					TestConnectionResult.Text = m_globalizor.Translate("Provider reported no errors");
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
        public bool ValidateResource() { return m_child.ValidateResource(); }
        public bool SupportsPreview { get { return m_child.SupportsPreview; } }
        public bool SupportsValidate { get { return m_child.SupportsValidate; } }
        public bool SupportsProfiling { get { return m_child.SupportsProfiling; } }
    }
}
