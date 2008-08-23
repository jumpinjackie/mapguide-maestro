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
        private string m_resourceId;

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

            if (m_resourceId.EndsWith("/"))
            {
                ResourceFolderHeaderType header = m_connection.GetFolderHeader(m_resourceId);
                if (header.Security.Users != null && header.Security.Users.User != null)
                    foreach (ResourceSecurityTypeUsersUser u in header.Security.Users.User)
                        if (ul.ContainsKey(u.Name))
                            UpdateListItem(u, ul[u.Name]);

                if (header.Security.Groups != null && header.Security.Groups.Group != null)
                    foreach (ResourceSecurityTypeGroupsGroup g in header.Security.Groups.Group)
                        if (gl.ContainsKey(g.Name))
                            UpdateListItem(g, gl[g.Name]);

                UseInherited.Checked = header.Security.Inherited;
            }
            else
            {

                ResourceDocumentHeaderType header = m_connection.GetResourceHeader(m_resourceId);
                if (header.Security.Users != null && header.Security.Users.User != null)
                    foreach (ResourceSecurityTypeUsersUser u in header.Security.Users.User)
                        if (ul.ContainsKey(u.Name))
                            UpdateListItem(u, ul[u.Name]);

                if (header.Security.Groups != null && header.Security.Groups.Group != null)
                    foreach (ResourceSecurityTypeGroupsGroup g in header.Security.Groups.Group)
                        if (gl.ContainsKey(g.Name))
                            UpdateListItem(g, gl[g.Name]);
                
                UseInherited.Checked = header.Security.Inherited;
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
    }
}