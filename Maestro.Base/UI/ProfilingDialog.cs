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
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Shared.UI;
using ICSharpCode.Core;

namespace Maestro.Base.UI
{
    /// <summary>
    /// A dialog that profiles a given resource for performance
    /// </summary>
    public partial class ProfilingDialog : Form
    {
        private IServerConnection m_connection;
        private IResource m_item;
        private string m_resourceId;

        private RuntimeMap m_tempmap;

        /// <summary>
        /// Initializes a new instance of the ProfilingDialog class
        /// </summary>
        /// <param name="item"></param>
        /// <param name="resourceId"></param>
        /// <param name="connection"></param>
        public ProfilingDialog(IResource item, string resourceId, IServerConnection connection)
            : this()
        {
            m_connection = connection;
            m_item = item;
            m_resourceId = resourceId;
        }

        private ProfilingDialog()
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

        private void ProfileFeatureSource(IFeatureSource fs)
        {
            //TODO: Determine what profiling benchmarks to use
            string resourceId = fs == m_item ? m_resourceId : fs.ResourceID;
            backgroundWorker.ReportProgress(0, (string.Format(Strings.Prof_LogMessageFeatureSource, resourceId)));
        }

        private static void SetTempLayer(IMapDefinition mdf, string resourceId)
        {
            var layers = new List<IMapLayer>(mdf.MapLayer);
            for (int i = 0; i < layers.Count; i++)
            {
                mdf.RemoveLayer(layers[i]);
            }

            var layer = mdf.AddLayer(null, "x", resourceId); //NOXLATE
            layer.Visible = true;
            layer.Selectable = true;
            layer.Name = "x"; //NOXLATE
            layer.LegendLabel = string.Empty;
        }

        private void MakeTempMap()
        {
            if (m_tempmap == null)
            {
                IMapDefinition m = ObjectFactory.CreateMapDefinition(m_connection, string.Empty);
                m.CoordinateSystem = @"LOCAL_CS[""*XY-M*"", LOCAL_DATUM[""*X-Y*"", 10000], UNIT[""Meter"", 1], AXIS[""X"", EAST], AXIS[""Y"", NORTH]]"; //NOXLATE
                m.SetExtents(-1, -1, 1, 1);

                //AIMS 2012 demands this checks out. Can't flub it anymore
                m.ResourceID = "Session:" + m_connection.SessionID + "//non-existing.MapDefinition"; //NOXLATE
                var mpsvc = (IMappingService)m_connection.GetService((int)ServiceType.Mapping);
                var rid = new ResourceIdentifier(Guid.NewGuid().ToString(), ResourceTypes.RuntimeMap, m_connection.SessionID);

                m_tempmap = mpsvc.CreateMap(m);
            }
        }


        private void ProfileLayerDefinition(ILayerDefinition ldef)
        {
            //TODO: This was a line-by-line port from 2.x to match the 3.x APIs
            //we should find time to clean this up and ensure the profiling numbers are
            //truly reflective of actual performance metrics
            if (backgroundWorker.CancellationPending)
                return;

            string resourceId = ldef == m_item ? m_resourceId : ldef.ResourceID;

            MakeTempMap();

            backgroundWorker.ReportProgress(0, (string.Format(Strings.Prof_LogMessageLayerDefinition, resourceId)));
            using (new Timer(Strings.Prof_LogMessageRuntimeLayer, backgroundWorker))
            {
                try
                {
                    IMapDefinition mdef = ObjectFactory.CreateMapDefinition(m_connection, string.Empty);
                    //We cannot flub this anymore. AIMS 2012 demands the Map Definition id specified checks out
                    mdef.ResourceID = "Session:" + m_connection.SessionID + "//ProfileTest.MapDefinition"; //NOXLATE
                    m_connection.ResourceService.SaveResource(mdef);
                    IMapLayer layer = mdef.AddLayer(null, "Test Layer", ldef.ResourceID); //NOXLATE
                    layer.Visible = false;
                    layer.Selectable = false;

                    if (backgroundWorker.CancellationPending)
                        return;

                    var mpsvc = (IMappingService)m_connection.GetService((int)ServiceType.Mapping);
                    
                    var map = mpsvc.CreateMap(mdef);
                    using (new Timer(Strings.Prof_LogMessageIdentifyFetching, backgroundWorker))
                    {
                        var rtl = map.Layers["Test Layer"]; //NOXLATE
                        rtl.Visible = true;
                        rtl.Selectable = true;
                    }

                    map.Save();
                }
                catch (Exception ex)
                {
                    //string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    backgroundWorker.ReportProgress(0, (string.Format(Strings.Prof_LayerDefinitionProfilingError, resourceId, ex.ToString(), Environment.NewLine)));
                }
            }

            if (backgroundWorker.CancellationPending)
                return;

            ILayerDefinition lx = (ILayerDefinition)ldef.Clone();
            if (lx.SubLayer.LayerType == LayerType.Vector || lx.SubLayer.LayerType == LayerType.Raster)
            {
                using (new Timer(Strings.Prof_LogMessageRenderingScales, backgroundWorker))
                {
                    if (lx.SubLayer.LayerType == LayerType.Vector)
                    {
                        IVectorLayerDefinition vlx = lx.SubLayer as IVectorLayerDefinition;
                        //VectorScaleRangeTypeCollection ranges = vlx.VectorScaleRange;
                        List<IVectorScaleRange> ranges = new List<IVectorScaleRange>(vlx.VectorScaleRange);
                        foreach (var vsr in ranges)
                        {
                            if (backgroundWorker.CancellationPending)
                                return;

                            string tmp1 = new ResourceIdentifier(Guid.NewGuid().ToString(), ResourceTypes.LayerDefinition, m_connection.SessionID);

                            try
                            {
                                double minscale = vsr.MinScale.HasValue ? vsr.MinScale.Value : 0;
                                double maxscale = vsr.MaxScale.HasValue ? vsr.MaxScale.Value : 10000000;

                                vlx.RemoveAllScaleRanges();
                                vsr.MinScale = null;
                                vsr.MaxScale = null;
                                vlx.AddVectorScaleRange(vsr);

                                m_connection.ResourceService.SaveResourceAs(lx, tmp1);

                                if (backgroundWorker.CancellationPending)
                                    return;

                                var lst = m_connection.FeatureService.GetSpatialContextInfo(vlx.ResourceId, false);

                                //Create a runtime map just containing this particular layer at this particular scale range
                                //We are profiling the stylization settings for this layer
                                var mdf = ObjectFactory.CreateMapDefinition(m_connection, "");
                                if (lst.SpatialContext != null && lst.SpatialContext.Count >= 1)
                                {
                                    mdf.CoordinateSystem = lst.SpatialContext[0].CoordinateSystemWkt;
                                    if (string.IsNullOrEmpty(m_tempmap.CoordinateSystem))
                                        mdf.CoordinateSystem = @"LOCAL_CS[""*XY-M*"", LOCAL_DATUM[""*X-Y*"", 10000], UNIT[""Meter"", 1], AXIS[""X"", EAST], AXIS[""Y"", NORTH]]"; //NOXLATE
                                    
                                    double llx = double.Parse(lst.SpatialContext[0].Extent.LowerLeftCoordinate.X, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                                    double lly = double.Parse(lst.SpatialContext[0].Extent.LowerLeftCoordinate.Y, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); ;
                                    double urx = double.Parse(lst.SpatialContext[0].Extent.UpperRightCoordinate.X, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); ;
                                    double ury = double.Parse(lst.SpatialContext[0].Extent.UpperRightCoordinate.Y, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); ;

                                    m_tempmap.DataExtent = ObjectFactory.CreateEnvelope(llx, lly, urx, ury);
                                }

                                SetTempLayer(mdf, tmp1);
                                
                                var mpsvc = (IMappingService)m_connection.GetService((int)ServiceType.Mapping);
                                //We cannot flub this anymore. AIMS 2012 demands the Map Definition id specified checks out
                                mdf.ResourceID = "Session:" + m_connection.SessionID + "//ProfileTest.MapDefinition"; //NOXLATE
                                m_connection.ResourceService.SaveResource(mdf);
                                var rtmap = mpsvc.CreateMap(mdf);

                                if (m_connection.ResourceService.ResourceExists(rtmap.ResourceID))
                                    m_connection.ResourceService.DeleteResource(rtmap.ResourceID);

                                rtmap.Save();

                                if (backgroundWorker.CancellationPending)
                                    return;

                                using (new Timer(string.Format(Strings.Prof_LogMessageScaleRange, minscale, maxscale), backgroundWorker))
                                {
                                    //TODO: Use extents rather than scale
                                    //using (System.IO.Stream s = m_connection.RenderRuntimeMap(tmp2, m.Extents, 1024, 800, 96))
                                    using (System.IO.Stream s = mpsvc.RenderRuntimeMap(rtmap, ((rtmap.DataExtent.MaxX - rtmap.DataExtent.MinX) / 2) + rtmap.DataExtent.MinX, ((rtmap.DataExtent.MaxY - rtmap.DataExtent.MinY) / 2) + rtmap.DataExtent.MinY, 50000, 1024, 800, 96))
                                    {
                                        backgroundWorker.ReportProgress(0, (string.Format(Strings.Prof_MapRenderingImageSize, s.Length)));
                                    }
                                }
                            }
                            finally
                            {
                                try { m_connection.ResourceService.DeleteResource(tmp1); }
                                catch { }
                            }
                        }

                    }
                }
            }

            if (backgroundWorker.CancellationPending)
                return;

            backgroundWorker.ReportProgress(0, ("\r\n")); //NOXLATE
        }

        private void ProfileMapDefinition(IMapDefinition mapDef)
        {
            //TODO: This was a line-by-line port from 2.x to match the 3.x APIs
            //we should find time to clean this up and ensure the profiling numbers are
            //truly reflective of actual performance metrics
            var mdef = (IMapDefinition)mapDef.Clone();
            if (backgroundWorker.CancellationPending)
                return;

            string resourceId = mdef == m_item ? m_resourceId : mdef.ResourceID;

            backgroundWorker.ReportProgress(0, (string.Format(Strings.Prof_LogMessageMapDefinition, resourceId)));

            using (new Timer(Strings.Prof_LogMessageRuntimeMap, backgroundWorker))
            {
                foreach (var ml in mdef.MapLayer)
                {
                    try
                    {
                        if (backgroundWorker.CancellationPending)
                            return;

                        ILayerDefinition ldef = (ILayerDefinition)mdef.CurrentConnection.ResourceService.GetResource(ml.ResourceId);
                        ProfileLayerDefinition(ldef);
                    }
                    catch (Exception ex)
                    {
                        //string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                        backgroundWorker.ReportProgress(0, (string.Format(Strings.Prof_LayerDefinitionProfilingError, ml.ResourceId, ex.ToString(), Environment.NewLine)));
                    }
                }

                if (mdef.BaseMap != null)
                {
                    foreach (var g in mdef.BaseMap.BaseMapLayerGroup)
                    {
                        if (g.BaseMapLayer != null)
                        {
                            foreach (var ml in g.BaseMapLayer)
                            {
                                try
                                {
                                    if (backgroundWorker.CancellationPending)
                                        return;

                                    ILayerDefinition ldef = (ILayerDefinition)mdef.CurrentConnection.ResourceService.GetResource(ml.ResourceId);
                                    ProfileLayerDefinition(ldef);
                                }
                                catch (Exception ex)
                                {
                                    //string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                                    backgroundWorker.ReportProgress(0, (string.Format(Strings.Prof_LayerDefinitionProfilingError, ml.ResourceId, ex.ToString(), Environment.NewLine)));
                                }
                            }
                        }
                    }
                }
            }

            if (backgroundWorker.CancellationPending)
                return;

            var mpsvc = (IMappingService)m_connection.GetService((int)ServiceType.Mapping);

            try
            {
                if (backgroundWorker.CancellationPending)
                    return;

                //m_connection.ResetFeatureSourceSchemaCache();
                using (new Timer(Strings.Prof_LogMessageRuntimeMapTotal, backgroundWorker))
                    mpsvc.CreateMap(mdef);
            }
            catch (Exception ex)
            {
                //string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                backgroundWorker.ReportProgress(0, (string.Format(Strings.Prof_RuntimeMapProfilingError, resourceId, ex.ToString(), Environment.NewLine)));
            }

            try
            {
                if (backgroundWorker.CancellationPending)
                    return;

                //We cannot flub this anymore. AIMS 2012 demands the Map Definition id specified checks out
                mdef.ResourceID = "Session:" + m_connection.SessionID + "//ProfilingTest.MapDefinition"; //NOXLATE
                m_connection.ResourceService.SaveResource(mdef);

                var rtmap = mpsvc.CreateMap(mdef);

                if (m_connection.ResourceService.ResourceExists(rtmap.ResourceID))
                    m_connection.ResourceService.DeleteResource(rtmap.ResourceID);

                rtmap.Save();

                using (new Timer(Strings.Prof_LogMessageRenderingMap, backgroundWorker))
                {
                    //TODO: Use extents rather than scale
                    //using (System.IO.Stream s = m_connection.RenderRuntimeMap(tmp2, mdef.Extents, 1024, 800, 96))
                    using (System.IO.Stream s = mpsvc.RenderRuntimeMap(rtmap, ((mdef.Extents.MaxX - mdef.Extents.MinX) / 2) + mdef.Extents.MinX, ((mdef.Extents.MaxY - mdef.Extents.MinY) / 2) + mdef.Extents.MinY, 50000, 1024, 800, 96))
                    {
                        //Just dispose it after being read
                        backgroundWorker.ReportProgress(0, (string.Format(Strings.Prof_MapRenderingImageSize, s.Length)));
                    }
                }
            }
            catch (Exception ex)
            {
                //string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                backgroundWorker.ReportProgress(0, (string.Format(Strings.Prof_MapRenderingError, resourceId, ex.ToString(), Environment.NewLine)));
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
                    m_worker.ReportProgress(0, (m_text + ts.TotalMilliseconds));
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
            if (m_item.ResourceType == ResourceTypes.FeatureSource)
            {
                ProfileFeatureSource(m_item as IFeatureSource);
            }
            else if (m_item.ResourceType == ResourceTypes.LayerDefinition)
            {
                ProfileLayerDefinition(m_item as ILayerDefinition);
            }
            else if (m_item.ResourceType == ResourceTypes.MapDefinition)
            {
                ProfileMapDefinition(m_item as IMapDefinition);
            }
            else
            {
                backgroundWorker.ReportProgress(0, Strings.Prof_LogMessageUnsupportedResourceType);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CancelBtn.Text = Strings.CloseButtonText;
            btnSave.Enabled = true;
            WriteString(Strings.Prof_LogMessageDone);
        }

        private void WriteString(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            bool scroll = Math.Abs((Results.SelectionStart + Results.SelectionLength) - Results.Text.Length) < 20;

            Results.Text += message + "\r\n"; //NOXLATE
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var diag = DialogFactory.SaveFile())
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(diag.FileName, Results.Text);
                    MessageService.ShowMessage(string.Format(Strings.Log_Saved, diag.FileName));
                }
            }
        }

    }
}
