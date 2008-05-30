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
		private Globalizator.Globalizator m_globalizor;

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

			featureSourceExtension.SetItem(m_editor, m_feature, m_extension, m_globalizor);
		}

		private EditExtensions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			m_globalizor = new Globalizator.Globalizator(this);
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
            this.featureSourceExtension = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.FeatureSourceExtension();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // featureSourceExtension
            // 
            this.featureSourceExtension.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.featureSourceExtension.Location = new System.Drawing.Point(8, 8);
            this.featureSourceExtension.Name = "featureSourceExtension";
            this.featureSourceExtension.Size = new System.Drawing.Size(608, 336);
            this.featureSourceExtension.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            this.panel1.Location = new System.Drawing.Point(8, 352);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(608, 24);
            this.panel1.TabIndex = 1;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(304, 0);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(80, 24);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.OKBtn.Location = new System.Drawing.Point(216, 0);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(80, 24);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // EditExtensions
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(624, 381);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.featureSourceExtension);
            this.Name = "EditExtensions";
            this.Text = "Edit feature source extensions";
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
