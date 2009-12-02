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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.Filebased
{
	/// <summary>
	/// Summary description for Managed.
	/// </summary>
	public class Managed : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.IContainer components;
		private ResourceEditors.EditorInterface m_editor = null;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item = null;
		private ResourceEditors.FeatureSourceEditors.ManagedFileControl managedFileControl;
        private string m_resourceName;

		public Managed()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			managedFileControl.NewDefaultSelected += new ManagedFileControl.NewDefaultSelectedDelegate(NewDefaultSelected);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Managed));
            this.managedFileControl = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ManagedFileControl();
            this.SuspendLayout();
            // 
            // managedFileControl
            // 
            resources.ApplyResources(this.managedFileControl, "managedFileControl");
            this.managedFileControl.FileTypes = null;
            this.managedFileControl.Name = "managedFileControl";
            // 
            // Managed
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.managedFileControl);
            this.Name = "Managed";
            this.ResumeLayout(false);

		}
		#endregion

        public void SetItem(ResourceEditors.EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource item, string resourceName, System.Collections.Specialized.NameValueCollection fileTypes)
		{
			m_item = item;
			m_editor = editor;
            m_resourceName = resourceName;
            managedFileControl.FileTypes = fileTypes;
			managedFileControl.SetItem(editor, item.ResourceId, new ManagedFileControl.IsDefaultItemDelegate(IsDefaultItem));
		}

		private bool IsDefaultItem(string filename)
		{
			return (m_item.Parameter != null && m_item.Parameter[m_resourceName] != null && m_item.Parameter[m_resourceName] == "%MG_DATA_FILE_PATH%" + filename);
		}

		private void NewDefaultSelected(string filename)
		{
			m_item.Parameter[m_resourceName] = "%MG_DATA_FILE_PATH%" + filename;
		}

		public void UpdateDisplay()
		{
			managedFileControl.UpdateDisplay();
		}


	}
}
