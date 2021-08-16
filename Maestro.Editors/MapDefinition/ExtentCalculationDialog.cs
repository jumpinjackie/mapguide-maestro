#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Maestro.Editors.MapDefinition
{
    /// <summary>
    /// Displays the progress and result of Map Definition extent calculation
    /// </summary>
    public partial class ExtentCalculationDialog : Form
    {
        private string _coordSys;
        private Func<IEnumerable<string>> _layerIdCollector;
        private IServerConnection _conn;

        private IList<string> _layerIdsToProcess;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentCalculationDialog"/> class.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="coordinateSystem"></param>
        /// <param name="layerIdCollector"></param>
        public ExtentCalculationDialog(IServerConnection conn, string coordinateSystem, Func<IEnumerable<string>> layerIdCollector)
        {
            InitializeComponent();
            _coordSys = coordinateSystem;
            _layerIdCollector = layerIdCollector;
            _conn = conn;
            grdCalculations.DataSource = _results;
            txtCoordinateSystem.Text = _coordSys;
        }

        private int GetLayerCount(IBaseMapDefinition baseMap)
        {
            int count = 0;
            foreach (var grp in baseMap.BaseMapLayerGroups)
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
            _layerIdsToProcess = _layerIdCollector().Distinct().ToList();
            prgCalculations.Maximum = _layerIdsToProcess.Count;
            bgCalculation.RunWorkerAsync();
        }

        private enum TransformStatus
        {
            Pass,
            Fail
        }

        private class CalculationResult
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
            var resSvc = _conn.ResourceService;

            //Accumulate layers
            Dictionary<string, ILayerDefinition> layers = new Dictionary<string, ILayerDefinition>();
            foreach (var ldfId in _layerIdsToProcess)
            {
                layers.Add(ldfId, (ILayerDefinition)resSvc.GetResource(ldfId));
            }

            Check.ArgumentNotNull(layers, nameof(layers));
            int processed = 0;

            //Begin
            foreach (var layer in layers.Values)
            {
                CalculationResult res = new CalculationResult();
                string wkt;
                var e1 = layer.GetSpatialExtent(_conn, true, out wkt);
                if (e1 != null)
                {
                    var epsg = _conn.CoordinateSystemCatalog.ConvertWktToEpsgCode(wkt);
                    if (epsg == "4326")
                    {
                        // Ensure that EPSG:4326 bounds are coerced down to [-180, -90, 180, 90]
                        // if they exceed it.
                        //
                        // We expect EPSG:4326 data sources are common enough that we should give
                        // this case special treatment
                        e1 = ObjectFactory.CreateEnvelope(
                            Math.Max(e1.MinX, -180),
                            Math.Max(e1.MinY, -90),
                            Math.Min(e1.MaxX, 180),
                            Math.Min(e1.MaxY, 90));
                    }

                    res.Extents = ExtentsToString(e1);
                }
                res.LayerDefinition = layer.ResourceID;
                res.CoordinateSystem = wkt;

                bool tx = false;
                if (wkt != _coordSys)
                {
                    tx = true;
                    //Transform if not the same, otherwise assume either arbitrary or same as the map
                    if (!string.IsNullOrEmpty(wkt))
                    {
                        e1 = Utility.TransformEnvelope(_conn.CoordinateSystemCatalog, e1, wkt, _coordSys);
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
            Check.ArgumentNotNull(env, nameof(env));
            return $"[{env.MinX} {env.MinY}] [{env.MaxX} {env.MaxY}]";
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