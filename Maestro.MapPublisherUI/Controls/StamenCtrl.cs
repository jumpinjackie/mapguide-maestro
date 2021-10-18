using Maestro.MapPublisher.Common;
using System;
using System.Windows.Forms;

namespace Maestro.MapPublisherUI.Controls
{
    public partial class StamenCtrl : UserControl
    {
        public StamenCtrl()
        {
            InitializeComponent();
        }

        public StamenBaseLayer Layer { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            cmbStamenType.DataSource = Enum.GetValues(typeof(StamenLayerType));
            cmbStamenType.SelectedItem = this.Layer.LayerType;
            base.OnLoad(e);
        }

        private void cmbStamenType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Layer != null)
            {
                this.Layer.LayerType = (StamenLayerType)cmbStamenType.SelectedItem;
            }
        }
    }
}
