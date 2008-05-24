#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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
using OSGeo.MapGuide.MaestroAPI;


namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for CoordinateSystemOverride.
	/// </summary>
	public class CoordinateSystemOverride : System.Windows.Forms.UserControl
    {
		private System.ComponentModel.IContainer components;

		private bool m_isUpdating = false;
		private EditorInterface m_editor = null;
		private System.Windows.Forms.ListView ProjectionOverrides;
		private System.Windows.Forms.ColumnHeader SourceHeader;
        private System.Windows.Forms.ColumnHeader TargetHeader;
        private ToolStripButton AddButton;
        private ToolStripButton DeleteButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton EditButton;
        private ToolStripButton RefreshButton;
		private FeatureSource m_item = null;

		public CoordinateSystemOverride()
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
            System.Windows.Forms.ToolStrip toolStrip;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoordinateSystemOverride));
            this.AddButton = new System.Windows.Forms.ToolStripButton();
            this.DeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.EditButton = new System.Windows.Forms.ToolStripButton();
            this.RefreshButton = new System.Windows.Forms.ToolStripButton();
            this.ProjectionOverrides = new System.Windows.Forms.ListView();
            this.SourceHeader = new System.Windows.Forms.ColumnHeader();
            this.TargetHeader = new System.Windows.Forms.ColumnHeader();
            toolStrip = new System.Windows.Forms.ToolStrip();
            toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddButton,
            this.DeleteButton,
            this.toolStripSeparator1,
            this.EditButton,
            this.RefreshButton});
            toolStrip.Location = new System.Drawing.Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip.Size = new System.Drawing.Size(648, 25);
            toolStrip.TabIndex = 5;
            toolStrip.Text = "toolStrip1";
            // 
            // AddButton
            // 
            this.AddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddButton.Image = ((System.Drawing.Image)(resources.GetObject("AddButton.Image")));
            this.AddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(23, 22);
            this.AddButton.Text = "toolStripButton1";
            this.AddButton.ToolTipText = "Add a new coordinate system override";
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteButton.Enabled = false;
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(23, 22);
            this.DeleteButton.Text = "toolStripButton2";
            this.DeleteButton.ToolTipText = "Delete a coordinate system override";
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // EditButton
            // 
            this.EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EditButton.Enabled = false;
            this.EditButton.Image = ((System.Drawing.Image)(resources.GetObject("EditButton.Image")));
            this.EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(23, 22);
            this.EditButton.Text = "toolStripButton3";
            this.EditButton.ToolTipText = "Edit a coordinate system override";
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshButton.Image")));
            this.RefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(23, 22);
            this.RefreshButton.Text = "toolStripButton4";
            this.RefreshButton.ToolTipText = "Fetch coordinate systems found in the featuresource";
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // ProjectionOverrides
            // 
            this.ProjectionOverrides.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SourceHeader,
            this.TargetHeader});
            this.ProjectionOverrides.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectionOverrides.Location = new System.Drawing.Point(0, 25);
            this.ProjectionOverrides.Name = "ProjectionOverrides";
            this.ProjectionOverrides.Size = new System.Drawing.Size(648, 183);
            this.ProjectionOverrides.TabIndex = 4;
            this.ProjectionOverrides.UseCompatibleStateImageBehavior = false;
            this.ProjectionOverrides.View = System.Windows.Forms.View.Details;
            this.ProjectionOverrides.SelectedIndexChanged += new System.EventHandler(this.ProjectionOverrides_SelectedIndexChanged);
            this.ProjectionOverrides.DoubleClick += new System.EventHandler(this.ProjectionOverrides_DoubleClick);
            // 
            // SourceHeader
            // 
            this.SourceHeader.Text = "Source";
            this.SourceHeader.Width = 288;
            // 
            // TargetHeader
            // 
            this.TargetHeader.Text = "Target";
            this.TargetHeader.Width = 297;
            // 
            // CoordinateSystemOverride
            // 
            this.Controls.Add(this.ProjectionOverrides);
            this.Controls.Add(toolStrip);
            this.Name = "CoordinateSystemOverride";
            this.Size = new System.Drawing.Size(648, 208);
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public void SetItem(EditorInterface editor, FeatureSource item)
		{
			m_editor = editor;
			m_item = item;

			try
			{
				m_isUpdating = true;
				ProjectionOverrides.Items.Clear();
				if (item.SupplementalSpatialContextInfo != null)
					
					foreach(SpatialContextType sp in item.SupplementalSpatialContextInfo)
						ProjectionOverrides.Items.Add(CreateLviFromSP(sp));
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void ProjectionOverrides_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DeleteButton.Enabled = EditButton.Enabled = ProjectionOverrides.SelectedItems.Count == 1;
		}

		private void ProjectionOverrides_DoubleClick(object sender, System.EventArgs e)
		{
            EditButton_Click(sender, e);
		}

		private ListViewItem CreateLviFromSP(SpatialContextType sp)
		{
			ListViewItem lvi = new ListViewItem(sp.Name);
			lvi.SubItems.Add(sp.CoordinateSystem);
			lvi.Tag = sp;
			return lvi;
		}

		public void UpdateDisplay()
		{
		}

        private void AddButton_Click(object sender, EventArgs e)
        {
            CoordinateSystemOverrideDialog dlg = new CoordinateSystemOverrideDialog(m_editor, new OSGeo.MapGuide.MaestroAPI.SpatialContextType());
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SpatialContextType fsp = dlg.Item;
                if (m_item.SupplementalSpatialContextInfo == null)
                    m_item.SupplementalSpatialContextInfo = new SpatialContextTypeCollection();

                m_item.SupplementalSpatialContextInfo.Add(fsp);

                ProjectionOverrides.Items.Add(CreateLviFromSP(fsp));
                m_editor.HasChanged();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (ProjectionOverrides.SelectedItems.Count != 1)
                return;
            ListViewItem lvi = ProjectionOverrides.SelectedItems[0];
            ProjectionOverrides.Items.Remove(lvi);

            for (int i = 0; i < m_item.SupplementalSpatialContextInfo.Count; i++)
                if (lvi.Tag == m_item.SupplementalSpatialContextInfo[i])
                {

                    m_item.SupplementalSpatialContextInfo.RemoveAt(i);
                    break;
                }

            /*if (m_item.SupplementalSpatialContextInfo.Count == 0)
                m_item.SupplementalSpatialContextInfo = null;*/

            m_editor.HasChanged();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (ProjectionOverrides.SelectedItems.Count != 1)
                return;
            ListViewItem lvi = ProjectionOverrides.SelectedItems[0];
            SpatialContextType sp = lvi.Tag as SpatialContextType;
            if (sp == null)
                return;

            CoordinateSystemOverrideDialog dlg = new CoordinateSystemOverrideDialog(m_editor, sp);

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                lvi.Text = sp.Name;
                lvi.SubItems[1].Text = sp.CoordinateSystem;
                m_editor.HasChanged();
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            try
            {

                Hashtable ht = new Hashtable();
                foreach (ListViewItem lvi in ProjectionOverrides.Items)
                    ht.Add(lvi.Text, null);

                if (m_item.CurrentConnection == null)
                    m_item.CurrentConnection = m_editor.CurrentConnection;

                //This is always a copy, so saving is always safe
                m_editor.CurrentConnection.SaveResource(m_item);

                OSGeo.MapGuide.MaestroAPI.FdoSpatialContextList resp = m_item.GetSpatialInfo();
                foreach (FdoSpatialContextListSpatialContext sp in resp.SpatialContext)
                    if (!ht.ContainsKey(sp.Name))
                    {
                        SpatialContextType spt = new SpatialContextType();
                        spt.Name = sp.Name;
                        spt.CoordinateSystem = sp.CoordinateSystemWkt;
                        if (m_item.SupplementalSpatialContextInfo == null)
                            m_item.SupplementalSpatialContextInfo = new SpatialContextTypeCollection();
                        m_item.SupplementalSpatialContextInfo.Add(spt);
                        ProjectionOverrides.Items.Add(CreateLviFromSP(spt));
                        ht.Add(sp.Name, null);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("Failed to retrive coordinate info: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
	
	}
}
