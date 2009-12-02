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
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton DisplayRawXml;
        private Label label1;
        private TextBox SourceCoordinateSystem;
        private TextBox TargetCoordinateSystem;
        private Label label2;
        private Button BrowseSimpleOverrideButton;
        private Panel SimpleOverridePanel;
        private Button LoadSimpleSourceProjection;
        private Button ToggleExtendedModeButton;
        private Button RemoveSimpleCoordinateOverrides;
        private ToolStripButton ToggleSimpleModeButton;
        private ToolStrip toolStrip;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoordinateSystemOverride));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.AddButton = new System.Windows.Forms.ToolStripButton();
            this.DeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.EditButton = new System.Windows.Forms.ToolStripButton();
            this.RefreshButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.DisplayRawXml = new System.Windows.Forms.ToolStripButton();
            this.ToggleSimpleModeButton = new System.Windows.Forms.ToolStripButton();
            this.ProjectionOverrides = new System.Windows.Forms.ListView();
            this.SourceHeader = new System.Windows.Forms.ColumnHeader();
            this.TargetHeader = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.SourceCoordinateSystem = new System.Windows.Forms.TextBox();
            this.TargetCoordinateSystem = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BrowseSimpleOverrideButton = new System.Windows.Forms.Button();
            this.SimpleOverridePanel = new System.Windows.Forms.Panel();
            this.RemoveSimpleCoordinateOverrides = new System.Windows.Forms.Button();
            this.LoadSimpleSourceProjection = new System.Windows.Forms.Button();
            this.ToggleExtendedModeButton = new System.Windows.Forms.Button();
            this.toolStrip.SuspendLayout();
            this.SimpleOverridePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddButton,
            this.DeleteButton,
            this.toolStripSeparator1,
            this.EditButton,
            this.RefreshButton,
            this.toolStripSeparator2,
            this.DisplayRawXml,
            this.ToggleSimpleModeButton});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // AddButton
            // 
            this.AddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.AddButton, "AddButton");
            this.AddButton.Name = "AddButton";
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.DeleteButton, "DeleteButton");
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // EditButton
            // 
            this.EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.EditButton, "EditButton");
            this.EditButton.Name = "EditButton";
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.RefreshButton, "RefreshButton");
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // DisplayRawXml
            // 
            this.DisplayRawXml.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.DisplayRawXml, "DisplayRawXml");
            this.DisplayRawXml.Name = "DisplayRawXml";
            this.DisplayRawXml.Click += new System.EventHandler(this.DisplayRawXml_Click);
            // 
            // ToggleSimpleModeButton
            // 
            this.ToggleSimpleModeButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToggleSimpleModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.ToggleSimpleModeButton, "ToggleSimpleModeButton");
            this.ToggleSimpleModeButton.Name = "ToggleSimpleModeButton";
            this.ToggleSimpleModeButton.Click += new System.EventHandler(this.ToggleSimpleModeButton_Click);
            // 
            // ProjectionOverrides
            // 
            this.ProjectionOverrides.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SourceHeader,
            this.TargetHeader});
            resources.ApplyResources(this.ProjectionOverrides, "ProjectionOverrides");
            this.ProjectionOverrides.Name = "ProjectionOverrides";
            this.ProjectionOverrides.UseCompatibleStateImageBehavior = false;
            this.ProjectionOverrides.View = System.Windows.Forms.View.Details;
            this.ProjectionOverrides.SelectedIndexChanged += new System.EventHandler(this.ProjectionOverrides_SelectedIndexChanged);
            this.ProjectionOverrides.DoubleClick += new System.EventHandler(this.ProjectionOverrides_DoubleClick);
            // 
            // SourceHeader
            // 
            resources.ApplyResources(this.SourceHeader, "SourceHeader");
            // 
            // TargetHeader
            // 
            resources.ApplyResources(this.TargetHeader, "TargetHeader");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // SourceCoordinateSystem
            // 
            resources.ApplyResources(this.SourceCoordinateSystem, "SourceCoordinateSystem");
            this.SourceCoordinateSystem.Name = "SourceCoordinateSystem";
            this.SourceCoordinateSystem.ReadOnly = true;
            this.SourceCoordinateSystem.TextChanged += new System.EventHandler(this.SimpleCoordinateSystem_TextChanged);
            // 
            // TargetCoordinateSystem
            // 
            resources.ApplyResources(this.TargetCoordinateSystem, "TargetCoordinateSystem");
            this.TargetCoordinateSystem.Name = "TargetCoordinateSystem";
            this.TargetCoordinateSystem.ReadOnly = true;
            this.TargetCoordinateSystem.TextChanged += new System.EventHandler(this.SimpleCoordinateSystem_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // BrowseSimpleOverrideButton
            // 
            resources.ApplyResources(this.BrowseSimpleOverrideButton, "BrowseSimpleOverrideButton");
            this.BrowseSimpleOverrideButton.Name = "BrowseSimpleOverrideButton";
            this.BrowseSimpleOverrideButton.UseVisualStyleBackColor = true;
            this.BrowseSimpleOverrideButton.Click += new System.EventHandler(this.BrowseSimpleOverrideButton_Click);
            // 
            // SimpleOverridePanel
            // 
            this.SimpleOverridePanel.Controls.Add(this.RemoveSimpleCoordinateOverrides);
            this.SimpleOverridePanel.Controls.Add(this.LoadSimpleSourceProjection);
            this.SimpleOverridePanel.Controls.Add(this.ToggleExtendedModeButton);
            this.SimpleOverridePanel.Controls.Add(this.BrowseSimpleOverrideButton);
            this.SimpleOverridePanel.Controls.Add(this.TargetCoordinateSystem);
            this.SimpleOverridePanel.Controls.Add(this.label1);
            this.SimpleOverridePanel.Controls.Add(this.label2);
            this.SimpleOverridePanel.Controls.Add(this.SourceCoordinateSystem);
            resources.ApplyResources(this.SimpleOverridePanel, "SimpleOverridePanel");
            this.SimpleOverridePanel.Name = "SimpleOverridePanel";
            // 
            // RemoveSimpleCoordinateOverrides
            // 
            resources.ApplyResources(this.RemoveSimpleCoordinateOverrides, "RemoveSimpleCoordinateOverrides");
            this.RemoveSimpleCoordinateOverrides.Name = "RemoveSimpleCoordinateOverrides";
            this.RemoveSimpleCoordinateOverrides.UseVisualStyleBackColor = true;
            this.RemoveSimpleCoordinateOverrides.Click += new System.EventHandler(this.RemoveSimpleCoordinateOverrides_Click);
            // 
            // LoadSimpleSourceProjection
            // 
            resources.ApplyResources(this.LoadSimpleSourceProjection, "LoadSimpleSourceProjection");
            this.LoadSimpleSourceProjection.Name = "LoadSimpleSourceProjection";
            this.LoadSimpleSourceProjection.UseVisualStyleBackColor = true;
            this.LoadSimpleSourceProjection.Click += new System.EventHandler(this.LoadSimpleSourceProjection_Click);
            // 
            // ToggleExtendedModeButton
            // 
            resources.ApplyResources(this.ToggleExtendedModeButton, "ToggleExtendedModeButton");
            this.ToggleExtendedModeButton.Name = "ToggleExtendedModeButton";
            this.ToggleExtendedModeButton.UseVisualStyleBackColor = true;
            this.ToggleExtendedModeButton.Click += new System.EventHandler(this.ToggleExtendedModeButton_Click);
            // 
            // CoordinateSystemOverride
            // 
            this.Controls.Add(this.ProjectionOverrides);
            this.Controls.Add(this.SimpleOverridePanel);
            this.Controls.Add(this.toolStrip);
            this.Name = "CoordinateSystemOverride";
            resources.ApplyResources(this, "$this");
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.SimpleOverridePanel.ResumeLayout(false);
            this.SimpleOverridePanel.PerformLayout();
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
                RefreshOverrideList();

                if (ProjectionOverrides.Items.Count <= 1)
                    ToggleSimpleModeButton_Click(this, null);
                else
                    ToggleExtendedModeButton_Click(this, null);
			}
			finally
			{
				m_isUpdating = false;
			}
		}

        private void RefreshOverrideList()
        {
            if (m_item == null)
                return;

            ProjectionOverrides.Items.Clear();
            if (m_item.SupplementalSpatialContextInfo != null)
                foreach (SpatialContextType sp in m_item.SupplementalSpatialContextInfo)
                    ProjectionOverrides.Items.Add(CreateLviFromSP(sp));
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
                MessageBox.Show(this, string.Format(Strings.CoordinateSystemOverride.CoordinateSystemLoadError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayRawXml_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_item.CurrentConnection == null)
                    m_item.CurrentConnection = m_editor.CurrentConnection;

                //This is always a copy, so saving is always safe
                m_editor.CurrentConnection.SaveResource(m_item);

                OSGeo.MapGuide.MaestroAPI.FdoSpatialContextList resp = m_item.GetSpatialInfo();
                if (resp == null)
                    throw new Exception(Strings.CoordinateSystemOverride.SpatialContentNullError);

                Form f = new Form();
                f.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                f.Text = Strings.CoordinateSystemOverride.SpatialContextInfoDialogTitle;
                f.StartPosition = FormStartPosition.CenterParent;
                f.Width = 500;
                f.Height = 500;
                TextBox t = new TextBox();
                f.Controls.Clear();
                f.Controls.Add(t);
                t.ReadOnly = true;
                t.Dock = DockStyle.Fill;
                t.Multiline = true;
                t.ScrollBars = ScrollBars.Both;


                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(resp.GetType());

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    xs.Serialize(ms, resp);
                    t.Text = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                }

                f.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(Strings.CoordinateSystemOverride.CoordinateSystemLoadError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        

        }

        private void ToggleExtendedModeButton_Click(object sender, EventArgs e)
        {
            RefreshOverrideList();
            SimpleOverridePanel.Visible = false;
            ProjectionOverrides.Visible = toolStrip.Visible = true;
        }

        private void ToggleSimpleModeButton_Click(object sender, EventArgs e)
        {
            if (ProjectionOverrides.Items.Count > 1)
            {
                if (MessageBox.Show(this, Strings.CoordinateSystemOverride.RemoveOverridesConfirmation, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
            }

            if (m_item != null && m_item.SupplementalSpatialContextInfo != null && m_item.SupplementalSpatialContextInfo.Count >= 1)
            {
                while (m_item.SupplementalSpatialContextInfo.Count > 1)
                    m_item.SupplementalSpatialContextInfo.RemoveAt(m_item.SupplementalSpatialContextInfo.Count - 1);

                //Copy to guard against an event that clears it
                string tmp = m_item.SupplementalSpatialContextInfo[0].CoordinateSystem;
                SourceCoordinateSystem.Text = m_item.SupplementalSpatialContextInfo[0].Name;
                TargetCoordinateSystem.Text = tmp;
            }
            else
            {
                SourceCoordinateSystem.Text = "";
                TargetCoordinateSystem.Text = "";
            }

            SimpleOverridePanel.Visible = true;
            ProjectionOverrides.Visible = toolStrip.Visible = false;
        }

        private void LoadSimpleSourceProjection_Click(object sender, EventArgs e)
        {
            if (m_item != null)
            {
                try
                {
                    if (m_item.CurrentConnection == null)
                        m_item.CurrentConnection = m_editor.CurrentConnection;

                    FdoSpatialContextList lst = m_item.GetSpatialInfo();
                    if (lst.SpatialContext.Count > 0)
                        SourceCoordinateSystem.Text = lst.SpatialContext[0].Name;
                    else
                        SourceCoordinateSystem.Text = "Default";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format(Strings.CoordinateSystemOverride.CoordinateSystemLoadError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RemoveSimpleCoordinateOverrides_Click(object sender, EventArgs e)
        {
            SourceCoordinateSystem.Text = TargetCoordinateSystem.Text = "";
            m_item.SupplementalSpatialContextInfo = null;
        }

        private void SimpleCoordinateSystem_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;

            if (m_item.SupplementalSpatialContextInfo == null)
                m_item.SupplementalSpatialContextInfo = new SpatialContextTypeCollection();

            SpatialContextType cnt = new SpatialContextType();
            cnt.Name = SourceCoordinateSystem.Text;
            cnt.CoordinateSystem = TargetCoordinateSystem.Text;
            m_item.SupplementalSpatialContextInfo.Clear();
            m_item.SupplementalSpatialContextInfo.Add(cnt);
            m_editor.HasChanged();
        }

        private void BrowseSimpleOverrideButton_Click(object sender, EventArgs e)
        {
            SelectCoordinateSystem dlg = new SelectCoordinateSystem(m_editor.CurrentConnection);
            dlg.SetWKT(TargetCoordinateSystem.Text);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                TargetCoordinateSystem.Text = dlg.SelectedCoordSys.WKT;

            if (SourceCoordinateSystem.Text.Length == 0)
                LoadSimpleSourceProjection_Click(sender, e);
        }
	
	}
}
