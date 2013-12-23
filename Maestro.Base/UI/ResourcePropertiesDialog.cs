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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.Core;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI.Schema;
using Maestro.Editors.Common;
using Maestro.Base.Services;

namespace Maestro.Base.UI
{
    /// <summary>
    /// A dialog that displays various properties of a given resource
    /// </summary>
    public partial class ResourcePropertiesDialog : Form
    {
        private ResourcePropertiesDialog()
        {
            InitializeComponent();
        }

        private IResourceIconCache m_icons;
        private IServerConnection m_connection;
        private ResourceIdentifier m_resourceId;

        private ResourceDocumentHeaderType m_resourceHeader;
        private ResourceFolderHeaderType m_folderHeader;

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
            Strings.ResProp_ReadWriteAccess,
            Strings.ResProp_ReadOnlyAccess,
            Strings.ResProp_NoAccess,
            Strings.ResProp_ReadWriteAccess,
            Strings.ResProp_ReadOnlyAccess,
            Strings.ResProp_NoAccess,
            Strings.ResProp_InheritedAccess,
            Strings.ResProp_InheritedAccess
        };

        private object m_lock = new object();
        private volatile System.Threading.Thread m_backgroundThread = null;
        private bool m_hasLoadedRefs = false;
        
        private string m_openResource = null;

        /// <summary>
        /// A resource that the main form should open after this form has closed
        /// </summary>
        public string OpenResource { get { return m_openResource; } }

        private OpenResourceManager _openMgr;
        private ISiteExplorer _siteExp;

        /// <summary>
        /// Initializes a new instance of the ResourcePropertiesDialog class
        /// </summary>
        /// <param name="icons"></param>
        /// <param name="connection"></param>
        /// <param name="resourceId"></param>
        /// <param name="openMgr"></param>
        /// <param name="siteExp"></param>
        public ResourcePropertiesDialog(IResourceIconCache icons, IServerConnection connection, string resourceId, OpenResourceManager openMgr, ISiteExplorer siteExp)
            : this()
        {
            m_connection = connection;
            m_resourceId = resourceId;
            ResourceID.Text = resourceId;
            m_icons = icons;
            _openMgr = openMgr;
            _siteExp = siteExp;

            InReferenceList.SmallImageList = OutReferenceList.SmallImageList = icons.SmallImageList;
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Form.Load event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            Dictionary<string, ListViewItem> ul = new Dictionary<string, ListViewItem>();
            Dictionary<string, ListViewItem> gl = new Dictionary<string, ListViewItem>();

            UsersAndGroups.Items.Clear();
            var site = (ISiteService)m_connection.GetService((int)ServiceType.Site);

            foreach (UserListUser u in site.EnumerateUsers().Items)
            {
                ListViewItem lvi = new ListViewItem(new string[] { u.FullName, u.Description, StatusNames[IHUSER] }, IHUSER);
                lvi.Tag = u;
                UsersAndGroups.Items.Add(lvi);
                ul.Add(u.Name, lvi);
            }

            foreach (GroupListGroup g in site.EnumerateGroups().Group)
            {
                ListViewItem lvi = new ListViewItem(new string[] { g.Name, g.Description, StatusNames[IHGROUP] }, IHGROUP);
                lvi.Tag = g;
                UsersAndGroups.Items.Add(lvi);
                gl.Add(g.Name, lvi);
            }

            if (m_resourceId.IsFolder)
            {
                m_folderHeader = m_connection.ResourceService.GetFolderHeader(m_resourceId);
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
                tabControl1.TabPages.Remove(ReferenceTab);
            }
            else
            {
                m_resourceHeader = m_connection.ResourceService.GetResourceHeader(m_resourceId);
                if (m_resourceHeader.Security.Users != null && m_resourceHeader.Security.Users.User != null)
                    foreach (ResourceSecurityTypeUsersUser u in m_resourceHeader.Security.Users.User)
                        if (ul.ContainsKey(u.Name))
                            UpdateListItem(u, ul[u.Name]);

                if (m_resourceHeader.Security.Groups != null && m_resourceHeader.Security.Groups.Group != null)
                    foreach (ResourceSecurityTypeGroupsGroup g in m_resourceHeader.Security.Groups.Group)
                        if (gl.ContainsKey(g.Name))
                            UpdateListItem(g, gl[g.Name]);

                UseInherited.Checked = m_resourceHeader.Security.Inherited;
                if (m_resourceId.Extension != ResourceTypes.LayerDefinition.ToString())
                    tabControl1.TabPages.Remove(WMSTab);
                else
                    UpdateWMSDisplay();

                if (m_resourceId.Extension != ResourceTypes.FeatureSource.ToString())
                    tabControl1.TabPages.Remove(WFSTab);
                else
                    UpdateWFSDisplay();
            }

            //Hide the tabControl if it only has one tab
            if (tabControl1.TabCount == 1)
            {
                foreach (Control c in new System.Collections.ArrayList(tabControl1.TabPages[0].Controls))
                {
                    tabControl1.Controls.Remove(c);
                    c.Top += tabControl1.Top;
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
                    var props = m_resourceHeader.Metadata.GetProperties();

                    WFSTitle.Text = props["_Title"]; //NOXLATE
                    WFSKeywords.Text = props["_Keywords"]; //NOXLATE
                    WFSAbstract.Text = props["_Abstract"]; //NOXLATE
                    WFSMetadata.Text = props["_ExtendedMetadata"]; //NOXLATE

                    txtPrimarySRS.Text = props["_PrimarySRS"]; //NOXLATE
                    txtOtherSRS.Text = props["_OtherSRS"]; //NOXLATE
                    WFSAvailable.Checked = props["_IsPublished"] == "1"; //NOXLATE
                    WFSBounds.Text = props["_Bounds"]; //NOXLATE
                }
                else
                {
                    WFSTitle.Text =
                    WFSKeywords.Text =
                    WFSAbstract.Text =
                    WFSMetadata.Text = string.Empty;

                    txtPrimarySRS.Text =
                    txtOtherSRS.Text =
                    WFSBounds.Text = string.Empty;
                    WFSAvailable.Checked = false;
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
                    var props = m_resourceHeader.Metadata.GetProperties();
                    WMSTitle.Text = props["_Title"]; //NOXLATE
                    WMSKeyWords.Text = props["_Keywords"]; //NOXLATE
                    WMSAbstract.Text = props["_Abstract"]; //NOXLATE
                    WMSMetadata.Text = props["_ExtendedMetadata"]; //NOXLATE

                    WMSQueryable.Checked = props["_Queryable"] == "1"; //NOXLATE
                    WMSOpaque.Checked = props["_Opaque"] == "1"; //NOXLATE
                    WMSAvalible.Checked = props["_IsPublished"] == "1"; //NOXLATE
                    WMSBounds.Text = props["_Bounds"]; //NOXLATE
                }
                else
                {
                    WMSTitle.Text =
                    WMSKeyWords.Text =
                    WMSAbstract.Text =
                    WMSMetadata.Text = string.Empty;

                    WMSQueryable.Checked =
                    WMSOpaque.Checked =
                    WMSAvalible.Checked = false;

                    WMSBounds.Text = string.Empty;
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
            UpdateCustomDisplay();
        }

        private void WMSTitle_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_Title", WMSTitle.Text); //NOXLATE
        }

        private void EnsureMetaData()
        {
            if (m_resourceHeader != null)
            {
                if (m_resourceHeader.Metadata == null)
                    m_resourceHeader.Metadata = ObjectFactory.CreateMetadata();

                /*
                if (m_resourceHeader.Metadata.Simple == null)
                    m_resourceHeader.Metadata.Simple = new ResourceDocumentHeaderTypeMetadataSimple();
                if (m_resourceHeader.Metadata.Simple.Property == null)
                    m_resourceHeader.Metadata.Simple.Property = new ResourceDocumentHeaderTypeMetadataSimplePropertyCollection();
                */

                if (m_resourceId.Extension == ResourceTypes.LayerDefinition.ToString())
                {
                    var props = m_resourceHeader.Metadata.GetProperties();
                    bool apply = false;

                    if (props["_Title"] == null) //NOXLATE
                    {
                        props["_Title"] = string.Empty; //NOXLATE
                        apply = true;
                    }

                    if (props["_Keywords"] == null) //NOXLATE
                    {
                        props["_Keywords"] = string.Empty; //NOXLATE
                        apply = true;
                    }

                    if (props["_Abstract"] == null) //NOXLATE
                    {
                        props["_Abstract"] = string.Empty; //NOXLATE
                        apply = true;
                    }

                    if (props["_ExtendedMetadata"] == null) //NOXLATE
                    {
                        props["_ExtendedMetadata"] = string.Empty; //NOXLATE
                        apply = true;
                    }

                    if (props["_Queryable"] == null) //NOXLATE
                    {
                        props["_Queryable"] = "0"; //NOXLATE
                        apply = true;
                    }

                    if (props["_Opaque"] == null) //NOXLATE
                    {
                        props["_Opaque"] = "0"; //NOXLATE
                        apply = true;
                    }

                    if (props["_IsPublished"] == null) //NOXLATE
                    {
                        props["_IsPublished"] = "0"; //NOXLATE
                        apply = true;
                    }

                    if (props["_Bounds"] == null) //NOXLATE
                    {
                        props["_Bounds"] = string.Empty; //NOXLATE
                        apply = true;
                    }

                    if (apply)
                        m_resourceHeader.Metadata.ApplyProperties(props);
                }
            }
        }

        private void WMSKeyWords_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_Keywords", WMSKeyWords.Text); //NOXLATE
        }

        private void WMSAbstract_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_Abstract", WMSAbstract.Text); //NOXLATE
        }

        private void WMSMetadata_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_ExtendedMetadata", WMSMetadata.Text); //NOXLATE
        }

        private void WMSAvalible_CheckedChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_IsPublished", Convert.ToInt32(WMSAvalible.Checked).ToString()); //NOXLATE
        }

        private void WMSBounds_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_Bounds", WMSBounds.Text); //NOXLATE
        }

        private void WMSQueryable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_Queryable", Convert.ToInt32(WMSQueryable.Checked).ToString()); //NOXLATE
        }

        private void WMSOpaque_CheckedChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_Opaque", Convert.ToInt32(WMSOpaque.Checked).ToString()); //NOXLATE
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == CustomTab)
                UpdateCustomDisplay();
            else if (tabControl1.SelectedTab == WMSTab)
                UpdateWMSDisplay();
            else if (tabControl1.SelectedTab == WFSTab)
            {
                UpdateWFSDisplay();
            }
            else if (tabControl1.SelectedTab == ReferenceTab)
            {
                if (!m_hasLoadedRefs)
                {
                    LoadingReferences.Visible = true;
                    ReferenceWorker.RunWorkerAsync(m_resourceId);
                }
            }
        }

        private void WFSTitle_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_Title", WFSTitle.Text); //NOXLATE
        }

        private void WFSKeywords_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_Keywords", WFSKeywords.Text); //NOXLATE
        }

        private void WFSAbstract_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_Abstract", WFSAbstract.Text); //NOXLATE
        }

        private void WFSMetadata_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_ExtendedMetadata", WFSMetadata.Text); //NOXLATE
        }

        private void WFSAvailable_CheckedChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_IsPublished", Convert.ToInt32(WFSAvailable.Checked).ToString()); //NOXLATE
        }

        private void WFSPrimarySRS_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_PrimarySRS", txtPrimarySRS.Text); //NOXLATE
        }

        private void WFSOtherSRS_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_OtherSRS", txtOtherSRS.Text); //NOXLATE
        }

        private void WFSBounds_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            EnsureMetaData();
            m_resourceHeader.Metadata.SetProperty("_Bounds", WFSBounds.Text); //NOXLATE
        }

        private void WFSClearHeaderButton_Click(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;
            m_resourceHeader.Metadata = null;
            UpdateWFSDisplay();
        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            EnsureMetaData();
            m_resourceHeader.Metadata.Simple.Property.Clear();

            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                string k = dr.Cells[0].Value == null || dr.Cells[0].Value == DBNull.Value ? string.Empty : dr.Cells[0].Value.ToString();
                string v = dr.Cells[1].Value == null || dr.Cells[1].Value == DBNull.Value ? string.Empty : dr.Cells[1].Value.ToString();

                if (!string.IsNullOrEmpty(k))
                    m_resourceHeader.Metadata.SetProperty(k, v);
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            m_openResource = null;

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
                        m_folderHeader.Security.Groups = ObjectFactory.CreateSecurityGroup();
                        m_folderHeader.Security.Users = ObjectFactory.CreateSecurityUser();
                        //m_folderHeader.Security.Groups.Group = new ResourceSecurityTypeGroupsGroupCollection();
                        //m_folderHeader.Security.Users.User = new ResourceSecurityTypeUsersUserCollection();
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
                        m_resourceHeader.Security.Groups = ObjectFactory.CreateSecurityGroup();
                        m_resourceHeader.Security.Users = ObjectFactory.CreateSecurityUser();
                        //m_resourceHeader.Security.Groups.Group = new ResourceSecurityTypeGroupsGroupCollection();
                        //m_resourceHeader.Security.Users.User = new ResourceSecurityTypeUsersUserCollection();
                        ReadSecurityData(m_resourceHeader.Security.Groups.Group, m_resourceHeader.Security.Users.User);
                    }
                }

                //Save header
                if (m_resourceId.IsFolder)
                    m_connection.ResourceService.SetFolderHeader(m_resourceId, m_folderHeader);
                else
                    m_connection.ResourceService.SetResourceHeader(m_resourceId, m_resourceHeader);
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                MessageBox.Show(this, string.Format(Strings.ResProp_SaveError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ReadSecurityData(IList<ResourceSecurityTypeGroupsGroup> groups, IList<ResourceSecurityTypeUsersUser> users)
        {
            foreach (ListViewItem lvi in UsersAndGroups.Items)
            {
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
                    foreach (OSGeo.MapGuide.MaestroAPI.CoordinateSystem.CoordinateSystemDefinitionBase c in m_connection.CoordinateSystemCatalog.Coordsys)
                    {
                        if (c.Code.StartsWith("EPSG:")) //NOXLATE
                            items.Add(c.Code);
                    }

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

        private void AutoGenerateWMSBounds_Click(object sender, EventArgs e)
        {
            try
            {
                m_isUpdating = true;
                string srs = "EPSG:????"; //NOXLATE
                string bounds = WMSBounds.Text;
                bool warnedEPSG = false;

                try
                {
                    if (!string.IsNullOrEmpty(bounds))
                    {
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        if (bounds.Trim().StartsWith("&lt;")) //NOXLATE
                            bounds = System.Web.HttpUtility.HtmlDecode(bounds);
                        bounds = "<root>" + bounds + "</root>"; //NOXLATE
                        doc.LoadXml(bounds);
                        System.Xml.XmlNode root = doc["root"]; //NOXLATE
                        if (root["Bounds"] != null) //NOXLATE
                        {
                            if (root["Bounds"].Attributes["SRS"] != null) //NOXLATE
                                srs = root["Bounds"].Attributes["SRS"].Value; //NOXLATE
                        }
                        else
                            throw new Exception(Strings.ResProp_MissingBoundsError);
                    }
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    warnedEPSG = true;
                    MessageBox.Show(this, string.Format(Strings.ResProp_BoundsDecodeError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                System.Globalization.CultureInfo ic = System.Globalization.CultureInfo.InvariantCulture;
                ILayerDefinition ldef = (ILayerDefinition)m_connection.ResourceService.GetResource(m_resourceId);
                string csWkt;
                var env = ldef.GetSpatialExtent(true, out csWkt);
                //TODO: Convert wkt to EPSG code and use that?
                //TODO: Convert to lon/lat

                bounds = "<Bounds west=\"" + env.MinX.ToString(ic) + "\" east=\"" + env.MaxX.ToString(ic) + "\" south=\"" + env.MinY.ToString(ic) + "\" north=\"" + env.MaxY.ToString(ic) + "\" "; //NOXLATE
                bounds += " SRS=\"" + srs + "\""; //NOXLATE
                bounds += " />"; //NOXLATE

                m_isUpdating = false;
                WMSBounds.Text = bounds;

                if ((srs == string.Empty || srs == "EPSG:????") && !warnedEPSG) //NOXLATE
                {
                    MessageBox.Show(this, Strings.ResProp_EpsgMissingWarning, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    WMSBounds.SelectionStart = WMSBounds.Text.IndexOf("SRS=\"") + "SRS=\"".Length; //NOXLATE
                    WMSBounds.SelectionLength = WMSBounds.Text.IndexOf("\"", WMSBounds.SelectionStart) - WMSBounds.SelectionStart;
                    WMSBounds.ScrollToCaret();
                    WMSBounds.Focus();
                }
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                MessageBox.Show(this, string.Format(Strings.ResProp_WMSBoundsReadError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_isUpdating = false;
            }
        }

        private void AutoGenerateWFSBounds_Click(object sender, EventArgs e)
        {
            try
            {
                m_isUpdating = true;
                System.Globalization.CultureInfo ic = System.Globalization.CultureInfo.InvariantCulture;
                IFeatureSource fs = (IFeatureSource)m_connection.ResourceService.GetResource(m_resourceId);
                bool failures = false;

                IEnvelope env = null;
                foreach (ClassDefinition scm in fs.Describe().AllClasses)
                {
                    foreach (var col in scm.Properties)
                    {
                        if (col.Type == OSGeo.MapGuide.MaestroAPI.Schema.PropertyDefinitionType.Geometry ||
                            col.Type == OSGeo.MapGuide.MaestroAPI.Schema.PropertyDefinitionType.Raster)
                        {
                            try
                            {
                                IEnvelope re = m_connection.FeatureService.GetSpatialExtent(fs.ResourceID, scm.Name, col.Name, true);
                                if (env == null)
                                    env = re;
                                else
                                    env.ExpandToInclude(re);
                            }
                            catch
                            {
                                failures = true;
                            }
                        }
                    }
                }

                if (env == null)
                    throw new Exception(failures ? Strings.ResProp_NoSpatialDataWithFailuresError : Strings.ResProp_NoSpatialDataError);

                m_isUpdating = false;
                WFSBounds.Text = "<Bounds west=\"" + env.MinX.ToString(ic) + "\" east=\"" + env.MaxX.ToString(ic) + "\" south=\"" + env.MinY.ToString(ic) + "\" north=\"" + env.MaxY.ToString(ic) + "\" />"; //NOXLATE
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                MessageBox.Show(this, string.Format(Strings.ResProp_WFSBoundsReadError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_isUpdating = false;
            }
        }

        private void ReferenceWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                lock(m_lock)
                    m_backgroundThread = System.Threading.Thread.CurrentThread;

                string resourceId = (ResourceIdentifier)e.Argument;

                List<string> lst = new List<string>();
                foreach (string s in  m_connection.ResourceService.EnumerateResourceReferences(resourceId).ResourceId)
                    if (!lst.Contains(s))
                        lst.Add(s);

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                using (var ms = m_connection.ResourceService.GetResourceXmlData(resourceId))
                {
                    doc.Load(ms);
                }

                List<KeyValuePair<System.Xml.XmlNode, string>> refs = Utility.GetResourceIdPointers(doc);
                List<string> r = new List<string>();
                foreach (KeyValuePair<System.Xml.XmlNode, string> s in refs)
                    if (!r.Contains(s.Value))
                        r.Add(s.Value);
                e.Result = new object[] { lst, r };
            }
            catch(System.Threading.ThreadAbortException)
            {
                System.Threading.Thread.ResetAbort();
                e.Cancel = true;
                return;
            }
            finally
            {
                lock(m_lock)
                    m_backgroundThread = null;
            }
        }

        private void ReferenceWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadingReferences.Visible = false;

            if (e.Cancelled)
            {
                this.Close();
                return;
            }
            if (e.Error != null || e.Result as object[] == null || (e.Result as object[]).Length != 2)
            {
                if (e.Error != null)
                    MessageBox.Show(this, string.Format(Strings.ResProp_ReferenceReadSpecificError, e.Error.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(this, Strings.ResProp_ReferenceReadError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            OutReferences.Enabled = InReferences.Enabled = true;

            List<string> l1 = ((object[])e.Result)[0] as List<string>;
            List<string> l2 = ((object[])e.Result)[1] as List<string>;

            foreach (string s in l2)
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                OutReferenceList.Items.Add(s, m_icons.GetImageIndexFromResourceID(s));
            }

            foreach (string s in l1)
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                InReferenceList.Items.Add(s, m_icons.GetImageIndexFromResourceID(s));
            }

            m_hasLoadedRefs = true;
        }

        private void OutReferenceList_DoubleClick(object sender, EventArgs e)
        {
            if (OutReferenceList.SelectedItems.Count == 1)
            {
                m_openResource = OutReferenceList.SelectedItems[0].Text;
                this.Close();
            }
        }

        private void InReferenceList_DoubleClick(object sender, EventArgs e)
        {
            if (InReferenceList.SelectedItems.Count == 1)
            {
                m_openResource = InReferenceList.SelectedItems[0].Text;
                this.Close();
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            m_openResource = null;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            lock (m_lock)
            {
                if (m_backgroundThread != null)
                {
                    this.Enabled = false;
                    m_backgroundThread.Abort();
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void btnPrimarySRS_Click(object sender, EventArgs e)
        {
            using (var picker = new CoordinateSystemPicker(m_connection.CoordinateSystemCatalog))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    var cs = picker.SelectedCoordSys;
                    if (!string.IsNullOrEmpty(cs.EPSG))
                    {
                        txtPrimarySRS.Text = "EPSG:" + cs.EPSG; //NOXLATE
                    }
                }
            }
        }

        private void btnOtherSRS_Click(object sender, EventArgs e)
        {
            using (var picker = new CoordinateSystemPicker(m_connection.CoordinateSystemCatalog))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    var cs = picker.SelectedCoordSys;
                    if (!string.IsNullOrEmpty(cs.EPSG))
                    {
                        txtOtherSRS.Text = "EPSG:" + cs.EPSG; //NOXLATE
                    }
                }
            }
        }

        private void referencedCopyResourceIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyInReference();
        }

        private void referencesCopyResourceIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyOutReference();
        }

        private void CopyInReference()
        {
            if (InReferenceList.SelectedItems.Count != 1)
                return;

            try
            {
                Clipboard.Clear();
                Clipboard.SetText(InReferenceList.SelectedItems[0].Text);
            }
            catch { }
        }

        private void CopyOutReference()
        {
            if (OutReferenceList.SelectedItems.Count != 1)
                return;

            try
            {
                Clipboard.Clear();
                Clipboard.SetText(OutReferenceList.SelectedItems[0].Text);
            }
            catch { }
        }

        private void OutReferenceList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.C)
                CopyOutReference();
        }

        private void InReferenceList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.C)
                CopyInReference();
        }

        private void ctxReferences_Opening(object sender, CancelEventArgs e)
        {
            if (OutReferenceList.SelectedItems.Count != 1)
                e.Cancel = true;
        }

        private void ctxReferenced_Opening(object sender, CancelEventArgs e)
        {
            if (InReferenceList.SelectedItems.Count != 1)
                e.Cancel = true;
        }

        private void OutReferenceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnReferencesOpenSelected.Enabled = (OutReferenceList.SelectedItems.Count > 0);
            btnReferencedByOpenSelected.Enabled = (InReferenceList.SelectedItems.Count > 0);
        }

        private void InReferenceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnReferencedByOpenSelected.Enabled = (InReferenceList.SelectedItems.Count > 0);
            btnReferencesOpenSelected.Enabled = (OutReferenceList.SelectedItems.Count > 0);
        }

        private void btnReferencesOpenSelected_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in OutReferenceList.SelectedItems)
            {
                _openMgr.Open(item.Text, m_connection, false, _siteExp);
            }
        }

        private void btnReferencedByOpenSelected_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in InReferenceList.SelectedItems)
            {
                _openMgr.Open(item.Text, m_connection, false, _siteExp);
            }
        }
    }
}
