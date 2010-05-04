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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.King.Oracle
{
	/// <summary>
	/// Summary description for FeatureSourceEditorKingOracle.
	/// </summary>
	public class FeatureSourceEditorKingOracle : System.Windows.Forms.UserControl, IResourceEditorControl
	{
		private System.ComponentModel.IContainer components;

		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private ResourceEditors.FeatureSourceEditors.ODBC.Credentials credentials;
		private System.Windows.Forms.ToolTip toolTips;
		private System.Windows.Forms.TextBox Schema;
		private System.Windows.Forms.TextBox FDOClass;
		private System.Windows.Forms.TextBox Service;
		private bool m_isUpdating = false;

		public FeatureSourceEditorKingOracle()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public FeatureSourceEditorKingOracle(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;

			UpdateDisplay();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureSourceEditorKingOracle));
            this.Schema = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FDOClass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Service = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.credentials = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Credentials();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // Schema
            // 
            resources.ApplyResources(this.Schema, "Schema");
            this.Schema.Name = "Schema";
            this.Schema.TextChanged += new System.EventHandler(this.Schema_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // FDOClass
            // 
            resources.ApplyResources(this.FDOClass, "FDOClass");
            this.FDOClass.Name = "FDOClass";
            this.FDOClass.TextChanged += new System.EventHandler(this.FDOClass_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // Service
            // 
            resources.ApplyResources(this.Service, "Service");
            this.Service.Name = "Service";
            this.Service.TextChanged += new System.EventHandler(this.Service_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // credentials
            // 
            resources.ApplyResources(this.credentials, "credentials");
            this.credentials.Name = "credentials";
            this.credentials.CredentialsChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Credentials.CredentialsChangedDelegate(this.credentials_CredentialsChanged);
            // 
            // FeatureSourceEditorKingOracle
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.Schema);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FDOClass);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Service);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.credentials);
            this.Name = "FeatureSourceEditorKingOracle";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public object Resource
		{
			get { return m_feature; }
			set 
			{
				m_feature = (OSGeo.MapGuide.MaestroAPI.FeatureSource)value;
				UpdateDisplay();
			}
		}

		public string ResourceId
		{
			get { return m_feature.ResourceId; }
			set { m_feature.ResourceId = value; }
		}

		public bool Preview()
		{
			return false;
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				if (m_feature == null || m_feature.Parameter == null)
					return;

				credentials.SetCredentials(m_feature.Parameter["Username"], m_feature.Parameter["Password"]);

				Service.Text = m_feature.Parameter["Service"];
				Schema.Text = m_feature.Parameter["OracleSchema"];
				FDOClass.Text = m_feature.Parameter["KingFdoClass"];
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		public bool Save(string savename)
		{
			return false;
		}

		private void credentials_CredentialsChanged(string username, string password)
		{
			if (m_feature == null || m_isUpdating)
				return;

			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_feature.Parameter["Username"] = username;
			m_feature.Parameter["Password"] = password;
			m_editor.HasChanged();
		}

		private void Service_TextChanged(object sender, System.EventArgs e)
		{
			if (m_feature == null || m_isUpdating)
				return;

			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_feature.Parameter["Service"] = Service.Text;
			m_editor.HasChanged();
		}

		private void Schema_TextChanged(object sender, System.EventArgs e)
		{
			if (m_feature == null || m_isUpdating)
				return;

			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_feature.Parameter["OracleSchema"] = Schema.Text;
			m_editor.HasChanged();
		}

		private void FDOClass_TextChanged(object sender, System.EventArgs e)
		{
			if (m_feature == null || m_isUpdating)
				return;

			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_feature.Parameter["KingFdoClass"] = FDOClass.Text;
			m_editor.HasChanged();
		}
    
        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return m_editor.CurrentConnection.SupportsResourcePreviews; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }
    }
}
