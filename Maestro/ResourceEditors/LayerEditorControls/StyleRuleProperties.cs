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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls
{
	/// <summary>
	/// Summary description for StyleRuleProperties.
	/// </summary>
	public class StyleRuleProperties : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox Filter;
		private System.Windows.Forms.CheckBox ShowFeatures;
		private System.Windows.Forms.CheckBox ShowLabels;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private bool m_isUpdating = false;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox LegendLabel;
		private System.Windows.Forms.Label label2;
		private object m_item;

		public event EventHandler Changed;
		public event EventHandler ChangedTree;

		public StyleRuleProperties()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;

				if (m_item == null)
				{
					this.Enabled = false;
					return;
				}
				this.Enabled = true;

				if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointRuleType))
				{
					OSGeo.MapGuide.MaestroAPI.PointRuleType prt = (OSGeo.MapGuide.MaestroAPI.PointRuleType)m_item;
					Filter.Text = prt.Filter;
					LegendLabel.Text = prt.LegendLabel;
					ShowFeatures.Checked = prt.Item != null;
					ShowLabels.Checked = prt.Label != null;
				}
				else if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineRuleType))
				{
					OSGeo.MapGuide.MaestroAPI.LineRuleType lrt = (OSGeo.MapGuide.MaestroAPI.LineRuleType)m_item;
					Filter.Text = lrt.Filter;
					LegendLabel.Text = lrt.LegendLabel;
					ShowFeatures.Checked = lrt.Items != null;
					ShowLabels.Checked = lrt.Label != null;
				}
				else if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaRuleType))
				{
					OSGeo.MapGuide.MaestroAPI.AreaRuleType art = (OSGeo.MapGuide.MaestroAPI.AreaRuleType)m_item;
					Filter.Text = art.Filter;
					LegendLabel.Text = art.LegendLabel;
					ShowFeatures.Checked = art.Item != null;
					ShowLabels.Checked = art.Label != null;
				}
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.Filter = new System.Windows.Forms.TextBox();
			this.ShowFeatures = new System.Windows.Forms.CheckBox();
			this.ShowLabels = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.LegendLabel = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Rule";
			// 
			// Filter
			// 
			this.Filter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Filter.Location = new System.Drawing.Point(96, 0);
			this.Filter.Name = "Filter";
			this.Filter.Size = new System.Drawing.Size(224, 20);
			this.Filter.TabIndex = 1;
			this.Filter.Text = "";
			this.Filter.TextChanged += new System.EventHandler(this.Filter_TextChanged);
			// 
			// ShowFeatures
			// 
			this.ShowFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ShowFeatures.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ShowFeatures.Location = new System.Drawing.Point(0, 48);
			this.ShowFeatures.Name = "ShowFeatures";
			this.ShowFeatures.Size = new System.Drawing.Size(352, 16);
			this.ShowFeatures.TabIndex = 2;
			this.ShowFeatures.Text = "Show features";
			this.ShowFeatures.CheckedChanged += new System.EventHandler(this.ShowFeatures_CheckedChanged);
			// 
			// ShowLabels
			// 
			this.ShowLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ShowLabels.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ShowLabels.Location = new System.Drawing.Point(0, 72);
			this.ShowLabels.Name = "ShowLabels";
			this.ShowLabels.Size = new System.Drawing.Size(352, 16);
			this.ShowLabels.TabIndex = 3;
			this.ShowLabels.Text = "Show feature labels";
			this.ShowLabels.CheckedChanged += new System.EventHandler(this.ShowLabels_CheckedChanged);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(328, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(24, 20);
			this.button1.TabIndex = 4;
			this.button1.Text = "...";
			// 
			// LegendLabel
			// 
			this.LegendLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LegendLabel.Location = new System.Drawing.Point(96, 24);
			this.LegendLabel.Name = "LegendLabel";
			this.LegendLabel.Size = new System.Drawing.Size(256, 20);
			this.LegendLabel.TabIndex = 6;
			this.LegendLabel.Text = "";
			this.LegendLabel.TextChanged += new System.EventHandler(this.LegendLabel_TextChanged);
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(0, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Legend label";
			// 
			// StyleRuleProperties
			// 
			this.Controls.Add(this.LegendLabel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.ShowLabels);
			this.Controls.Add(this.ShowFeatures);
			this.Controls.Add(this.Filter);
			this.Controls.Add(this.label1);
			this.Name = "StyleRuleProperties";
			this.Size = new System.Drawing.Size(352, 128);
			this.ResumeLayout(false);

		}
		#endregion

		private void Filter_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.PointRuleType prt = (OSGeo.MapGuide.MaestroAPI.PointRuleType)m_item;
				prt.Filter = Filter.Text;
			}
			else if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.LineRuleType lrt = (OSGeo.MapGuide.MaestroAPI.LineRuleType)m_item;
				lrt.Filter = Filter.Text;
			}
			else if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.AreaRuleType art = (OSGeo.MapGuide.MaestroAPI.AreaRuleType)m_item;
				art.Filter = Filter.Text;
			}

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void ShowFeatures_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.PointRuleType prt = (OSGeo.MapGuide.MaestroAPI.PointRuleType)m_item;
				if (ShowFeatures.Checked)
				{
					if (ShowFeatures.Tag != null)
						prt.Item = (OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType)ShowFeatures.Tag;
					else
						prt.Item = new OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType();
				}
				else
				{
					ShowFeatures.Tag = prt.Item;
					prt.Item = null;
				}

			}
			else if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.LineRuleType lrt = (OSGeo.MapGuide.MaestroAPI.LineRuleType)m_item;
				if (ShowFeatures.Checked)
				{
					if (ShowFeatures.Tag != null)
						lrt.Items = (OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection)ShowFeatures.Tag;
					else
						lrt.Items = new OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection();
				}
				else
				{
					ShowFeatures.Tag = lrt.Items;
					lrt.Items = null;
				}
			}
			else if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.AreaRuleType art = (OSGeo.MapGuide.MaestroAPI.AreaRuleType)m_item;
				if (ShowFeatures.Checked)
				{
					if (ShowFeatures.Tag != null)
						art.Item = (OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType)ShowFeatures.Tag;
					else
						art.Item = new OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType();
				}
				else
				{
					ShowFeatures.Tag = art.Item;
					art.Item = null;
				}
			}
		
			if (Changed != null)
				Changed(this, new EventArgs());
			if (ChangedTree != null)
				ChangedTree(this, new EventArgs());
		}

		private void ShowLabels_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.PointRuleType prt = (OSGeo.MapGuide.MaestroAPI.PointRuleType)m_item;
				if (ShowLabels.Checked)
				{
					if (ShowLabels.Tag != null)
						prt.Label = (OSGeo.MapGuide.MaestroAPI.TextSymbolType)ShowLabels.Tag;
					else
						prt.Label = new OSGeo.MapGuide.MaestroAPI.TextSymbolType();
				}
				else
				{
					ShowLabels.Tag = prt.Label;
					prt.Label = null;
				}

			}
			else if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.LineRuleType lrt = (OSGeo.MapGuide.MaestroAPI.LineRuleType)m_item;
				if (ShowLabels.Checked)
				{
					if (ShowFeatures.Tag != null)
						lrt.Label = (OSGeo.MapGuide.MaestroAPI.TextSymbolType)ShowLabels.Tag;
					else
						lrt.Label = new OSGeo.MapGuide.MaestroAPI.TextSymbolType();
				}
				else
				{
					ShowLabels.Tag = lrt.Label;
					lrt.Label = null;
				}
			}
			else if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.AreaRuleType art = (OSGeo.MapGuide.MaestroAPI.AreaRuleType)m_item;
				if (ShowLabels.Checked)
				{
					if (ShowLabels.Tag != null)
						art.Label = (OSGeo.MapGuide.MaestroAPI.TextSymbolType)ShowLabels.Tag;
					else
						art.Label = new OSGeo.MapGuide.MaestroAPI.TextSymbolType();
				}
				else
				{
					ShowLabels.Tag = art.Label;
					art.Label = null;
				}
			}
		
		
			if (Changed != null)
				Changed(this, new EventArgs());
			if (ChangedTree != null)
				ChangedTree(this, new EventArgs());
		}

		private void LegendLabel_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.PointRuleType prt = (OSGeo.MapGuide.MaestroAPI.PointRuleType)m_item;
				prt.LegendLabel = LegendLabel.Text;
			}
			else if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.LineRuleType lrt = (OSGeo.MapGuide.MaestroAPI.LineRuleType)m_item;
				lrt.LegendLabel = LegendLabel.Text;
			}
			else if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.AreaRuleType art = (OSGeo.MapGuide.MaestroAPI.AreaRuleType)m_item;
				art.LegendLabel = LegendLabel.Text;
			}

			if (Changed != null)
				Changed(this, new EventArgs());
		
		}

		public object Item
		{
			get { return m_item; }
			set 
			{ 
				m_item = value; 
				UpdateDisplay();
			}
		}
	}
}
