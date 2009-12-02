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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions
{
	/// <summary>
	/// Summary description for EditExtensions.
	/// </summary>
	public class EditExtensions : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button OKBtn;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private EditorInterface m_editor;
		private OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.FeatureSourceExtension featureSourceExtension;
		private FeatureSource m_feature;
		private System.Windows.Forms.Button CancelBtn;
		private FeatureSourceTypeExtensionCollection m_extension;

		public EditExtensions(EditorInterface editor, FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;
			//Copy the whole thing, so we can use the cancel button
			m_extension = new FeatureSourceTypeExtensionCollection();
			if (m_feature.Extension != null)
				foreach(FeatureSourceTypeExtension fex in m_feature.Extension)
				{
					FeatureSourceTypeExtension nfex = new FeatureSourceTypeExtension();
					nfex.FeatureClass = fex.FeatureClass;
					nfex.Name = fex.Name;

					if (fex.CalculatedProperty != null)
					{
						nfex.CalculatedProperty = new CalculatedPropertyTypeCollection();
						foreach(CalculatedPropertyType cpt in fex.CalculatedProperty)
						{
							CalculatedPropertyType ncpt = new CalculatedPropertyType();
							ncpt.Expression = cpt.Expression;
							ncpt.Name = cpt.Name;
							nfex.CalculatedProperty.Add(ncpt);
						}
					}

					if (fex.AttributeRelate != null)
					{
						nfex.AttributeRelate = new AttributeRelateTypeCollection();
						foreach(AttributeRelateType atr in fex.AttributeRelate)
						{
							AttributeRelateType natr = new AttributeRelateType();
							natr.AttributeClass = atr.AttributeClass;
							natr.AttributeNameDelimiter = atr.AttributeNameDelimiter;
							natr.ForceOneToOne = atr.ForceOneToOne;
							natr.ForceOneToOneSpecified = atr.ForceOneToOneSpecified;
							natr.Name = atr.Name;
							natr.RelateType = atr.RelateType;
							natr.RelateTypeSpecified = atr.RelateTypeSpecified;
							natr.ResourceId = atr.ResourceId;

							if (atr.RelateProperty != null)
							{
								natr.RelateProperty = new RelatePropertyTypeCollection();
								foreach(RelatePropertyType rtp in atr.RelateProperty)
								{
									RelatePropertyType nrtp = new RelatePropertyType();
									nrtp.AttributeClassProperty = rtp.AttributeClassProperty;
									nrtp.FeatureClassProperty = rtp.FeatureClassProperty;
									natr.RelateProperty.Add(nrtp);
								}
							}

							nfex.AttributeRelate.Add(natr);
						}
					}

					m_extension.Add(nfex);

				}

			featureSourceExtension.SetItem(m_editor, m_feature, m_extension);
		}

		private EditExtensions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditExtensions));
            this.featureSourceExtension = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.FeatureSourceExtension();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // featureSourceExtension
            // 
            resources.ApplyResources(this.featureSourceExtension, "featureSourceExtension");
            this.featureSourceExtension.Name = "featureSourceExtension";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            this.panel1.Name = "panel1";
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // EditExtensions
            // 
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.CancelBtn;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.featureSourceExtension);
            this.Name = "EditExtensions";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void OKBtn_Click(object sender, System.EventArgs e)
		{
			m_feature.Extension = m_extension;
			m_editor.HasChanged();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
