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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.RasterViewer
{
	/// <summary>
	/// Summary description for PreviewMap.
	/// </summary>
	public class PreviewMap : System.Windows.Forms.Form
	{
		private RasterViewer rasterViewer1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PreviewMap(ServerConnectionI con, string map)
			: this()
		{
			rasterViewer1.SetCurrentMap(con, map);
		}


		public PreviewMap()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.rasterViewer1 = new RasterViewer();
			this.SuspendLayout();
			// 
			// rasterViewer1
			// 
			this.rasterViewer1.CurrentScale = 0;
			this.rasterViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rasterViewer1.Location = new System.Drawing.Point(0, 0);
			this.rasterViewer1.Name = "rasterViewer1";
			this.rasterViewer1.Size = new System.Drawing.Size(292, 273);
			this.rasterViewer1.TabIndex = 0;
			// 
			// PreviewMap
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.rasterViewer1);
			this.Name = "PreviewMap";
			this.Text = "PreviewMap";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
