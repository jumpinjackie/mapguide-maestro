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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro
{
    public partial class Profiling : Form
    {
        private ServerConnectionI m_connection;
        private object m_item;
        private string m_resourceId;

        private OSGeo.MapGuide.MaestroAPI.RuntimeClasses.RuntimeMap m_tempmap;


        public Profiling(object item, string resourceId, ServerConnectionI connection)
            : this()
        {
            m_connection = connection;
            m_item = item;
            m_resourceId = resourceId;
        }

        private Profiling()
        {
            InitializeComponent();
            m_tempmap = null;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
                this.Close();
            else if (!backgroundWorker.CancellationPending)
                backgroundWorker.CancelAsync();
        }

        private void ProfileFeatureSource(FeatureSource fs)
        {
            string resourceId = fs == m_item ? m_resourceId : fs.ResourceId;
            backgroundWorker.ReportProgress(0, (string.Format(Strings.Profiling.LogMessageFeatureSource, resourceId)));
        }

        private void SetTempLayer(string resourceId)
        {
            m_tempmap.Layers.Clear();

            MapDefinition m = new MapDefinition();
            m.CurrentConnection = m_connection;
            MapLayerType mlt = new MapLayerType();
            mlt.Visible = true;
            mlt.Selectable = true;
            mlt.ResourceId = resourceId;
            mlt.Name = "x";
            mlt.LegendLabel = "";
            mlt.Parent = m;

            m_tempmap.Layers.Add(new OSGeo.MapGuide.MaestroAPI.RuntimeClasses.RuntimeMapLayer(mlt));
        }

        private void MakeTempMap()
        {
            if (m_tempmap == null)
            {
                MapDefinition m = new MapDefinition();
                m.BackgroundColor = Color.White;
                m.CoordinateSystem = @"LOCAL_CS[""*XY-M*"", LOCAL_DATUM[""*X-Y*"", 10000], UNIT[""Meter"", 1], AXIS[""X"", EAST], AXIS[""Y"", NORTH]]";
                m.Extents = new OSGeo.MapGuide.MaestroAPI.Box2DType();
                m.Extents.MinX = -1;
                m.Extents.MinY = -1;
                m.Extents.MaxX = 1;
                m.Extents.MaxY = 1;
                
                m.CurrentConnection = m_connection;
                m.ResourceId = "Library://non-existing.MapDefinition";
                m_tempmap = new OSGeo.MapGuide.MaestroAPI.RuntimeClasses.RuntimeMap(m);
                m_tempmap.ResourceID = new ResourceIdentifier(Guid.NewGuid().ToString(), ResourceTypes.RuntimeMap, m_connection.SessionID);
                m_connection.CreateRuntimeMap(m_tempmap.ResourceID, m_tempmap);
            }
        }


        private void ProfileLayerDefinition(LayerDefinition ldef)
        {
            if (backgroundWorker.CancellationPending)
                return;

            string resourceId = ldef == m_item ? m_resourceId : ldef.ResourceId;

            MakeTempMap();

            backgroundWorker.ReportProgress(0, (string.Format(Strings.Profiling.LogMessageLayerDefinition, resourceId)));
            using (new Timer(Strings.Profiling.LogMessageRuntimeLayer, backgroundWorker))
            {
                try
                {
                    MapDefinition mdef = new MapDefinition();
                    mdef.CurrentConnection = m_connection;
                    MapLayerType mltype = new MapLayerType();
                    mltype.ResourceId = ldef.ResourceId;
                    mltype.Visible = false;
                    mltype.Selectable = false;
                    mdef.Layers.Add(mltype);
                    m_connection.ResetFeatureSourceSchemaCache();

                    if (backgroundWorker.CancellationPending)
                        return;

                    OSGeo.MapGuide.MaestroAPI.RuntimeClasses.RuntimeMap map = new OSGeo.MapGuide.MaestroAPI.RuntimeClasses.RuntimeMap(mdef);
                    using (new Timer(Strings.Profiling.LogMessageIdentifyFetching, backgroundWorker))
                    {
                        //map.Layers[0].Parent = map;
                        map.Layers[0].Visible = true;
                        map.Layers[0].Selectable = true;
                    }
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    backgroundWorker.ReportProgress(0, (string.Format(Strings.Profiling.LayerDefinitionProfilingError, resourceId, msg)));
                }
            }

            if (backgroundWorker.CancellationPending)
                return;

            LayerDefinition lx = (LayerDefinition)Utility.XmlDeepCopy(ldef);
            if (lx.Item as VectorLayerDefinitionType != null || lx.Item as GridLayerDefinitionType != null)
            {
                using (new Timer(Strings.Profiling.LogMessageRenderingScales, backgroundWorker))
                {
                    if (lx.Item as VectorLayerDefinitionType != null)
                    {
                        VectorLayerDefinitionType vlx = lx.Item as VectorLayerDefinitionType;
                        VectorScaleRangeTypeCollection ranges = vlx.VectorScaleRange;

                        foreach (VectorScaleRangeType vsr in ranges)
                        {
                            if (backgroundWorker.CancellationPending)
                                return;

                            string tmp1 = new ResourceIdentifier(Guid.NewGuid().ToString(), ResourceTypes.LayerDefiniton, m_connection.SessionID);

                            try
                            {
                                double minscale = vsr.MinScaleSpecified ? vsr.MinScale : 0;
                                double maxscale = vsr.MaxScaleSpecified ? vsr.MaxScale : 10000000;

                                vlx.VectorScaleRange = new VectorScaleRangeTypeCollection();
                                vsr.MaxScaleSpecified = false;
                                vsr.MinScaleSpecified = false;
                                vlx.VectorScaleRange.Add(vsr);

                                m_connection.SaveResourceAs(lx, tmp1);

                                if (backgroundWorker.CancellationPending)
                                    return;

                                OSGeo.MapGuide.MaestroAPI.FdoSpatialContextList lst = m_connection.GetSpatialContextInfo(vlx.ResourceId, false);

                                if (lst.SpatialContext != null && lst.SpatialContext.Count >= 1)
                                {
                                    m_tempmap.CoordinateSystem = lst.SpatialContext[0].CoordinateSystemWkt;
                                    if (string.IsNullOrEmpty(m_tempmap.CoordinateSystem))
                                        m_tempmap.CoordinateSystem = @"LOCAL_CS[""*XY-M*"", LOCAL_DATUM[""*X-Y*"", 10000], UNIT[""Meter"", 1], AXIS[""X"", EAST], AXIS[""Y"", NORTH]]";
                                    m_tempmap.Extents = new OSGeo.MapGuide.MaestroAPI.Box2DType();
                                    m_tempmap.Extents.MinX = double.Parse(lst.SpatialContext[0].Extent.LowerLeftCoordinate.X, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                                    m_tempmap.Extents.MinY = double.Parse(lst.SpatialContext[0].Extent.LowerLeftCoordinate.Y, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); ;
                                    m_tempmap.Extents.MaxX = double.Parse(lst.SpatialContext[0].Extent.UpperRightCoordinate.X, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); ;
                                    m_tempmap.Extents.MaxY = double.Parse(lst.SpatialContext[0].Extent.UpperRightCoordinate.Y, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); ;
                                }

                                SetTempLayer(tmp1);
                                m_connection.SaveRuntimeMap(m_tempmap.ResourceID, m_tempmap);

                                if (backgroundWorker.CancellationPending)
                                    return;

                                using (new Timer(string.Format(Strings.Profiling.LogMessageScaleRange, minscale, maxscale), backgroundWorker))
                                {
                                    //TODO: Use extents rather than scale
                                    //using (System.IO.Stream s = m_connection.RenderRuntimeMap(tmp2, m.Extents, 1024, 800, 96))
                                    using (System.IO.Stream s = m_connection.RenderRuntimeMap(m_tempmap.ResourceID, ((m_tempmap.Extents.MaxX - m_tempmap.Extents.MinX) / 2) + m_tempmap.Extents.MinX, ((m_tempmap.Extents.MaxY - m_tempmap.Extents.MinY) / 2) + m_tempmap.Extents.MinY, 50000, 1024, 800, 96))
                                        new Bitmap(s).Dispose();
                                }
                            }
                            finally
                            {
                                try { m_connection.DeleteResource(tmp1); }
                                catch { }
                            }
                        }

                    }
                }
            }

            if (backgroundWorker.CancellationPending)
                return;

            backgroundWorker.ReportProgress(0, ("\r\n"));
        }

        private void ProfileMapDefinition(MapDefinition mdef)
        {
            if (backgroundWorker.CancellationPending)
                return;

            string resourceId = mdef == m_item ? m_resourceId : mdef.ResourceId;

            backgroundWorker.ReportProgress(0, (string.Format(Strings.Profiling.LogMessageMapDefinition, resourceId)));

            using (new Timer(Strings.Profiling.LogMessageRuntimeMap, backgroundWorker))
            {
                foreach (MapLayerType ml in mdef.Layers)
                {
                    try
                    {
                        if (backgroundWorker.CancellationPending)
                            return;

                        LayerDefinition ldef = mdef.CurrentConnection.GetLayerDefinition(ml.ResourceId);
                        ProfileLayerDefinition(ldef);
                    }
                    catch (Exception ex)
                    {
                        string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                        backgroundWorker.ReportProgress(0, (string.Format(Strings.Profiling.LayerDefinitionProfilingError, ml.ResourceId, msg)));
                    }
                }

                if (mdef.BaseMapDefinition != null && mdef.BaseMapDefinition.BaseMapLayerGroup != null)
                {
                    foreach (BaseMapLayerGroupCommonType g in mdef.BaseMapDefinition.BaseMapLayerGroup)
                    {
                        if (g.BaseMapLayer != null)
                        {
                            foreach (BaseMapLayerType ml in g.BaseMapLayer)
                            {
                                try
                                {
                                    if (backgroundWorker.CancellationPending)
                                        return;

                                    LayerDefinition ldef = mdef.CurrentConnection.GetLayerDefinition(ml.ResourceId);
                                    ProfileLayerDefinition(ldef);
                                }
                                catch (Exception ex)
                                {
                                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                                    backgroundWorker.ReportProgress(0, (string.Format(Strings.Profiling.LayerDefinitionProfilingError, ml.ResourceId, msg)));
                                }
                            }
                        }
                    }
                }
            }

            if (backgroundWorker.CancellationPending)
                return;

            try
            {
                if (backgroundWorker.CancellationPending)
                    return;

                m_connection.ResetFeatureSourceSchemaCache();
                using (new Timer(Strings.Profiling.LogMessageRuntimeMapTotal, backgroundWorker))
                    new OSGeo.MapGuide.MaestroAPI.RuntimeClasses.RuntimeMap(mdef);
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                backgroundWorker.ReportProgress(0, (string.Format(Strings.Profiling.RuntimeMapProfilingError, resourceId, msg)));
            }

            string tmp2 = new ResourceIdentifier(Guid.NewGuid().ToString(), ResourceTypes.RuntimeMap, m_connection.SessionID);

            try
            {
                if (backgroundWorker.CancellationPending)
                    return;

                m_connection.CreateRuntimeMap(tmp2, mdef);
                using (new Timer(Strings.Profiling.LogMessageRenderingMap, backgroundWorker))
                {
                    //TODO: Use extents rather than scale
                    //using (System.IO.Stream s = m_connection.RenderRuntimeMap(tmp2, mdef.Extents, 1024, 800, 96))
                    using (System.IO.Stream s = m_connection.RenderRuntimeMap(tmp2, ((mdef.Extents.MaxX - mdef.Extents.MinX) / 2) + mdef.Extents.MinX, ((mdef.Extents.MaxY - mdef.Extents.MinY) / 2) + mdef.Extents.MinY, 50000, 1024, 800, 96))
                    using (Bitmap b = new Bitmap(s))
                    {
                        //Just dispose it after being read
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                backgroundWorker.ReportProgress(0, (string.Format(Strings.Profiling.MapRenderingError, resourceId, msg)));
            }
            finally
            {
                try { m_connection.DeleteResource(tmp2); }
                catch { }
            }

        }

        private class Timer : IDisposable 
        {
            private string m_text;
            private bool m_isDisposed;
            private DateTime m_begin;
            private BackgroundWorker m_worker;

            public Timer(string text, BackgroundWorker worker)
            {
                m_begin = DateTime.Now;
                m_isDisposed = false;
                m_text = text;
                m_worker = worker;
            }

            #region IDisposable Members

            public void Dispose()
            {
                if (!m_isDisposed)
                {
                    TimeSpan ts = DateTime.Now - m_begin;
                    m_isDisposed = true;
                    m_worker.ReportProgress(0, (m_text + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00") + "." + ts.Milliseconds.ToString("000")));
                    m_worker = null;
                    m_text = null;
                }
            }

            #endregion
        }

        private void Profiling_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void Profiling_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                e.Cancel = true;

                if (!backgroundWorker.CancellationPending)
                    backgroundWorker.CancelAsync();
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (m_item as FeatureSource != null)
            {
                ProfileFeatureSource(m_item as FeatureSource);
            }
            else if (m_item as LayerDefinition != null)
            {
                ProfileLayerDefinition(m_item as LayerDefinition);
            }
            else if (m_item as MapDefinition != null)
            {
                ProfileMapDefinition(m_item as MapDefinition);
            }
            else
            {
                backgroundWorker.ReportProgress(0, Strings.Profiling.LogMessageUnsupportedResourceType);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CancelBtn.Text = Strings.Profiling.CloseButtonText;
            WriteString(Strings.Profiling.LogMessageDone);
        }

        private void WriteString(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            bool scroll = Math.Abs((Results.SelectionStart + Results.SelectionLength) - Results.Text.Length) < 20;

            Results.Text += message + "\r\n";
            if (scroll)
            {
                Results.SelectionLength = 0;
                Results.SelectionStart = Results.Text.Length;
                Results.ScrollToCaret();
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            WriteString(e.UserState as string);
        }

    }
}