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
using System.Collections;

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// This class represents the different functions used to map resources to editors and vice versa
	/// </summary>
	public class ResourceEditorMap
		: ResourceEditors.ResourceEditorMap
	{

		/// <summary>
		/// This class is used to read the xml file through serialization
		/// </summary>
		[System.Xml.Serialization.XmlRoot("ResourceEditors")]
		public class ResourceEditorXml
		{
			private ResourceEditorEntry[] m_editors;

			/// <summary>
			/// Class for an editor entry, see the "EditorMap.xml" file for details
			/// </summary>
			public class ResourceEditorEntry
			{
				public string DisplayName;
				public string ResourceExtension;
				public string ImagePath;
				public string ImageAssembly;
				public string ImageResourceName;
				public string ResourceEditorAssembly;
				public string ResourceEditorClass;
				public string ResourceInstanceAssembly;
				public string ResourceInstanceClass;

				public ResourceEditorEntry()
				{
				}
			}

			public ResourceEditorXml()
			{
			}

			[System.Xml.Serialization.XmlElement("ResourceEditorEntry")]
			public ResourceEditorEntry[] Editors
			{
				get { return m_editors; }
				set { m_editors = value; }
			}
		}


		/// <summary>
		/// This class represents a single resource editor
		/// </summary>
		public class ResourceEditorEntry
		{
			private string m_displayName;
			private string m_resourceExtension;
			private System.Type m_editor;
			private System.Type m_instance;
			private int m_imageIndex;

			public ResourceEditorEntry(string displayName, string resourceExtension, System.Type editor, System.Type instance, int imageIndex)
			{
				m_displayName = displayName;
				m_resourceExtension = resourceExtension;
				m_editor = editor;
				m_imageIndex = imageIndex;
				m_instance = instance;
			}

			public string DisplayName 
			{
				get { return m_displayName; }
				set { m_displayName = value; }
			}

			public string ResourceExtension
			{
				get { return m_resourceExtension; }
				set { m_resourceExtension = value; }
			}

			public Type EditorType
			{
				get { return m_editor; }
				set { m_editor = value; }
			}

			public Type Instance
			{
				get { return m_instance; }
				set { m_instance = value; }
			}

			public int ImageIndex
			{
				get { return m_imageIndex; }
				set { m_imageIndex = value; }
			}
		}


		public ResourceEditorMap(string configfile)
		{
			try
			{
				ResourceEditorXml edt = null;
				System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(ResourceEditorXml));
				using(System.IO.FileStream fs = System.IO.File.Open(configfile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
					edt = (ResourceEditorXml)sr.Deserialize(fs);

				m_editors = new Hashtable();
				m_smallImages = new System.Windows.Forms.ImageList();
				m_largeImages = new System.Windows.Forms.ImageList();
				m_smallImages.ColorDepth = m_largeImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
				m_smallImages.ImageSize = new System.Drawing.Size(16, 16);
				m_largeImages.ImageSize = new System.Drawing.Size(32, 32);

				//Add the folder type
				m_editors.Add("", 
					new ResourceEditorEntry(
					"Folder",
					"",
					null,
					null,
					1
					)
					);

				System.Drawing.Image blankImg = System.Drawing.Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OSGeo.MapGuide.Maestro.Icons.blank_icon.gif"));
				System.Drawing.Image folderImg = System.Drawing.Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OSGeo.MapGuide.Maestro.Icons.FolderOpen.ico"));
				System.Drawing.Image serverImg = System.Drawing.Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OSGeo.MapGuide.Maestro.Icons.Server.ico"));
				System.Drawing.Image unknownImg = System.Drawing.Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OSGeo.MapGuide.Maestro.Icons.Unknown.ico"));

				m_smallImages.Images.Add(blankImg);
				m_largeImages.Images.Add(blankImg);
				m_smallImages.Images.Add(folderImg);
				m_largeImages.Images.Add(folderImg);
				m_smallImages.Images.Add(serverImg);
				m_largeImages.Images.Add(serverImg);
				m_smallImages.Images.Add(unknownImg);
				m_largeImages.Images.Add(unknownImg);

				string path;

				foreach(ResourceEditorXml.ResourceEditorEntry e in edt.Editors)
				{
					System.Drawing.Image img = null;
					int imageindex = -1;

					//TODO: Fail gracefully with missing icons?
					if (e.ImagePath != null && e.ImagePath.Trim().Length > 0)
						img = System.Drawing.Image.FromFile(e.ImagePath);
					else if (e.ImageAssembly != null && e.ImageAssembly.Trim().Length > 0 && e.ImageResourceName != null && e.ImageResourceName.Trim().Length > 0)
					{
						path = e.ImageAssembly;
						if (!System.IO.Path.IsPathRooted(path))
							path = System.IO.Path.GetFullPath(path);
						System.Reflection.Assembly asm;
						try
						{
							asm = System.Reflection.Assembly.LoadFile(path);
						}
						catch(Exception ex)
						{
							throw new Exception("Failed while loading assembly: " + path + ", error: " + ex.Message, ex);
						}
						img = System.Drawing.Image.FromStream(asm.GetManifestResourceStream(e.ImageResourceName));
					}

					if (img != null)
					{
						m_smallImages.Images.Add(img);
						m_largeImages.Images.Add(img);
						imageindex = m_smallImages.Images.Count - 1;
					}

					path = e.ResourceEditorAssembly;
					if (!System.IO.Path.IsPathRooted(path))
						path = System.IO.Path.GetFullPath(path);

					System.Reflection.Assembly editorAsm;
					try
					{
						editorAsm = System.Reflection.Assembly.LoadFile(path);
					} 
					catch (Exception ex)
					{
						throw new Exception("Failed while loading assembly: " + path + ", error: " + ex.Message, ex);
					}

					System.Type editorType = editorAsm.GetType(e.ResourceEditorClass, true, false);

					path = e.ResourceInstanceAssembly;
					if (!System.IO.Path.IsPathRooted(path))
						path = System.IO.Path.GetFullPath(path);

					System.Reflection.Assembly instanceAsm;
					try
					{
						instanceAsm = System.Reflection.Assembly.LoadFile(path);
					}
					catch (Exception ex)
					{
						throw new Exception("Failed while loading assembly: " + path + ", error: " + ex.Message, ex);
					}
					System.Type instanceType = instanceAsm.GetType(e.ResourceInstanceClass, true, false);

					if (editorType == null)
						throw new Exception("Failed to load type " + e.ResourceEditorClass + " from assembly: " + e.ResourceEditorAssembly);
					if (instanceType == null)
						throw new Exception("Failed to load type " + e.ResourceInstanceClass + " from assembly: " + e.ResourceInstanceAssembly);


					if (editorType.GetInterface(typeof(ResourceEditor).FullName) == null)
						throw new Exception("Resource editor for " + e.ResourceExtension + " does not implement the required interface: " + typeof(ResourceEditor).FullName);

					m_editors.Add(e.ResourceExtension, 
						new ResourceEditorEntry(
						e.DisplayName,
						e.ResourceExtension,
						editorType,
						instanceType,
						imageindex
						)
						);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to load the resource editors: " + ex.Message, ex);
			}
		}

		private Hashtable m_editors;
		private System.Windows.Forms.ImageList m_smallImages;
		private System.Windows.Forms.ImageList m_largeImages;

		public System.Windows.Forms.ImageList SmallImageList { get { return m_smallImages; } }
		public System.Windows.Forms.ImageList LargeImageList { get { return m_largeImages; } }

		public string GetResourceDisplayNameFromResourceType(string itemType)
		{
			if (m_editors.ContainsKey(itemType))
				return ((ResourceEditorEntry)m_editors[itemType]).DisplayName;
			else
				return "Unknown";
		}

		public Type GetResourceEditorTypeFromResourceType(string itemType)
		{
			if (m_editors.ContainsKey(itemType))
				return ((ResourceEditorEntry)m_editors[itemType]).EditorType;
			else
				return null;
		}

		public string[] AvalibleResourceTypes
		{
			get 
			{
				string[] items = new string[m_editors.Count];
				int i = 0;
				foreach(string key in m_editors.Keys)
					items[i++] = key;
				return items;
			}
		}

		public int GetImageIndexFromResourceID(string resourceID)
		{
			string suffix = GetResourceTypeNameFromResourceID(resourceID);
			if (m_editors.ContainsKey(suffix))
				return ((ResourceEditorEntry)m_editors[suffix]).ImageIndex;
			else
				return this.UnknownIcon;
		}

		public string GetResourceNameFromResourceID(string resourceID)
		{
			string[] parts = SplitResourceID(resourceID);
			string name = parts[parts.Length-1];
			int x = name.LastIndexOf(".");
			if (x > 0)
				name = name.Substring(0, x);
			return name;
		}

		public string GetResourceTypeNameFromResourceID(string resourceID)
		{
			string suffix = resourceID;
			if (suffix.IndexOf("/") > 0)
				suffix = suffix.Substring(suffix.LastIndexOf("/") + 1);
            
			if (suffix.IndexOf(".") > 0)
				suffix = suffix.Substring(suffix.LastIndexOf(".") + 1);
			else
				suffix = "";
			
			return suffix;
		}

		public string[] SplitResourceID(string resourceID)
		{
			int x = resourceID.IndexOf("://");
			if (x < 0)
				throw new Exception("Invalid Resource Identifier: " + resourceID);
			
			string parts = resourceID.Substring(x + "://".Length);
			if (parts.EndsWith("/"))
				parts = parts.Substring(0, parts.Length - 1);
			return parts.Split('/');
		}

		public System.Type GetResourceEditorTypeFromResourceID(string resourceID)
		{
			return GetResourceEditorTypeFromResourceType(GetResourceTypeNameFromResourceID(resourceID));
		}

		public int GetImageIndexFromResourceType(string itemType)
		{
			if (m_editors.ContainsKey(itemType))
				return ((ResourceEditorEntry)m_editors[itemType]).ImageIndex;
			else
				return this.UnknownIcon;
		}

		public Type GetResourceInstanceTypeFromResourceType(string resourceType)
		{
			if (m_editors.ContainsKey(resourceType))
				return ((ResourceEditorEntry)m_editors[resourceType]).Instance;
			else
				return null;

		}

		public Type GetResourceInstanceTypeFromResourceID(string resourceID)
		{
			return GetResourceInstanceTypeFromResourceType(GetResourceTypeNameFromResourceID(resourceID));
		}

		/// <summary>
		/// Gets the imageindex of the folder icon.
		/// </summary>
		public int FolderIcon { get { return 1; } }

		/// <summary>
		/// Gets the imageindex of the blank icon.
		/// </summary>
		public int BlankIcon { get { return 0; } }

		/// <summary>
		/// Gets the imageindex of the server icon.
		/// </summary>
		public int ServerIcon { get { return 2; } }

		/// <summary>
		/// Gets the imageindex of the unknown resource icon.
		/// </summary>
		public int UnknownIcon { get { return 3; } }

	}
}
