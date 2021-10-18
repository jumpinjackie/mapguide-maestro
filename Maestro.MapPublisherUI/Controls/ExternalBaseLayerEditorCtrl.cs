using Maestro.MapPublisher.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maestro.MapPublisherUI.Controls
{
    public partial class ExternalBaseLayerEditorCtrl : UserControl
    {
        public ExternalBaseLayerEditorCtrl()
        {
            InitializeComponent();
        }

        public ExternalBaseLayer Layer { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            txtName.Text = this.Layer?.Name;
            panelBody.Controls.Clear();
            switch (this.Layer?.Type)
            {
                case ExternalBaseLayerType.Stamen:
                    panelBody.Controls.Add(new StamenCtrl { Layer = (StamenBaseLayer)this.Layer, Dock = DockStyle.Fill });
                    break;
                case ExternalBaseLayerType.OSM:
                    break;
                case ExternalBaseLayerType.BingMaps:
                    panelBody.Controls.Add(new BingMapsCtrl { Layer = (BingMapsBaseLayer)this.Layer, Dock = DockStyle.Fill });
                    break;
                case ExternalBaseLayerType.XYZ:
                    break;
            }

            base.OnLoad(e);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (this.Layer != null)
                this.Layer.Name = txtName.Text;
        }
    }
}
