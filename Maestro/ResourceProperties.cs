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

        private const int RWUSER = 0;
        private const int ROUSER = 1;
        private const int NOUSER = 3;
        private const int RWGROUP = 4;
        private const int ROGROUP = 5;
        private const int NOGROUP = 6;

        public ResourceProperties(ServerConnectionI connection, string resourceId)
            : this()
        {
            m_connection = connection;
            m_resourceId = resourceId;
        }

        private ResourceProperties()
        {
            InitializeComponent();
        }

        private void ResourceProperties_Load(object sender, EventArgs e)
        {
            Dictionary<string, ListViewItem> ul = new Dictionary<string,ListViewItem>();
            Dictionary<string, ListViewItem> gl = new Dictionary<string,ListViewItem>();

            UsersAndGroups.Items.Clear();
            foreach (UserListUser u in m_connection.EnumerateUsers().Items)
            {
                ListViewItem lvi = new ListViewItem(u.FullName, RWUSER);
                lvi.Tag = u;
                UsersAndGroups.Items.Add(lvi);
                ul.Add(lvi.Text, lvi);
            }

            foreach (GroupListGroup g in m_connection.EnumerateGroups().Group)
            {
                ListViewItem lvi = new ListViewItem(g.Name, RWGROUP);
                lvi.Tag = g;
                UsersAndGroups.Items.Add(lvi);
                gl.Add(lvi.Text, lvi);
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
        }

        private void UpdateWFSDisplay()
        {
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
            
            UpdateCustomDisplay();
        }

        private void UpdateWMSDisplay()
        {
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
        }

        private void UpdateListItem(ResourceSecurityTypeGroupsGroup g, ListViewItem lvi)
        {
            switch (g.Permissions)
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
        }

        private void ClearHeaderButton_Click(object sender, EventArgs e)
        {
            m_resourceHeader.Metadata = null;
            UpdateWMSDisplay();
        }

        private void WMSTitle_TextChanged(object sender, EventArgs e)
        {
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
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Keywords"] = WMSKeyWords.Text;
        }

        private void WMSAbstract_TextChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Abstract"] = WMSAbstract.Text;
        }

        private void WMSMetadata_TextChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_ExtendedMetadata"] = WMSMetadata.Text;
        }

        private void WMSAvalible_CheckedChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_IsPublished"] = WMSAvalible.Checked ? "1" : "0";
        }

        private void WMSBounds_TextChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Bounds"] = WMSBounds.Text;
        }

        private void WMSQueryable_CheckedChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Queryable"] = WMSQueryable.Checked ? "1" : "0";
        }

        private void WMSOpaque_CheckedChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Opaque"] = WMSOpaque.Checked ? "1" : "0";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == CustomTab)
                UpdateWMSDisplay();
            else if (tabControl1.SelectedTab == WMSTab)
                UpdateWFSDisplay();
            else if (tabControl1.SelectedTab == WFSTab)
                UpdateCustomDisplay();
        }

        private void WFSTitle_TextChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Title"] = WFSTitle.Text;
        }

        private void WFSKeywords_TextChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Keywords"] = WFSKeywords.Text;
        }

        private void WFSAbstract_TextChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Abstract"] = WFSAbstract.Text;
        }

        private void WFSMetadata_TextChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_ExtendedMetadata"] = WFSMetadata.Text;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_IsPublished"] = WFSAvalible.Checked ? "1" : "0";

        }

        private void WFSPrimarySRS_TextChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_PrimarySRS"] = WFSPrimarySRS.Text;
        }

        private void WFSOtherSRS_TextChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_OtherSRS"] = WFSOtherSRS.Text;
        }

        private void WFSBounds_TextChanged(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property["_Bounds"] = WFSBounds.Text;
        }

        private void WFSClearHeaderButton_Click(object sender, EventArgs e)
        {
            m_resourceHeader.Metadata = null;
            UpdateWFSDisplay();
        }

        private void ClearHeaderButton_Click_1(object sender, EventArgs e)
        {
            m_resourceHeader.Metadata = null;
            UpdateCustomDisplay();
        }
    }
}