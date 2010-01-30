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
using System.Windows.Forms;
using System.Collections.Generic;

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// An implementation of the interface avalible to resource editor implementations
	/// </summary>
	public class EditorInterface : OSGeo.MapGuide.Maestro.ResourceEditors.EditorInterface
	{
		private FormMain m_editor;
		private TabPage m_page;
		private string m_resourceID;
        private string m_tempresourceID;
		private bool m_existing;
		private static string m_lastPath;
		public event EventHandler Closing;

		public EditorInterface(FormMain editor, TabPage page, string resid, bool exisiting)
		{
			m_editor = editor;
			m_page = page;
			m_existing = exisiting;
			m_resourceID = resid;
            if (!exisiting)
                m_tempresourceID = m_resourceID;
            else
            {
                string tmp = "Session:" + m_editor.CurrentConnection.SessionID + "//" + Guid.NewGuid().ToString() + "." + new MaestroAPI.ResourceIdentifier(m_resourceID).Extension;
                m_editor.CurrentConnection.CopyResource(m_resourceID, tmp, true);
                m_tempresourceID = tmp;
            }

            if (m_page != null)
                m_page.ToolTipText = resid == null ? "" : resid;
		}

		#region EditorInterface Members

		public System.Windows.Forms.ImageList ImageList
		{
			get
			{
				return m_editor.ResourceEditorMap.SmallImageList ;
			}
		}

		public OSGeo.MapGuide.MaestroAPI.ServerConnectionI CurrentConnection
		{
			get
			{
				return m_editor.CurrentConnection;
			}
		}

		public int ImageIndexForItem(string iconType)
		{
			return m_editor.ResourceEditorMap.GetImageIndexFromResourceType(iconType);
		}

		public void EditItem(string resourceID)
		{
			m_editor.OpenResource(resourceID);
		}

		public void HasChanged()
		{
			if (m_page != null)
				if (!m_page.Text.EndsWith("*"))
					m_page.Text += " *";
		}

		internal TabPage Page
		{
			get { return m_page; }
		}

        public string BrowseResource(string itemType)
        {
            string[] tmp = BrowseResource(new string[] { itemType }, false);
            if (tmp != null && tmp.Length == 1)
                return tmp[0];
            else
                return null;
        }
        
        public string[] BrowseResource(string itemType, bool multiSelect)
		{
			return BrowseResource(new string[] { itemType }, multiSelect );
		}

        public string BrowseResource(string[] itemTypes)
        {
            string[] tmp = BrowseResource(itemTypes, false);
            if (tmp != null && tmp.Length == 1)
                return tmp[0];
            else
                return null;
        }

		public string[] BrowseResource(string[] itemTypes, bool multiSelect)
		{
            ResourceBrowser.BrowseResource dlg = new ResourceBrowser.BrowseResource(m_editor.RepositoryCache, m_editor, true, multiSelect, itemTypes);
			dlg.SelectedResources = new string[] { m_editor.LastSelectedNode };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.SelectedResources != null && dlg.SelectedResources.Length > 0)
                    m_editor.LastSelectedNode = dlg.SelectedResources[0];
                return dlg.SelectedResources;
            }
            else
                return null;
		}

		public void Delete()
		{
			using (new WaitCursor(m_editor))
			{
				try
				{
					if (m_existing)
						m_editor.CurrentConnection.DeleteResource(m_resourceID);
					m_editor.tabItems.TabPages.Remove(m_page);
				}
				catch (Exception ex)
				{
                    SetLastException(ex);
					MessageBox.Show(m_editor, string.Format(Strings.EditorInterface.DeleteResourceError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		public bool Close(bool askUser)
		{
			if (askUser && m_page.Text.EndsWith(" *"))
				switch (MessageBox.Show(m_editor, Strings.EditorInterface.SaveBeforeCloseConfirmation, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
				{
					case DialogResult.Yes:
						this.Save();
						if (m_page.Text.EndsWith(" *"))
							return false;
						break;
					case DialogResult.No:
						break;
					case DialogResult.Cancel:
						return false;
				}

			if (Closing != null)
				Closing(this, null);

            int index = m_editor.tabItems.TabPages.IndexOf(m_page);
			m_editor.tabItems.TabPages.Remove(m_page);
			
			foreach(KeyValuePair<string, EditorInterface> de in m_editor.OpenResourceEditors)
				if (de.Value == this)
				{
					m_editor.OpenResourceEditors.Remove(de.Key);
					break;
				}

            if (m_editor.tabItems.TabPages.Count > 0 && m_editor.tabItems.SelectedIndex == 0)
                m_editor.tabItems.SelectedIndex = Math.Max(0, Math.Min(index - 1, m_editor.tabItems.TabPages.Count));

			return true;
		}

		public ResourceEditors.ResourceEditorMap ResourceEditorMap 
		{
			get { return m_editor.ResourceEditorMap; } 
		}

		public string BrowseUnmanagedData(string startPath, System.Collections.Specialized.NameValueCollection filetypes)
		{
			BrowseUnmanagedData umd = new BrowseUnmanagedData(this.CurrentConnection);
			umd.SetFileTypes(filetypes);
			if (m_lastPath != null)
				umd.SelectedText = m_lastPath;
            if (startPath != null && startPath.StartsWith("%MG_DATA_PATH_ALIAS["))
                umd.SelectedText = startPath;
			if (umd.ShowDialog(m_editor) == DialogResult.OK)
			{
				m_lastPath = umd.SelectedText;
				return umd.SelectedText;
			}
			else
				return null;
		}


		private bool Save(string resid)
		{
			using (new WaitCursor(m_editor))
			{
                if (((IResourceEditorControl)m_page.Controls[0]).SupportsValidate)
                    if (((IResourceEditorControl)m_page.Controls[0]).ValidateResource(false))
                    {
                        System.Reflection.PropertyInfo pi = ((IResourceEditorControl)m_page.Controls[0]).Resource.GetType().GetProperty("CurrentConnection");
                        if (pi != null && pi.CanWrite)
                            pi.SetValue(((IResourceEditorControl)m_page.Controls[0]).Resource, m_editor.CurrentConnection, null);

                        ResourceValidators.ValidationIssue[] issues = ResourceValidators.Validation.Validate(((IResourceEditorControl)m_page.Controls[0]).Resource, false);
                        List<string> errors = new List<string>();
                        List<string> warnings = new List<string>();

                        foreach (ResourceValidators.ValidationIssue issue in issues)
                        {
                            if(issue.Status == OSGeo.MapGuide.Maestro.ResourceValidators.ValidationStatus.Error)
                                errors.Add(issue.Message);
                            else if (issue.Status == OSGeo.MapGuide.Maestro.ResourceValidators.ValidationStatus.Warning)
                                warnings.Add(issue.Message);
                        }

                        if (errors.Count > 0 || warnings.Count > 0)
                        {
                            string msg;

                            string fullmsg;
                            if (errors.Count > 0)
                            {
                                for (int i = 0; i < errors.Count; i++)
                                    errors[i] = errors[i].Trim();
                                fullmsg = string.Join("\n", errors.ToArray());
                            }
                            else
                            {
                                for (int i = 0; i < warnings.Count; i++)
                                    warnings[i] = warnings[i].Trim();
                                fullmsg = string.Join("\n", warnings.ToArray());
                            }

                            if (fullmsg.Length > 512)
                                fullmsg = string.Format(Strings.EditorInterface.ValidationMessageTooLong, fullmsg.Substring(0, 512));

                            if (errors.Count > 0)
                                msg = string.Format(Strings.EditorInterface.SaveWithErrorsConfirmation, fullmsg);
                            else
                                msg = string.Format(Strings.EditorInterface.SaveWithWarningsConfirmation, fullmsg);

                            if (MessageBox.Show(m_editor, msg, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
                                return false;
                        }
                    }

				string resourceType = m_editor.ResourceEditorMap.GetResourceTypeNameFromResourceID(m_resourceID);
				if (!m_existing || resid == null)
				{
                    ResourceBrowser.BrowseResource dlg = new ResourceBrowser.BrowseResource(m_editor.RepositoryCache, m_editor, false, false, new string[] { resourceType });
					dlg.SelectedResources = new string[] { m_editor.LastSelectedNode };
					if (dlg.ShowDialog() != DialogResult.OK || dlg.SelectedResources == null || dlg.SelectedResources.Length != 1)
						return false;

					resid = dlg.SelectedResources[0];
				}

				if (m_editor.OpenResourceEditors.ContainsKey(resid) && m_editor.OpenResourceEditors[resid] != this)
					if (!((EditorInterface)m_editor.OpenResourceEditors[resid]).Close(true))
						return false;

                try
                {
                    if (resid != null)
                    {
                        //If the control handles the save, we only update the local items
                        if (!((IResourceEditorControl)m_page.Controls[0]).Save(m_tempresourceID))
                            m_editor.CurrentConnection.SaveResourceAs(((IResourceEditorControl)m_page.Controls[0]).Resource, m_tempresourceID);

                        m_editor.CurrentConnection.CopyResource(m_tempresourceID, resid, true);

                        m_resourceID = resid;
                        m_page.Text = OSGeo.MapGuide.MaestroAPI.ResourceIdentifier.GetName(resid);
                        m_page.ToolTipText = resid;
                    }

                    if (m_page.Text.EndsWith(" *"))
                        m_page.Text = m_page.Text.Substring(0, m_page.Text.Length - 2);

                    if (!m_existing || m_resourceID != resid)
                    {
                        m_editor.RebuildDocumentTree();
                        m_existing = true;
                        string n = resid.Substring(resid.LastIndexOf("/") + 1);
                        m_page.Text = n.Substring(0, n.LastIndexOf("."));


                        foreach (KeyValuePair<string, EditorInterface> de in m_editor.OpenResourceEditors)
                            if (de.Value == this)
                            {
                                m_editor.OpenResourceEditors.Remove(de.Key);
                                m_editor.OpenResourceEditors.Add(m_resourceID, this);
                                break;
                            }
                    }

                    ((IResourceEditorControl)m_page.Controls[0]).UpdateDisplay();
                    return true;
                }
                catch (CancelException)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    SetLastException(ex);
                    MessageBox.Show(m_editor, string.Format(Strings.EditorInterface.SaveResourceError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
			}
		}

		public bool Existing
		{
			get { return m_existing; }
			set { m_existing = value; }
		}

		public bool SaveAs()
		{
			return Save(null);
		}

		public bool Save()
		{
			return Save(m_resourceID);
		}

        public string TempResourceId
        {
            get { return m_tempresourceID; }
        }

        public DialogResult LengthyOperation(object caller, System.Reflection.MethodInfo mi)
        {
            return LengthyOperation(caller, mi, true);
        }

		public DialogResult LengthyOperation(object caller, System.Reflection.MethodInfo mi, bool waitForAccept)
		{
			OSGeo.MapGuide.Maestro.LengthyOperation lo = new OSGeo.MapGuide.Maestro.LengthyOperation();
			lo.CallbackObject = caller;
			lo.CallbackEnabledMethod = mi;
			lo.InitializeDialog(m_editor.CurrentConnection, "", "", OSGeo.MapGuide.Maestro.LengthyOperation.OperationType.Other, waitForAccept);
			return lo.ShowDialog(m_editor);
		}

        public string EditExpression(string current, OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema schema, string providername, string featuresSourceId)
        {
            FormExpression dlg = new FormExpression();
            dlg.SetupForm(m_editor.CurrentConnection, schema, providername, featuresSourceId);
            dlg.Expression = current;
            if (dlg.ShowDialog() == DialogResult.OK)
                return dlg.Expression;
            else
                return null;
        }

        public void OpenUrl(string url)
        {
            Program.OpenUrl(url);
        }

        public bool UseFusionPreview { get { return Program.ApplicationSettings.UseFusionPreview; } }

        public void SetLastException(Exception ex) { m_editor.LastException = ex; }

        public bool IsModified { get { return m_page.Text.EndsWith(" *"); } }

        public string ResourceId { get { return m_resourceID; } }

		#endregion
	}
}
