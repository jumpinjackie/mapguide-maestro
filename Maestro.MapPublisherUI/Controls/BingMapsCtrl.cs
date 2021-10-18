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
    public partial class BingMapsCtrl : UserControl
    {
        public BingMapsCtrl()
        {
            InitializeComponent();
        }

        public BingMapsBaseLayer Layer { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            cmbLayerType.DataSource = new[]
            {
                "Aerial",
                "AerialWithLabelsOnDemand",
                "RoadOnDemand",
                "CanvasDark",
                "OrdnanceSurvey"
            };
            cmbLayerType.SelectedItem = this.Layer.LayerType;
            txtApiKey.Text = this.Layer.ApiKey;
            base.OnLoad(e);
        }
    }
}
