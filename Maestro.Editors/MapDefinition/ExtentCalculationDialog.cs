#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.MapDefinition
{
    /// <summary>
    /// Displays the progress and result of Map Definition extent calculation
    /// </summary>
    public partial class ExtentCalculationDialog : Form
    {
        private IMapDefinition _mdf;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentCalculationDialog"/> class.
        /// </summary>
        /// <param name="mdf">The MDF.</param>
        public ExtentCalculationDialog(IMapDefinition mdf)
        {
            InitializeComponent();
            _mdf = mdf;
            grdCalculations.DataSource = _results;

            prgCalculations.Maximum = mdf.GetLayerCount();
            if (mdf.BaseMap != null)
                prgCalculations.Maximum += GetLayerCount(mdf.BaseMap);

            txtCoordinateSystem.Text = mdf.CoordinateSystem;
        }

        private int GetLayerCount(IBaseMapDefinition baseMap)
        {
            int count = 0;
            foreach (var grp in baseMap.BaseMapLayerGroup)
            {
                foreach (var layer in grp.BaseMapLayer)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            bgCalculation.RunWorkerAsync(_mdf);
        }

        enum TransformStatus
        {
            Pass,
            Fail
        }

        class CalculationResult
        {
            public TransformStatus Status { get; set; }

            public string LayerDefinition { get; set; }

            public string CoordinateSystem { get; set; }

            public string Extents { get; set; }

            public string TransformedExtents { get; set; }

            [Browsable(false)]
            public IEnvelope TransformedResult { get; set; }

            public string Messages { get; set; }
        }

        private BindingList<CalculationResult> _results = new BindingList<CalculationResult>();

        /// <summary>
        /// Gets the extents.
        /// </summary>
        public IEnvelope Extents
        {
            get
            {
                double llx;
                double lly;
                double urx;
                double ury;
                if (double.TryParse(txtLowerX.Text, out llx) &&
                    double.TryParse(txtLowerY.Text, out lly) &&
                    double.TryParse(txtUpperX.Text, out urx) &&
                    double.TryParse(txtUpperY.Text, out ury))
                {
                    return ObjectFactory.CreateEnvelope(llx, lly, urx, ury);
                }
                return null;
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void bgCalculation_DoWork(object sender, DoWorkEventArgs e)
        {
            var mdf = (IMapDefinition)e.Argument;
            var resSvc = mdf.CurrentConnection.ResourceService;

            //Accumulate layers
            Dictionary<string, ILayerDefinition> layers = new Dictionary<string, ILayerDefinition>();
            foreach (var lyr in mdf.MapLayer)
            {
                if (!layers.ContainsKey(lyr.ResourceId))
                    layers.Add(lyr.ResourceId, (ILayerDefinition)resSvc.GetResource(lyr.ResourceId));
            }
            if (mdf.BaseMap != null)
            {
                foreach (var group in mdf.BaseMap.BaseMapLayerGroup)
                {
                    foreach (var layer in group.BaseMapLayer)
                    {
                        if (!layers.ContainsKey(layer.ResourceId))
                            layers.Add(layer.ResourceId, (ILayerDefinition)resSvc.GetResource(layer.ResourceId));
                    }
                }
            }

            Check.NotNull(layers, "layers");
            int processed = 0;

            //Begin
            foreach (var layer in layers.Values)
            {
                CalculationResult res = new CalculationResult();
                string wkt;
                var e1 = layer.GetSpatialExtent(true, out wkt);
                if (e1 != null)
                {
                    res.Extents = ExtentsToString(e1);
                }
                res.LayerDefinition = layer.ResourceID;
                res.CoordinateSystem = wkt;

                bool tx = false;
                if (wkt != mdf.CoordinateSystem)
                {
                    tx = true;
                    //Transform if not the same, otherwise assume either arbitrary or same as the map
                    if (!string.IsNullOrEmpty(wkt))
                    {
                        e1 = Utility.TransformEnvelope(e1, wkt, mdf.CoordinateSystem);
                        res.TransformedResult = e1;       
                    }
                }
                else
                {
                    res.TransformedResult = e1;
                    res.Messages = Strings.NoTransformationRequired;
                }

                if (e1 != null)
                {
                    res.Status = TransformStatus.Pass;
                    if (tx)
                        res.TransformedExtents = ExtentsToString(e1);
                }
                else
                {
                    res.Status = TransformStatus.Fail;
                    res.Messages = string.Format(Strings.ExtentsTransformationFailed, layer.ResourceID);
                }

                processed++;
                bgCalculation.ReportProgress(processed, res);
            }
        }

        private string ExtentsToString(IEnvelope env)
        {
            Check.NotNull(env, "env");
            return string.Format("[{0} {1}] [{2} {3}]", env.MinX, env.MinY, env.MaxX, env.MaxY);
        }

        private void bgCalculation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            prgCalculations.Value = e.ProgressPercentage;
            var res = (CalculationResult)e.UserState;

            var env = this.Extents;
            if (env == null)
            {
                if (res.Status == TransformStatus.Pass && res.TransformedResult != null)
                {
                    txtLowerX.Text = res.TransformedResult.MinX.ToString();
                    txtLowerY.Text = res.TransformedResult.MinY.ToString();
                    txtUpperX.Text = res.TransformedResult.MaxX.ToString();
                    txtUpperY.Text = res.TransformedResult.MaxY.ToString();
                }
            }
            else
            {
                if (res.Status == TransformStatus.Pass && res.TransformedResult != null)
                {
                    env.ExpandToInclude(res.TransformedResult);

                    txtLowerX.Text = env.MinX.ToString();
                    txtLowerY.Text = env.MinY.ToString();
                    txtUpperX.Text = env.MaxX.ToString();
                    txtUpperY.Text = env.MaxY.ToString();
                }
            }

            _results.Add(res);
        }

        private void bgCalculation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblMessage.Text = Strings.ExtentsCalculationCompleted;
            btnAccept.Enabled = btnClose.Enabled = true;
        }
    }
}
