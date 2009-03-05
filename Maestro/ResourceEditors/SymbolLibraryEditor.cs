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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for SymbolLibraryEditor.
	/// </summary>
	public class SymbolLibraryEditor : System.Windows.Forms.UserControl, IResourceEditorControl
	{
		private OSGeo.MapGuide.MaestroAPI.SymbolLibraryType m_library;
		private EditorInterface m_editor;

		public SymbolLibraryEditor(EditorInterface editor)
			: this()
		{
			m_editor = editor;
			m_library = new OSGeo.MapGuide.MaestroAPI.SymbolLibraryType();
			UpdateDisplay();
		}

		public SymbolLibraryEditor(EditorInterface editor, string resourceID)
			: this()
		{
			m_editor = editor;
			//m_library = m_editor.CurrentConnection.GetSymbolLibrary(resourceID);
			UpdateDisplay();
		}
	
		public void UpdateDisplay()
		{
		}

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected SymbolLibraryEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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
			components = new System.ComponentModel.Container();
		}
		#endregion

		public object Resource
		{
			get { return m_library; }
			set 
			{
				m_library = (OSGeo.MapGuide.MaestroAPI.SymbolLibraryType)value;
				UpdateDisplay();
			}
		}
		public string ResourceId
		{
			get { return m_library.ResourceId; }
			set { m_library.ResourceId = value; }
		}

		public bool Preview()
		{
			return false;
		}

		public bool Save(string savename)
		{
			return false;
		}
    
        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return true; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }
    }
}
