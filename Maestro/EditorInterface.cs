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
	/// Summary description for EditorInterface.
	/// </summary>
	public class EditorInterface : OSGeo.MapGuide.Maestro.ResourceEditors.EditorInterface
	{
		private FormMain m_editor;
		private TabPage m_page;
		private string m_resourceID;
		private bool m_existing;
		private  Globalizator.Globalizator m_globalizor;
		private static string m_lastPath;
		public event EventHandler Closing;

		public EditorInterface(FormMain editor, TabPage page, string resid, bool exisiting)
		{
			m_editor = editor;
			m_page = page;
			m_existing = exisiting;
			m_resourceID = resid;
			m_globalizor = new Globalizator.Globalizator(this);
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

		public void CreateItem(string itemtype)
		{
			m_editor.CreateResource(null, itemtype);
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

		public string ResourceID
		{
			get { return m_resourceID; }
		}

		public string BrowseResource(string itemType)
		{
			return BrowseResource(new string[] { itemType } );
		}

		public string BrowseResource(string[] itemTypes)
		{
            ResourceBrowser.BrowseResource dlg = new ResourceBrowser.BrowseResource(m_editor.RepositoryCache, m_editor, true, itemTypes);
			dlg.SelectedResource = m_editor.LastSelectedNode;

            if (dlg.ShowDialog() == DialogResult.OK)
                return dlg.SelectedResource;
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
					MessageBox.Show(m_editor, string.Format(m_globalizor.Translate("An error occured while deleting: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		public bool Close(bool askUser)
		{
			if (askUser && m_page.Text.EndsWith(" *"))
				switch (MessageBox.Show(m_editor, m_globalizor.Translate("The resource has modifications. Do you want so save the changes before closing the page?"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
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

			m_editor.tabItems.TabPages.Remove(m_page);
			
			foreach(KeyValuePair<string, EditorInterface> de in m_editor.OpenResourceEditors)
				if (de.Value == this)
				{
					m_editor.OpenResourceEditors.Remove(de.Key);
					break;
				}

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

                            if (errors.Count > 0)
                                msg = string.Format("The resource had the following errors:\n {0}\n\nDo you want to save it anyway?", string.Join("\n", errors.ToArray()));
                            else
                                msg = string.Format("The resource had the following warnings:\n {0}\n\nDo you want to save it anyway?", string.Join("\n", warnings.ToArray()));

                            if (MessageBox.Show(m_editor, msg, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
                                return false;
                        }
                    }

				string resourceType = m_editor.ResourceEditorMap.GetResourceTypeNameFromResourceID(m_resourceID);
				if (!m_existing || resid == null)
				{
                    ResourceBrowser.BrowseResource dlg = new ResourceBrowser.BrowseResource(m_editor.RepositoryCache, m_editor, false, new string[] { resourceType });
					dlg.SelectedResource = m_editor.LastSelectedNode;
					if (dlg.ShowDialog() != DialogResult.OK)
						return false;

					resid = dlg.SelectedResource;
				}

				if (m_editor.OpenResourceEditors.ContainsKey(resid) && m_editor.OpenResourceEditors[resid] != this)
					if (!((EditorInterface)m_editor.OpenResourceEditors[resid]).Close(true))
						return false;

                try
                {
                    if (resid != null)
                    {
                        //If the control handles the save, we only update the local items
                        if (!((IResourceEditorControl)m_page.Controls[0]).Save(resid))
                        {
                            m_editor.CurrentConnection.SaveResourceAs(((IResourceEditorControl)m_page.Controls[0]).Resource, resid);
                            ((IResourceEditorControl)m_page.Controls[0]).ResourceId = resid;
                        }

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
                    MessageBox.Show(m_editor, string.Format(m_globalizor.Translate("An error occured while saving: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

		public DialogResult LengthyOperation(object caller, System.Reflection.MethodInfo mi)
		{
			OSGeo.MapGuide.Maestro.LengthyOperation lo = new OSGeo.MapGuide.Maestro.LengthyOperation();
			lo.CallbackObject = caller;
			lo.CallbackEnabledMethod = mi;
			lo.InitializeDialog(m_editor.CurrentConnection, "", "", OSGeo.MapGuide.Maestro.LengthyOperation.OperationType.Other);
			return lo.ShowDialog(m_editor);
		}

        public string EditExpression(string current, OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema schema, string providername)
        {
            FormExpression dlg = new FormExpression();
            dlg.SetupForm(m_editor.CurrentConnection, schema, providername);
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

		#endregion
	}
}
