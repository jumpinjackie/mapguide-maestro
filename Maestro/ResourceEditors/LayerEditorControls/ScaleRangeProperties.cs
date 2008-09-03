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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls
{
	/// <summary>
	/// Summary description for ScaleRangeProperties.
	/// </summary>
	public class ScaleRangeProperties : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.CheckBox UsePoint;
		private System.Windows.Forms.CheckBox UseLine;
		private System.Windows.Forms.CheckBox UseArea;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;

		private OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType m_range;
		private bool m_isUpdating = false;
		private System.Windows.Forms.ComboBox minimumScale;
		private System.Windows.Forms.ComboBox maximumScale;

		public event EventHandler Changed;
		public event EventHandler ChangedTree;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ScaleRangeProperties()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true; 
				if (m_range == null)
				{
					this.Enabled = false;
					return;
				}
				
				this.Enabled = true;
				if (m_range.MaxScaleSpecified)
					maximumScale.Text = m_range.MaxScale.ToString();
				else
					maximumScale.SelectedIndex = 0;

				if (m_range.MinScaleSpecified)
					minimumScale.Text = m_range.MinScale.ToString();
				else
					minimumScale.SelectedIndex = 0;

				OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType area = null;
				OSGeo.MapGuide.MaestroAPI.LineTypeStyleType line = null;
				OSGeo.MapGuide.MaestroAPI.PointTypeStyleType point = null;

				if (m_range.Items != null)
					foreach(object o in m_range.Items)
						if (o as OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType != null)
							area = o as OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType;
						else if (o as OSGeo.MapGuide.MaestroAPI.LineTypeStyleType != null)
							line = o as OSGeo.MapGuide.MaestroAPI.LineTypeStyleType;
						else if (o as OSGeo.MapGuide.MaestroAPI.PointTypeStyleType != null)
							point = o as OSGeo.MapGuide.MaestroAPI.PointTypeStyleType;

				UsePoint.Checked = point != null;
				UseLine.Checked = line != null;
				UseArea.Checked = area != null;
			}
			finally
			{
				m_isUpdating = false;
			}
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

		public OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType Item
		{
			get { return m_range; }
			set 
			{
				m_range = value; 
				UpdateDisplay();
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.UsePoint = new System.Windows.Forms.CheckBox();
			this.UseLine = new System.Windows.Forms.CheckBox();
			this.UseArea = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.minimumScale = new System.Windows.Forms.ComboBox();
			this.maximumScale = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// UsePoint
			// 
			this.UsePoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.UsePoint.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.UsePoint.Location = new System.Drawing.Point(0, 48);
			this.UsePoint.Name = "UsePoint";
			this.UsePoint.Size = new System.Drawing.Size(248, 16);
			this.UsePoint.TabIndex = 0;
			this.UsePoint.Text = "Display points";
			this.UsePoint.CheckedChanged += new System.EventHandler(this.UsePoint_CheckedChanged);
			// 
			// UseLine
			// 
			this.UseLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.UseLine.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.UseLine.Location = new System.Drawing.Point(0, 72);
			this.UseLine.Name = "UseLine";
			this.UseLine.Size = new System.Drawing.Size(248, 16);
			this.UseLine.TabIndex = 1;
			this.UseLine.Text = "Display lines";
			this.UseLine.CheckedChanged += new System.EventHandler(this.UseLine_CheckedChanged);
			// 
			// UseArea
			// 
			this.UseArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.UseArea.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.UseArea.Location = new System.Drawing.Point(0, 96);
			this.UseArea.Name = "UseArea";
			this.UseArea.Size = new System.Drawing.Size(248, 16);
			this.UseArea.TabIndex = 2;
			this.UseArea.Text = "Display areas";
			this.UseArea.CheckedChanged += new System.EventHandler(this.UseArea_CheckedChanged);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Minimum scale";
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(0, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Maximum scale";
			// 
			// minimumScale
			// 
			this.minimumScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.minimumScale.Items.AddRange(new object[] {
															  "Infinite"});
			this.minimumScale.Location = new System.Drawing.Point(104, 0);
			this.minimumScale.Name = "minimumScale";
			this.minimumScale.Size = new System.Drawing.Size(144, 21);
			this.minimumScale.TabIndex = 5;
			this.minimumScale.Text = "comboBox1";
			this.minimumScale.TextChanged += new System.EventHandler(this.minimumScale_TextChanged);
			// 
			// maximumScale
			// 
			this.maximumScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.maximumScale.Items.AddRange(new object[] {
															  "Infinite"});
			this.maximumScale.Location = new System.Drawing.Point(104, 24);
			this.maximumScale.Name = "maximumScale";
			this.maximumScale.Size = new System.Drawing.Size(144, 21);
			this.maximumScale.TabIndex = 6;
			this.maximumScale.Text = "comboBox2";
			this.maximumScale.TextChanged += new System.EventHandler(this.maximumScale_TextChanged);
			// 
			// ScaleRangeProperties
			// 
			this.Controls.Add(this.maximumScale);
			this.Controls.Add(this.minimumScale);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.UseArea);
			this.Controls.Add(this.UseLine);
			this.Controls.Add(this.UsePoint);
			this.Name = "ScaleRangeProperties";
			this.Size = new System.Drawing.Size(248, 312);
			this.ResumeLayout(false);

		}
		#endregion

		private void minimumScale_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (minimumScale.Text == "Infinite")
				m_range.MinScaleSpecified = false;
			else
			{
				m_range.MinScaleSpecified = true;
				double x;
				if (double.TryParse(minimumScale.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentUICulture, out x))
					m_range.MinScale = x;
			}

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void maximumScale_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;
		
			if (maximumScale.Text == "Infinite")
				m_range.MaxScaleSpecified = false;
			else
			{
				m_range.MaxScaleSpecified = true;
				double x;
				if (double.TryParse(maximumScale.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentUICulture, out x))
					m_range.MaxScale = x;
			}

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void UsePoint_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;
		
			if (m_range.Items == null)
				m_range.Items = new ArrayList();

			if (UsePoint.Checked)
			{
				if (UsePoint.Tag == null)
					UsePoint.Tag = new OSGeo.MapGuide.MaestroAPI.PointTypeStyleType();
                m_range.Items.Add(UsePoint.Tag);
			}
			else
			{
				if (m_range.Items != null)
					foreach(object o in m_range.Items)
						if (o as OSGeo.MapGuide.MaestroAPI.PointTypeStyleType != null)
						{
							m_range.Items.Remove(o);
							UsePoint.Tag = o;
							break;
						}
			}
			if (Changed != null)
				Changed(this, new EventArgs());
			if (ChangedTree != null)
				ChangedTree(this, new EventArgs());
		}

		private void UseLine_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_range.Items == null)
				m_range.Items = new ArrayList();
		
			if (UseLine.Checked)
			{
				if (UseLine.Tag == null)
					UseLine.Tag = new OSGeo.MapGuide.MaestroAPI.LineTypeStyleType();

				m_range.Items.Add(UseLine.Tag);
			}
			else
			{
				if (m_range.Items != null)
					foreach(object o in m_range.Items)
						if (o as OSGeo.MapGuide.MaestroAPI.LineTypeStyleType != null)
						{
							m_range.Items.Remove(o);
							UseLine.Tag = o;
							break;
						}
			}
			if (Changed != null)
				Changed(this, new EventArgs());
			if (ChangedTree != null)
				ChangedTree(this, new EventArgs());
		}

		private void UseArea_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_range.Items == null)
				m_range.Items = new ArrayList();
		
			if (UseArea.Checked)
			{
				if (UseArea.Tag == null)
					UseArea.Tag = new OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType();
				m_range.Items.Add(UseArea.Tag);
			}
			else
			{
				if (m_range.Items != null)
					foreach(object o in m_range.Items)
						if (o as OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType != null)
						{
							m_range.Items.Remove(o);
							UseArea.Tag = o;
							break;
						}
			}
			if (Changed != null)
				Changed(this, new EventArgs());
			if (ChangedTree != null)
				ChangedTree(this, new EventArgs());
		}
	}
}
