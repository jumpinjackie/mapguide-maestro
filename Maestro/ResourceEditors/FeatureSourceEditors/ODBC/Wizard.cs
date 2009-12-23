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
using System.Collections.Specialized;
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC
{
	/// <summary>
	/// Summary description for Wizard.
	/// </summary>
	public class Wizard : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private ResourceEditors.EditorInterface m_editor = null;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item = null;
		private System.Windows.Forms.Panel WizardPanel;
		private ResourceEditors.FeatureSourceEditors.ODBC.Wizards.MySQL mySQL;
		private ResourceEditors.FeatureSourceEditors.ODBC.Wizards.MsSQL msSQL;
		private ResourceEditors.FeatureSourceEditors.ODBC.Wizards.PostgreSQL postgreSQL;
		private ResourceEditors.FeatureSourceEditors.ODBC.Wizards.Informix informix;
		private ResourceEditors.FeatureSourceEditors.ODBC.Wizards.Oracle oracle;
		private ResourceEditors.FeatureSourceEditors.ODBC.Wizards.OracleMS oracleMS;
		private System.Windows.Forms.ComboBox DatabaseType;
		private bool m_isUpdating = false;
		public event FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate ConnectionStringUpdated;

		private enum DBTypes : int
		{
			MySQL,
			MSSql,
			PostgreSQL,
			Informix,
			OracleMS,
			Oracle
		}

		public Wizard()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			foreach(Control c in WizardPanel.Controls)
			{
				c.Visible = false;
				c.Dock = DockStyle.Fill;
			}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Wizard));
            this.DatabaseType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.WizardPanel = new System.Windows.Forms.Panel();
            this.oracleMS = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Wizards.OracleMS();
            this.oracle = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Wizards.Oracle();
            this.informix = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Wizards.Informix();
            this.postgreSQL = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Wizards.PostgreSQL();
            this.msSQL = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Wizards.MsSQL();
            this.mySQL = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Wizards.MySQL();
            this.WizardPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DatabaseType
            // 
            resources.ApplyResources(this.DatabaseType, "DatabaseType");
            this.DatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DatabaseType.Items.AddRange(new object[] {
            resources.GetString("DatabaseType.Items"),
            resources.GetString("DatabaseType.Items1"),
            resources.GetString("DatabaseType.Items2"),
            resources.GetString("DatabaseType.Items3"),
            resources.GetString("DatabaseType.Items4"),
            resources.GetString("DatabaseType.Items5")});
            this.DatabaseType.Name = "DatabaseType";
            this.DatabaseType.SelectedIndexChanged += new System.EventHandler(this.DatabaseType_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // WizardPanel
            // 
            resources.ApplyResources(this.WizardPanel, "WizardPanel");
            this.WizardPanel.Controls.Add(this.oracleMS);
            this.WizardPanel.Controls.Add(this.oracle);
            this.WizardPanel.Controls.Add(this.informix);
            this.WizardPanel.Controls.Add(this.postgreSQL);
            this.WizardPanel.Controls.Add(this.msSQL);
            this.WizardPanel.Controls.Add(this.mySQL);
            this.WizardPanel.Name = "WizardPanel";
            // 
            // oracleMS
            // 
            resources.ApplyResources(this.oracleMS, "oracleMS");
            this.oracleMS.Name = "oracleMS";
            this.oracleMS.ConnectionStringUpdated += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate(this.ConnectionStringUpdate);
            // 
            // oracle
            // 
            resources.ApplyResources(this.oracle, "oracle");
            this.oracle.Name = "oracle";
            this.oracle.ConnectionStringUpdated += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate(this.ConnectionStringUpdate);
            // 
            // informix
            // 
            resources.ApplyResources(this.informix, "informix");
            this.informix.Name = "informix";
            this.informix.ConnectionStringUpdated += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate(this.ConnectionStringUpdate);
            // 
            // postgreSQL
            // 
            resources.ApplyResources(this.postgreSQL, "postgreSQL");
            this.postgreSQL.Name = "postgreSQL";
            this.postgreSQL.ConnectionStringUpdated += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate(this.ConnectionStringUpdate);
            // 
            // msSQL
            // 
            resources.ApplyResources(this.msSQL, "msSQL");
            this.msSQL.Name = "msSQL";
            this.msSQL.ConnectionStringUpdated += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate(this.ConnectionStringUpdate);
            // 
            // mySQL
            // 
            resources.ApplyResources(this.mySQL, "mySQL");
            this.mySQL.Name = "mySQL";
            this.mySQL.ConnectionStringUpdated += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate(this.ConnectionStringUpdate);
            // 
            // Wizard
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.WizardPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DatabaseType);
            this.Name = "Wizard";
            this.WizardPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public void SetItem(ResourceEditors.EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource item)
		{
			try
			{
				m_isUpdating = true;
				m_editor = editor;
				m_item = item;

				informix.SetItem(editor, item);
				msSQL.SetItem(editor, item);
				mySQL.SetItem(editor, item);
				oracle.SetItem(editor, item);
				oracleMS.SetItem(editor, item);
				postgreSQL.SetItem(editor, item);


				UpdateDisplay();
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				if (m_item == null || m_item.Parameter == null)
					return;

				informix.UpdateDisplay();
				msSQL.UpdateDisplay();
				mySQL.UpdateDisplay();
				oracle.UpdateDisplay();
				oracleMS.UpdateDisplay();
				postgreSQL.UpdateDisplay();


				NameValueCollection nv = ConnectionStringManager.SplitConnectionString(m_item.Parameter["ConnectionString"]);
				switch(nv["Driver"])
				{
					case "{MySQL ODBC 3.51 Driver}":
						DatabaseType.SelectedIndex = (int)DBTypes.MySQL;
						break;
					case "{SQL Server}":
						DatabaseType.SelectedIndex = (int)DBTypes.MSSql;
						break;
					case "{PostgreSQL}":
						DatabaseType.SelectedIndex = (int)DBTypes.PostgreSQL;
						break;
					case "{INFORMIX 3.30 32 BIT}":
						DatabaseType.SelectedIndex = (int)DBTypes.Informix;
						break;
					case "{Microsoft ODBC for Oracle}":
						DatabaseType.SelectedIndex = (int)DBTypes.OracleMS;
						break;
					case "{Oracle ODBC Driver}":
						DatabaseType.SelectedIndex = (int)DBTypes.Oracle;
						break;
					default:
						DatabaseType.SelectedIndex = -1;
						break;
				}
			}
			finally
			{
				m_isUpdating = false;
			}
		}


		private void DatabaseType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Control sel = null;
			switch((DBTypes)DatabaseType.SelectedIndex)
			{
				case DBTypes.MySQL:
					sel = mySQL;
					break;
				case DBTypes.MSSql:
					sel = msSQL;
					break;
				case DBTypes.PostgreSQL:
					sel = postgreSQL;
					break;
				case DBTypes.Informix:
					sel = informix;
					break;
				case DBTypes.OracleMS:
					sel = oracleMS;
					break;
				case DBTypes.Oracle:
					sel = oracle;
					break;
			}

			foreach(Control c in WizardPanel.Controls)
				c.Visible = c == sel;
		}

		private void ConnectionStringUpdate(string connectionString)
		{
			if (ConnectionStringUpdated != null)
				ConnectionStringUpdated(connectionString);
		}

	}
}
