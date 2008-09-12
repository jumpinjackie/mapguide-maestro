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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro
{
    public partial class ResourceProperties : Form
    {
        private ServerConnectionI m_connection;
        private ResourceIdentifier m_resourceId;

        private ResourceDocumentHeaderType m_resourceHeader;
        private ResourceFolderHeaderType m_folderHeader;

        private Globalizator.Globalizator m_globalizor = null;

        private bool m_isUpdating = false;
        private string[] m_srslist = null;

        private const int RWUSER = 0;
        private const int ROUSER = 1;
        private const int NOUSER = 2;
        private const int RWGROUP = 3;
        private const int ROGROUP = 4;
        private const int NOGROUP = 5;
        private const int IHUSER = 6;
        private const int IHGROUP = 7;

        private string[] StatusNames =
        {
            "Read/write",
            "Read only",
            "No access",
            "Read/write",
            "Read only",
            "No access",
            "Inherited",
            "Inherited"
        };

        public ResourceProperties(ServerConnectionI connection, string resourceId)
            : this()
        {
            m_connection = connection;
            m_resourceId = resourceId;
            ResourceID.Text = resourceId;
        }

        private ResourceProperties()
        {
            InitializeComponent();
            m_globalizor = new Globalizator.Globalizator(this);
            for (int i = 0; i < StatusNames.Length; i++)
                StatusNames[i] = m_globalizor.Translate(StatusNames[i]);
        }

        private void ResourceProperties_Load(object sender, EventArgs e)
        {
            Dictionary<string, ListViewItem> ul = new Dictionary<string, ListViewItem>();
            Dictionary<string, ListViewItem> gl = new Dictionary<string, ListViewItem>();

            UsersAndGroups.Items.Clear();
            foreach (UserListUser u in m_connection.EnumerateUsers().Items)
            {
                ListViewItem lvi = new ListViewItem(new string[] { u.FullName, u.Description, StatusNames[IHUSER] }, IHUSER);
                lvi.Tag = u;
                UsersAndGroups.Items.Add(lvi);
                ul.Add(u.Name, lvi);
            }

            foreach (GroupListGroup g in m_connection.EnumerateGroups().Group)
            {
                ListViewItem lvi = new ListViewItem(new string[] { g.Name, g.Description, StatusNames[IHGROUP] }, IHGROUP);
                lvi.Tag = g;
                UsersAndGroups.Items.Add(lvi);
                gl.Add(g.Name, lvi);
            }

            if (m_resourceId.IsFolder)
            {
                m_folderHeader = m_connection.GetFolderHeader(m_resourceId);
                if (m_folderHeader.Security.Users != null && m_folderHeader.Security.Users.User != null)
                    foreach (ResourceSecurityTypeUsersUser u in m_folderHeader.Security.Users.User)
                        if (ul.ContainsKey(u.Name))
                            UpdateListItem(u, ul[u.Name]);

                if (m_folderHeader.Security.Groups != null && m_folderHeader.Security.Groups.Group != null)
                    foreach (ResourceSecurityTypeGroupsGroup g in m_folderHeader.Security.Groups.Group)
                        if (gl.ContainsKey(g.Name))
                            UpdateListItem(g, gl[g.Name]);

                UseInherited.Checked = m_folderHeader.Security.Inherited;
                tabControl1.TabPages.Remove(WMSTab);
                tabControl1.TabPages.Remove(WFSTab);
                tabControl1.TabPages.Remove(CustomTab);
            }
            else
            {
                m_resourceHeader = m_connection.GetResourceHeader(m_resourceId);
                if (m_resourceHeader.Security.Users != null && m_resourceHeader.Security.Users.User != null)
                    foreach (ResourceSecurityTypeUsersUser u in m_resourceHeader.Security.Users.User)
                        if (ul.ContainsKey(u.Name))
                            UpdateListItem(u, ul[u.Name]);

                if (m_resourceHeader.Security.Groups != null && m_resourceHeader.Security.Groups.Group != null)
                    foreach (ResourceSecurityTypeGroupsGroup g in m_resourceHeader.Security.Groups.Group)
                        if (gl.ContainsKey(g.Name))
                            UpdateListItem(g, gl[g.Name]);

                UseInherited.Checked = m_resourceHeader.Security.Inherited;
                if (m_resourceId.Extension != "LayerDefinition")
                    tabControl1.TabPages.Remove(WMSTab);
                else
                    UpdateWMSDisplay();

                if (m_resourceId.Extension != "FeatureSource")
                    tabControl1.TabPages.Remove(WFSTab);
                else
                    UpdateWFSDisplay();
            }

            if (tabControl1.TabCount == 1)
            {
                foreach (Control c in new System.Collections.ArrayList(tabControl1.TabPages[0].Controls))
                {
                    tabControl1.Controls.Remove(c);
                    this.Controls.Add(c);
                }

                this.Controls.Remove(tabControl1);
            }

            this.Text = m_resourceId;
            UseInherited_CheckedChanged(null, null);

        }

        private void UpdateWFSDisplay()
        {
            try
            {
                m_isUpdating = true;
                if (m_resourceHeader.Metadata != null && m_resourceHeader.Metadata.Simple != null && m_resourceHeader.Metadata.Simple.Property != null)
                {
                    WFSTitle.Text = m_resourceHeader.Metadata.Simple.Property["_Title"];
                    WFSKeywords.Text = m_resourceHeader.Metadata.Simple.Property["_Keywords"];
                    WFSAbstract.Text = m_resourceHeader.Metadata.Simple.Property["_Abstract"];
                    WFSMetadata.Text = m_resourceHeader.Metadata.Simple.Property["_ExtendedMetadata"];

                    WFSPrimarySRS.Text = m_resourceHeader.Metadata.Simple.Property["_PrimarySRS"];
                    WFSOtherSRS.Text = m_resourceHeader.Metadata.Simple.Property["_OtherSRS"];
                    WFSAvalible.Checked = m_resourceHeader.Metadata.Simple.Property["_IsPublished"] == "1";
                    WFSBounds.Text = m_resourceHeader.Metadata.Simple.Property["_Bounds"];
                }
                else
                {
                    WFSTitle.Text =
                    WFSKeywords.Text =
                    WFSAbstract.Text =
                    WFSMetadata.Text = "";

                    WFSPrimarySRS.Text =
                    WFSOtherSRS.Text =
                    WFSBounds.Text = "";
                    WFSAvalible.Checked = false;
                }
            }
            finally
            {
                m_isUpdating = false;
            }

            UpdateCustomDisplay();
        }

        private void UpdateWMSDisplay()
        {
            try
            {
                m_isUpdating = true;
                if (m_resourceHeader.Metadata != null && m_resourceHeader.Metadata.Simple != null && m_resourceHeader.Metadata.Simple.Property != null)
                {
                    WMSTitle.Text = m_resourceHeader.Metadata.Simple.Property["_Title"];
                    WMSKeyWords.Text = m_resourceHeader.Metadata.Simple.Property["_Keywords"];
                    WMSAbstract.Text = m_resourceHeader.Metadata.Simple.Property["_Abstract"];
                    WMSMetadata.Text = m_resourceHeader.Metadata.Simple.Property["_ExtendedMetadata"];

                    WMSQueryable.Checked = m_resourceHeader.Metadata.Simple.Property["_Queryable"] == "1";
                    WMSOpaque.Checked = m_resourceHeader.Metadata.Simple.Property["_Opaque"] == "1";
                    WMSAvalible.Checked = m_resourceHeader.Metadata.Simple.Property["_IsPublished"] == "1";
                    WMSBounds.Text = m_resourceHeader.Metadata.Simple.Property["_Bounds"];
                }
                else
                {
                    WMSTitle.Text =
                    WMSKeyWords.Text =
                    WMSAbstract.Text =
                    WMSMetadata.Text = "";

                    WMSQueryable.Checked =
                    WMSOpaque.Checked =
                    WMSAvalible.Checked = false;

                    WMSBounds.Text = "";
                }
            }
            finally
            {
                m_isUpdating = false;
            }
            UpdateCustomDisplay();
        }

        private void UpdateCustomDisplay()
        {
            dataGridView1.Rows.Clear();
            if (m_resourceHeader.Metadata != null && m_resourceHeader.Metadata.Simple != null && m_resourceHeader.Metadata.Simple.Property != null)
            {
                foreach (ResourceDocumentHeaderTypeMetadataSimpleProperty prop in m_resourceHeader.Metadata.Simple.Property)
                    dataGridView1.Rows.Add(new string[] { prop.Name, prop.Value });
            }
        }

        private void UseInherited_CheckedChanged(object sender, EventArgs e)
        {
            UsersAndGroups.Enabled = !UseInherited.Checked;
        }

        private void UpdateListItem(ResourceSecurityTypeUsersUser u, ListViewItem lvi)
        {
            if (u == null)
                lvi.ImageIndex = IHUSER;

            switch (u.Permissions)
            {
                case PermissionsType.rw:
                    lvi.ImageIndex = RWUSER;
                    break;
                case PermissionsType.r:
                    lvi.ImageIndex = ROUSER;
                    break;
                case PermissionsType.n:
                    lvi.ImageIndex = NOUSER;
                    break;
            }

            lvi.SubItems[2].Text = StatusNames[lvi.ImageIndex];
        }

        private void UpdateListItem(ResourceSecurityTypeGroupsGroup g, ListViewItem lvi)
        {
            if (g == null)
                lvi.ImageIndex = IHGROUP;

            switch (g.Permissions)
            {
                case PermissionsType.rw:
                    lvi.ImageIndex = RWGROUP;
                    break;
                case PermissionsType.r:
                    lvi.ImageIndex = ROGROUP;
                    break;
                case PermissionsType.n:
                    lvi.ImageIndex = NOGROUP;
                    break;
            }
            lvi.SubItems[2].Text = StatusNames[lvi.ImageIndex];
        }

        private void ClearHeaderButton_Click(object sender, EventArgs e)
        {
            m_resourceHeader.Metadata = null;
            UpdateWMSDisplay();
        }

        private void WMSTitle_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Title"] = WMSTitle.Text;
        }

        private void EnsureMetaData()
        {
            if (m_resourceHeader != null)
            {
                if (m_resourceHeader.Metadata == null)
                    m_resourceHeader.Metadata = new ResourceDocumentHeaderTypeMetadata();
                if (m_resourceHeader.Metadata.Simple == null)
                    m_resourceHeader.Metadata.Simple = new ResourceDocumentHeaderTypeMetadataSimple();
                if (m_resourceHeader.Metadata.Simple.Property == null)
                    m_resourceHeader.Metadata.Simple.Property = new ResourceDocumentHeaderTypeMetadataSimplePropertyCollection();

                if (m_resourceId.Extension == "LayerDefinition")
                {
                    if (m_resourceHeader.Metadata.Simple.Property["_Title"] == null)
                        m_resourceHeader.Metadata.Simple.Property["_Title"] = "";
                    if (m_resourceHeader.Metadata.Simple.Property["_Keywords"] == null)
                        m_resourceHeader.Metadata.Simple.Property["_Keywords"] = "";
                    if (m_resourceHeader.Metadata.Simple.Property["_Abstract"] == null)
                        m_resourceHeader.Metadata.Simple.Property["_Abstract"] = "";
                    if (m_resourceHeader.Metadata.Simple.Property["_ExtendedMetadata"] == null)
                        m_resourceHeader.Metadata.Simple.Property["_ExtendedMetadata"] = "";

                    if (m_resourceHeader.Metadata.Simple.Property["_Queryable"] == null)
                        m_resourceHeader.Metadata.Simple.Property["_Queryable"] = "0";
                    if (m_resourceHeader.Metadata.Simple.Property["_Opaque"] == null)
                        m_resourceHeader.Metadata.Simple.Property["_Opaque"] = "0";
                    if (m_resourceHeader.Metadata.Simple.Property["_IsPublished"] == null)
                        m_resourceHeader.Metadata.Simple.Property["_IsPublished"] = "0";
                    if (m_resourceHeader.Metadata.Simple.Property["_Bounds"] == null)
                        m_resourceHeader.Metadata.Simple.Property["_Bounds"] = "";
                }
            }
        }

        private void WMSKeyWords_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Keywords"] = WMSKeyWords.Text;
        }

        private void WMSAbstract_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Abstract"] = WMSAbstract.Text;
        }

        private void WMSMetadata_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_ExtendedMetadata"] = WMSMetadata.Text;
        }

        private void WMSAvalible_CheckedChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_IsPublished"] = WMSAvalible.Checked ? "1" : "0";
        }

        private void WMSBounds_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Bounds"] = WMSBounds.Text;
        }

        private void WMSQueryable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Queryable"] = WMSQueryable.Checked ? "1" : "0";
        }

        private void WMSOpaque_CheckedChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Opaque"] = WMSOpaque.Checked ? "1" : "0";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == CustomTab)
                UpdateCustomDisplay();
            else if (tabControl1.SelectedTab == WMSTab)
                UpdateWMSDisplay();
            else if (tabControl1.SelectedTab == WFSTab)
            {
                FillCoordSysLists();
                UpdateWFSDisplay();
            }
        }

        private void FillCoordSysLists()
        {
            if (WFSPrimarySRS.Items.Count == 0)
            {
                try
                {
                    List<string> items = new List<string>();
                    foreach (OSGeo.MapGuide.MaestroAPI.CoordinateSystem.CoordSys c in m_connection.CoordinateSystem.Coordsys)
                        if (c.Code.StartsWith("EPSG:"))
                            items.Add(c.Code);

                    m_srslist = items.ToArray();
                    try
                    {
                        WFSPrimarySRS.BeginUpdate();
                        WFSPrimarySRS.Items.AddRange(m_srslist);
                    }
                    finally
                    {
                        WFSPrimarySRS.EndUpdate();
                    }

                    try
                    {
                        WFSOtherSRS.BeginUpdate();
                        WFSOtherSRS.Items.AddRange(m_srslist);
                    }
                    finally
                    {
                        WFSOtherSRS.EndUpdate();
                    }
                }
                catch
                {
                }
            }
        }

        private void WFSTitle_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Title"] = WFSTitle.Text;
        }

        private void WFSKeywords_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Keywords"] = WFSKeywords.Text;
        }

        private void WFSAbstract_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Abstract"] = WFSAbstract.Text;
        }

        private void WFSMetadata_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_ExtendedMetadata"] = WFSMetadata.Text;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_IsPublished"] = WFSAvalible.Checked ? "1" : "0";

        }

        private void WFSPrimarySRS_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_PrimarySRS"] = WFSPrimarySRS.Text;
        }

        private void WFSOtherSRS_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_OtherSRS"] = WFSOtherSRS.Text;
        }

        private void WFSBounds_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Bounds"] = WFSBounds.Text;
        }

        private void WFSClearHeaderButton_Click(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            m_resourceHeader.Metadata = null;
            UpdateWFSDisplay();
        }

        private void ClearHeaderButton_Click_1(object sender, EventArgs e)
        {
            m_resourceHeader.Metadata = null;
            UpdateCustomDisplay();
        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property.Clear();

            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                string k = dr.Cells[0].Value == null || dr.Cells[0].Value == DBNull.Value ? "" : dr.Cells[0].Value.ToString();
                string v = dr.Cells[1].Value == null || dr.Cells[1].Value == DBNull.Value ? "" : dr.Cells[1].Value.ToString();

                if (!string.IsNullOrEmpty(k))
                    m_resourceHeader.Metadata.Simple.Property[k] = v;
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //Update security info
                if (m_resourceId.IsFolder)
                {
                    m_folderHeader.Security.Inherited = UseInherited.Checked;
                    if (m_folderHeader.Security.Inherited)
                    {
                        m_folderHeader.Security.Groups = null;
                        m_folderHeader.Security.Users = null;
                    }
                    else
                    {
                        m_folderHeader.Security.Groups = new ResourceSecurityTypeGroups();
                        m_folderHeader.Security.Users = new ResourceSecurityTypeUsers();
                        m_folderHeader.Security.Groups.Group = new ResourceSecurityTypeGroupsGroupCollection();
                        m_folderHeader.Security.Users.User = new ResourceSecurityTypeUsersUserCollection();
                        ReadSecurityData(m_folderHeader.Security.Groups.Group, m_folderHeader.Security.Users.User);
                    }
                }
                else
                {
                    m_resourceHeader.Security.Inherited = UseInherited.Checked;
                    if (m_resourceHeader.Security.Inherited)
                    {
                        m_resourceHeader.Security.Groups = null;
                        m_resourceHeader.Security.Users = null;
                    }
                    else
                    {
                        m_resourceHeader.Security.Groups = new ResourceSecurityTypeGroups();
                        m_resourceHeader.Security.Users = new ResourceSecurityTypeUsers();
                        m_resourceHeader.Security.Groups.Group = new ResourceSecurityTypeGroupsGroupCollection();
                        m_resourceHeader.Security.Users.User = new ResourceSecurityTypeUsersUserCollection();
                        ReadSecurityData(m_resourceHeader.Security.Groups.Group, m_resourceHeader.Security.Users.User);
                    }
                }

                //Save header
                if (m_resourceId.IsFolder)
                    m_connection.SetFolderHeader(m_resourceId, m_folderHeader);
                else
                    m_connection.SetResourceHeader(m_resourceId, m_resourceHeader);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(m_globalizor.Translate("Failed to save the resource properties: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ReadSecurityData(ResourceSecurityTypeGroupsGroupCollection groups, ResourceSecurityTypeUsersUserCollection users)
        {
            foreach (ListViewItem lvi in UsersAndGroups.Items)
                if (lvi.Tag as UserListUser != null)
                {
                    ResourceSecurityTypeUsersUser u = new ResourceSecurityTypeUsersUser();
                    u.Name = (lvi.Tag as UserListUser).Name;
                    if (lvi.ImageIndex == RWUSER)
                        u.Permissions = PermissionsType.rw;
                    else if (lvi.ImageIndex == ROUSER)
                        u.Permissions = PermissionsType.r;
                    else if (lvi.ImageIndex == NOUSER)
                        u.Permissions = PermissionsType.n;
                    else
                        continue;
                    users.Add(u);
                }
                else if (lvi.Tag as GroupListGroup != null && lvi.ImageIndex != IHGROUP)
                {
                    ResourceSecurityTypeGroupsGroup g = new ResourceSecurityTypeGroupsGroup();
                    g.Name = (lvi.Tag as GroupListGroup).Name;
                    if (lvi.ImageIndex == RWGROUP)
                        g.Permissions = PermissionsType.rw;
                    else if (lvi.ImageIndex == ROGROUP)
                        g.Permissions = PermissionsType.r;
                    else if (lvi.ImageIndex == NOGROUP)
                        g.Permissions = PermissionsType.n;
                    else
                        continue;
                    groups.Add(g);
                }
        }

        private void securityContextMenu_Opening(object sender, CancelEventArgs e)
        {
            readWriteAccessToolStripMenuItem.Enabled =
                readOnlyAccessToolStripMenuItem.Enabled =
                denyAccessToolStripMenuItem.Enabled =
                inheritedAccessRightsToolStripMenuItem.Enabled =
                    UsersAndGroups.SelectedItems.Count > 0;
        }

        private void readWriteAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in UsersAndGroups.SelectedItems)
            {
                if (lvi.Tag as UserListUser != null)
                    lvi.ImageIndex = RWUSER;
                else
                    lvi.ImageIndex = RWGROUP;

                lvi.SubItems[2].Text = StatusNames[lvi.ImageIndex];
            }
        }

        private void readOnlyAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in UsersAndGroups.SelectedItems)
            {
                if (lvi.Tag as UserListUser != null)
                    lvi.ImageIndex = ROUSER;
                else
                    lvi.ImageIndex = ROGROUP;

                lvi.SubItems[2].Text = StatusNames[lvi.ImageIndex];
            }

        }

        private void denyAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in UsersAndGroups.SelectedItems)
            {
                if (lvi.Tag as UserListUser != null)
                    lvi.ImageIndex = NOUSER;
                else
                    lvi.ImageIndex = NOGROUP;

                lvi.SubItems[2].Text = StatusNames[lvi.ImageIndex];
            }

        }

        private void inheritedAccessRightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in UsersAndGroups.SelectedItems)
            {
                if (lvi.Tag as UserListUser != null)
                    lvi.ImageIndex = IHUSER;
                else
                    lvi.ImageIndex = IHGROUP;

                lvi.SubItems[2].Text = StatusNames[lvi.ImageIndex];
            }

        }

        private void EditWMSBounds_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_srslist == null)
                {
                    List<string> items = new List<string>();
                    foreach (OSGeo.MapGuide.MaestroAPI.CoordinateSystem.CoordSys c in m_connection.CoordinateSystem.Coordsys)
                        if (c.Code.StartsWith("EPSG:"))
                            items.Add(c.Code);

                    m_srslist = items.ToArray();
                }
            }
            catch
            {
            }

            BoundsPicker bp = new BoundsPicker(WMSBounds.Text, m_srslist);
            if (bp.ShowDialog(this) == DialogResult.OK)
                WMSBounds.Text = bp.SRSBounds;
        }

        private void EditWFSBounds_Click(object sender, EventArgs e)
        {
            BoundsPicker bp = new BoundsPicker(WFSBounds.Text, null);
            if (bp.ShowDialog(this) == DialogResult.OK)
                WFSBounds.Text = bp.SRSBounds;

        }
    }

}