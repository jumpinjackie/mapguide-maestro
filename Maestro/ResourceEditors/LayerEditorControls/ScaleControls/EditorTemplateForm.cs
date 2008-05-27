using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls
{
    public partial class EditorTemplateForm : Form
    {
        public EditorTemplateForm()
        {
            InitializeComponent();
        }

        public void RefreshSize()
        {
            if (ItemPanel.Controls.Count > 0 && ItemPanel.Controls[0] as UserControl != null)
            {
                this.Height = ButtonPanel.Height + ItemPanel.Top + (ItemPanel.Controls[0] as UserControl).AutoScrollMinSize.Height + (8 * 4);

                this.Width = Math.Max(this.Width, (ItemPanel.Controls[0] as UserControl).AutoScrollMinSize.Width + 2 * ItemPanel.Left + (8 * 4));
            }
        }

        private void EditorTemplateForm_Load(object sender, EventArgs e)
        {
            RefreshSize();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}