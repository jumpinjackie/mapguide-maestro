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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace XsdGenerator
{
	/// <summary>
	/// Summary description for FormNamespace.
	/// </summary>
	public class FormNamespace : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtNamespace;
		private System.Windows.Forms.RadioButton optCS;
		private System.Windows.Forms.RadioButton optVB;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormNamespace()
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtNamespace = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.optCS = new System.Windows.Forms.RadioButton();
			this.optVB = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Namespace";
			// 
			// txtNamespace
			// 
			this.txtNamespace.Location = new System.Drawing.Point(88, 8);
			this.txtNamespace.Name = "txtNamespace";
			this.txtNamespace.Size = new System.Drawing.Size(248, 20);
			this.txtNamespace.TabIndex = 1;
			this.txtNamespace.Text = "default";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(72, 72);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 32);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(184, 72);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 32);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// optCS
			// 
			this.optCS.Checked = true;
			this.optCS.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.optCS.Location = new System.Drawing.Point(8, 32);
			this.optCS.Name = "optCS";
			this.optCS.Size = new System.Drawing.Size(136, 16);
			this.optCS.TabIndex = 4;
			this.optCS.TabStop = true;
			this.optCS.Text = "C#";
			// 
			// optVB
			// 
			this.optVB.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.optVB.Location = new System.Drawing.Point(8, 48);
			this.optVB.Name = "optVB";
			this.optVB.Size = new System.Drawing.Size(136, 16);
			this.optVB.TabIndex = 5;
			this.optVB.Text = "VB";
			// 
			// FormNamespace
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(352, 120);
			this.Controls.Add(this.optVB);
			this.Controls.Add(this.optCS);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtNamespace);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormNamespace";
			this.Text = "Enter .Net namespace";
			this.Load += new System.EventHandler(this.FormNamespace_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormNamespace_Load(object sender, System.EventArgs e)
		{
			txtNamespace.SelectAll();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
		
		}

		public string SelectedNameSpace { get { return txtNamespace.Text; } }
		public System.CodeDom.Compiler.CodeDomProvider SelectedLanguage 
		{ 
			get 
			{
				if (optCS.Checked)
					return new Microsoft.CSharp.CSharpCodeProvider();
				else
					return new Microsoft.VisualBasic.VBCodeProvider();
			}
		}
	}
}
